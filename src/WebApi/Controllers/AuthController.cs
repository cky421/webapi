using System;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common.Auth;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.IdentityModel.Tokens;
using WebApi.Models;
using WebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using WebApi.Common;

namespace WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    public class AuthController : Controller
    {
        private readonly IUserRepository users;
        private readonly ILogger logger;
        public AuthController(IUserRepository users, ILogger<AuthController> logger)
        {
            this.users = users;
            this.logger = logger;
        }

        [HttpPost]
        public string POST([FromBody]User user)
        {
            var existUser = users.Find(user.Username, user.Password);

            if (existUser != null)
            {
                var requestAt = DateTime.Now;
                var expiresIn = requestAt + TokenAuthOption.ExpiresSpan;
                var token = GenerateToken(existUser, expiresIn);

                return JsonConvert.SerializeObject(new Response
                {
                    state = ResponseState.Success,
                    data = new
                    {
                        requertAt = requestAt,
                        expiresIn = TokenAuthOption.ExpiresSpan.TotalSeconds,
                        tokeyType = TokenAuthOption.TokenType,
                        accessToken = token
                    }
                });
            }
            else
            {
                return JsonConvert.SerializeObject(new Response
                {
                    state = ResponseState.Failed,
                    msg = "Username or password is invalid"
                });
            }
        }

        private string GenerateToken(User user, DateTime expires)
        {
            var handler = new JwtSecurityTokenHandler();

            ClaimsIdentity identity = new ClaimsIdentity(
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
        public string GET()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;

            return JsonConvert.SerializeObject(new Response
            {
                state = ResponseState.Success,
                data = new
                {
                    UserName = claimsIdentity.Name
                }
            });
        }
    }
}
