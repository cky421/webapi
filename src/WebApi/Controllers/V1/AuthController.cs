using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebApi.Common;
using WebApi.Common.Auth;
using WebApi.Models.Requests;
using WebApi.Models.Responses;
using WebApi.Repositories.Interfaces;
using static WebApi.Common.Auth.ClaimsIdentityHelper;

namespace WebApi.Controllers.V1
{
    [Route("api/v1/user")]
    public class AuthController : Controller
    {
        private readonly IUserRepository _users;
        private readonly ILogger _logger;
        public AuthController(IUserRepository users, ILogger<AuthController> logger)
        {
            _users = users;
            _logger = logger;
        }

        [HttpPost("auth")]
        public string Post([FromBody]AuthRequest user)
        {
            if (user == null)
            {
                _logger.LogInformation("User passed is null");
                return JsonConvert.SerializeObject(new Response
                {
                    Message = "Username and password are empty"
                });
            }

            _logger.LogInformation("User name:{0}", user.Username);
            var existUser = _users.Find(user.Username, user.Password);

            if (existUser == null)
                return JsonConvert.SerializeObject(new Response
                {
                    Message = "Username or password is invalid"
                });
            var expiresIn = DateTime.Now + TokenAuthOption.ExpiresSpan;
            var accessToken = GenerateToken(existUser.UserId, existUser.UserName, expiresIn);

            return JsonConvert.SerializeObject(new AuthResponse
            {
                Expire = TokenAuthOption.ExpiresSpan.TotalSeconds,
                Type = TokenAuthOption.TokenType,
                Token = accessToken,
                Username = user.Username
            });
        }

        [HttpGet]
        [Authorize(Config.IdentityType)]
        public string Get()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;

            if (claimsIdentity != null)
                return JsonConvert.SerializeObject(new AuthResponse
                {
                    Username = claimsIdentity.Name
                });
            return JsonConvert.SerializeObject(new Response
            {
                Message = "User is unidentity."
            });

        }
    }
}
