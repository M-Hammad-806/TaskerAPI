using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.ObjectPool;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TaskerApi.Data;
using TaskerApi.DTOS;
using TaskerApi.Models;

namespace TaskerApi.Services
{
    public class UserAuthService(AppDbContext context, IConfiguration config,ILogger<UserAuthService> logger) : IUserAuthService
    {
        public async Task<ServiceResult<RegisterResponseDto>> Register(RegisterRequestDto request)
        {
            if (await context.Users.AnyAsync(u => u.Email == request.Email))
                return new ServiceResult<RegisterResponseDto>().Fail("Email Reuse Found", "An Account is already registered on this email address");
            User user = new User();
            user.Email = request.Email;
            user.Name = request.Name;
            user.HashPassword = new PasswordHasher<User>().HashPassword(user, request.Password);
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            return new ServiceResult<RegisterResponseDto>().Success(new RegisterResponseDto { Name = user.Name, Role = user.Role, Email = user.Email });
        }
        public async Task<ServiceResult<LoginResponseDto>> Login(LoginRequestDto request)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user is null)
                return new ServiceResult<LoginResponseDto>().Fail("Account not found", "Wrong email address!");

            if (new PasswordHasher<User>()
                  .VerifyHashedPassword(user, user.HashPassword, request.Password)
                  == PasswordVerificationResult.Failed)
                return new ServiceResult<LoginResponseDto>().Fail("Invalid Password", "Wrong password!");

            return new ServiceResult<LoginResponseDto>()
                .Success(new LoginResponseDto
                {
                    Email = user.Email,
                    Role = user.Role,
                    RefreshToken = await CreateAndSaveRefreshToken(user),
                    Token = await CreateToken(user),
                    RefreshTokenExpirationDate = user.RefreshTokenExpirationDate
                });
        }
        public async Task<string> CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.Name),
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Role,user. Role),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetValue<string>("Appsettings:Token")!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
            var tokenDescriptor = new JwtSecurityToken
                (
                issuer: config.GetValue<string>("AppSettings:Issuer"),
                audience: config.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                signingCredentials: creds,
                expires: DateTime.UtcNow.AddMinutes(3)
                );
            var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
            return token;
        }
        public async Task<ServiceResult<string>> Logout(int id)
        {
            var user = await context.Users.FindAsync(id);
            user.RefreshToken = null;
            user.RefreshTokenExpirationDate = DateTime.MinValue;
            await context.SaveChangesAsync();
            return new ServiceResult<string>().Success("User Logged out");
        }
        public async Task<ServiceResult<RefreshTokenResponseDto>> RefreshTokenAsync(RefreshTokenRequestDto request)
        {
            var user = await context.Users.Where(u=>u.RefreshToken==request.RefreshToken).FirstOrDefaultAsync();
            if (user is null)
            {
                logger.InvalidRefreshTokenLog(DateTime.Now);
                return new ServiceResult<RefreshTokenResponseDto>().Fail("Invalid Token", "There's no user with this refresh token!");
            }
            if (user.RefreshTokenExpirationDate < DateTime.UtcNow)
                return new ServiceResult<RefreshTokenResponseDto>().Fail("Token Expired", "Login Session has ended login again!");

            var res = new RefreshTokenResponseDto
            {
                UserId = user.Id,
                RefreshToken = await CreateAndSaveRefreshToken(user),
                Token = await CreateToken(user)
            };
            return new ServiceResult<RefreshTokenResponseDto>().Success(res);
        }
        public async Task<string> CreateAndSaveRefreshToken(User user)
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            var rToken = Convert.ToBase64String(randomNumber);
            user.RefreshToken = rToken;
            user.RefreshTokenExpirationDate = DateTime.UtcNow.AddDays(3);
            await context.SaveChangesAsync();
            return rToken;
        }
    }
}