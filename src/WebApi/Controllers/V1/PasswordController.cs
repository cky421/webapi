using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.Common;
using WebApi.Models.Requests.Passwords;
using WebApi.Repositories.Interfaces;

namespace WebApi.Controllers.V1
{
    [Route("api/v1/password")]
    [Authorize(Config.IdentityType)]
    public class PasswordController : Controller
    {
        private readonly IPasswordRepository _passwords;
        private readonly ILogger _logger;

        public PasswordController(IPasswordRepository passwords, ILogger<GroupController> logger)
        {
            _passwords = passwords;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreatePasswordRequest password)
        {
            var response = string.IsNullOrEmpty(password?.GroupId) ||
                       string.IsNullOrWhiteSpace(password.Title)
                ? this.GenerateBadRequestResponse("title/groupid can not be null")
                : _passwords.InsertPassword(password.GroupId, this.GetUserId(), password.Title,
                    password.Username, password.Pwd, password.PayPwd, password.Note);
            return this.HandleResponse(response);
        }

        [HttpPut]
        public IActionResult Update([FromBody] UpdatePasswordRequest password)
        {
            var response = string.IsNullOrWhiteSpace(password?.GroupId) ||
                       string.IsNullOrWhiteSpace(password.PasswordId) ||
                       string.IsNullOrWhiteSpace(password.Title)
                ? this.GenerateBadRequestResponse("groupid/passwordid/title can not be null")
                : _passwords.UpdatePassword(password.GroupId, this.GetUserId(), password.PasswordId,
                    password.Title, password.Username, password.Pwd, password.PayPwd, password.Note);

            return this.HandleResponse(response);
        }

        [HttpGet("{passwordid}")]
        public IActionResult Get([FromRoute] string passwordId)
        {
            var response = string.IsNullOrWhiteSpace(passwordId)
                ? this.GenerateBadRequestResponse("passwordid can not be null")
                : _passwords.GetPassword(passwordId, this.GetUserId());

            return this.HandleResponse(response);
        }

        [HttpGet("{groupid}/all")]
        public IActionResult Fetch([FromRoute] string groupId)
        {
            var response = string.IsNullOrWhiteSpace(groupId)
                ? this.GenerateBadRequestResponse("groupid can not be null")
                : _passwords.GetAllPasswordByGroupId(groupId, this.GetUserId());
            return this.HandleResponse(response);
        }
    }
}