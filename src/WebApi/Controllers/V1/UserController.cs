using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.Common;
using WebApi.Models.Requests.Users;
using WebApi.Repositories.Interfaces;

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
            var response = string.IsNullOrWhiteSpace(user?.Username) || string.IsNullOrWhiteSpace(user.Password)
                ? this.GenerateBadRequestResponse("username/password can not be null")
                : _users.Auth(user.Username, user.Password);

            return this.HandleResponse(response);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateUserRequest user)
        {
            var response = string.IsNullOrWhiteSpace(user?.Username) || string.IsNullOrWhiteSpace(user.Password)
                ? this.GenerateBadRequestResponse("username/password can not be null")
                : _users.Create(user.Username, user.Password);

            return this.HandleResponse(response);
        }

        [HttpPut]
        [Authorize(Config.IdentityType)]
        public IActionResult Update([FromBody] UpdateUserRequest user)
        {
            var response = string.IsNullOrWhiteSpace(user?.Password)
                ? this.GenerateBadRequestResponse("password can not be null")
                : _users.Update(this.GetUserId(), user.Password);

            return this.HandleResponse(response);
        }
    }
}