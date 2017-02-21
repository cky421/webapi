using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using WebApi.Common;
using WebApi.Common.Auth;
using WebApi.Models;
using WebApi.Models.Responses;
using WebApi.Repositories;

namespace WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    public class AuthController : Controller
    {
        private readonly IUserRepository _users;
        private readonly ILogger _logger;
        public AuthController(IUserRepository users, ILogger<AuthController> logger)
        {
            _users = users;
            _logger = logger;
        }

        [HttpPost]
        public string Post([FromBody]User user)
        {
            if (user == null)
            {
                _logger.LogInformation("User passed is null");
                return JsonConvert.SerializeObject(new Response<string>
                {
                    Message = "Username and password are empty"
                });
            }

            _logger.LogInformation("User name:{0}", user.Username);
            var existUser = _users.Find(user.Username, user.Password);

            if (existUser == null)
                return JsonConvert.SerializeObject(new Response<string>
                {
                    Message = "Username or password is invalid"
                });
            var expiresIn = DateTime.Now + TokenAuthOption.ExpiresSpan;
            var accessToken = GenerateToken(existUser, expiresIn);

            return JsonConvert.SerializeObject(new Response<AuthResponse>
            {
                Data = new AuthResponse
                {
                    Expire = TokenAuthOption.ExpiresSpan.TotalSeconds,
                    Type = TokenAuthOption.TokenType,
                    Token = accessToken,
                    Username = user.Username
                }
            });
        }

        private static string GenerateToken(User user, DateTime expires)
        {
            var handler = new JwtSecurityTokenHandler();

            var identity = new ClaimsIdentity(
                new GenericIdentity(user.Username, Config.GenericIdentityType),
                new[] {
                    new Claim("_id", user._id.ToString())
                }
            );

            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = TokenAuthOption.Issuer,
                Audience = TokenAuthOption.Audience,
                SigningCredentials = TokenAuthOption.SigningCredentials,
                Subject = identity,
                Expires = expires
            });
            return handler.WriteToken(securityToken);
        }

        [HttpGet]
        [Authorize(Config.IdentityType)]
        public string Get()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;

            if (claimsIdentity != null)
                return JsonConvert.SerializeObject(new Response<AuthResponse>
                {
                    Data = new AuthResponse
                    {
                        Username = claimsIdentity.Name
                    }
                });
            return JsonConvert.SerializeObject(new Response<string>
            {
                Message = "User is unidentity."
            });

        }
    }
}
