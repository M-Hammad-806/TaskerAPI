
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;
using TaskerApi.Data;
using TaskerApi.Services;

namespace TaskerApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            builder.Logging.ClearProviders();          
            builder.Logging.AddConsole();              
            builder.Logging.SetMinimumLevel(LogLevel.Information);
            //custom services
            builder.Services.AddScoped<IUserAuthService, UserAuthService>();
            builder.Services.AddScoped<ITaskService, TaskService>();
            //JwtBearer 
            builder.Services.AddAuthentication().AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
                options =>
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateActor = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,

                        ValidIssuer = builder.Configuration.GetValue<string>("AppSettings:Issuer"),
                        ValidAudience = builder.Configuration.GetValue<string>("AppSettings:Audience"),
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"]!))
                    };
                });
           
            //database context
            builder.Services.AddDbContext<AppDbContext>
                (options=>options
                .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
