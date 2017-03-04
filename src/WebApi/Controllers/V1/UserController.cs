using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebApi.Common;
using WebApi.Common.Auth;
using WebApi.Models.Requests.Users;
using WebApi.Models.Responses;
using WebApi.Models.Responses.Users;
using WebApi.Repositories.Interfaces;
using static WebApi.Common.Auth.ClaimsIdentityHelper;

namespace WebApi.Controllers.V1
{
    [Route("api/v1/user")]
    public class UserController : Controller
    {
        private readonly IUserRepository _users;
        private readonly ILogger _logger;

        public UserController(IUserRepository users, ILogger<UserController> logger)
        {
            _users = users;
            _logger = logger;
        }

        [HttpPost("auth")]
        public IActionResult Auth([FromBody]AuthRequest user)
        {
            var builder = new AuthResponse.Builder();
            if (user == null)
            {
                _logger.LogInformation("User passed is null");
                builder.SetMessage("Username and password are empty");
                builder.SetResult(Results.NotExists);
            }
            else
            {
                _logger.LogInformation("User name:{0}", user.Username);
                var existUser = _users.Find(user.Username, user.Password);

                switch (existUser.Result)
                {
                    case Results.NotExists:
                        builder.SetMessage("Username or password is invalid");
                        builder.SetResult(Results.NotExists);
                        break;
                    case Results.Succeed:
                        var expiresIn = DateTime.Now + TokenAuthOption.ExpiresSpan;
                        var accessToken = GenerateToken(existUser.UserId, existUser.UserName, expiresIn);
                        builder.SetExpire(TokenAuthOption.ExpiresSpan.TotalSeconds)
                            .SetToken(accessToken)
                            .SetUserId(existUser.UserId)
                            .SetType(TokenAuthOption.TokenType)
                            .SetUsername(existUser.UserName)
                            .SetResult(Results.Succeed);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            var response = builder.Build();
            var content = JsonConvert.SerializeObject(response);
            switch (response.Result)
            {
                case Results.Succeed:
                    return Ok(content);
                case Results.NotExists:
                    return NotFound(content);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateUserRequest user)
        {
            var response = _users.Create(user?.Username, user?.Password);
            return HandleResponse(response);
        }

        [HttpPut]
        [Authorize(Config.IdentityType)]
        public IActionResult Update([FromBody] UpdateUserRequest user)
        {
            var response = _users.Update(this.GetUserId(), user?.Password);
            return HandleResponse(response);
        }

        private IActionResult HandleResponse(Response response)
        {
            var content = JsonConvert.SerializeObject(response);
            IActionResult result;
            switch (response.Result)
            {
                case Results.Succeed:
                    result = Ok(content);
                    break;
                case Results.NotExists:
                    result = NotFound(content);
                    break;
                case Results.Exists:
                case Results.Failed:
                    result = BadRequest(content);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return result;
        }
    }
}