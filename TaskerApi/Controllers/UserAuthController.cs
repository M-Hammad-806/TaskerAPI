using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using System.Security.Claims;
using TaskerApi.DTOS;
using TaskerApi.Services;

namespace TaskerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
   // [ApiExplorerSettings(IgnoreApi = true)]
    public class UserAuthController : ControllerBase
    {

        private readonly IUserAuthService service;
        private readonly ILogger<UserAuthController> logger;
        public UserAuthController(IUserAuthService service, ILogger<UserAuthController> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterRequestDto request)
        {
            var res = await service.Register(request);

            return res.IsSuccess ? Ok(res.Result) : BadRequest(new { res.Title, res.Message });
        }
        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginRequestDto request)
        {
            var res = await service.Login(request);
            return res.IsSuccess ? Ok(res.Result) : BadRequest(new { res.Title, res.Message });
        }
        [HttpPut("log-out")]
        [Authorize]
        public async Task<ActionResult> Logout()
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var res = await service.Logout(userId);

            return res.IsSuccess ? Ok(res.Result) : BadRequest(new { res.Title, res.Message });
        }
        [HttpPut("refresh-token")]
        public async Task<ActionResult> RefreshToken(RefreshTokenRequestDto request)
        {
            var res = await service.RefreshTokenAsync(request);
            logger.RefreshTokenRequestLog(DateTime.Now);
            return res.IsSuccess ? Ok(res.Result) : BadRequest(new { res.Title, res.Message });
        }
    }
}
