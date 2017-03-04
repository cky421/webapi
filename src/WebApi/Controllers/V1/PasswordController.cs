using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebApi.Common;
using WebApi.Common.Auth;
using WebApi.Models.Requests.Passwords;
using WebApi.Models.Responses;
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
            var passwordResponse = _passwords.InsertPassword(this.GetUserId(), password?.Title, password?.Username,
                password?.Pwd, password?.PayPwd, password?.GroupId, password?.Note);
            return HandleResponse(passwordResponse);
        }

        [HttpPut]
        public IActionResult Update([FromBody] UpdatePasswordRequest password)
        {
            var passwordResponse = _passwords.UpdatePassword(password?.PasswordId, this.GetUserId(), password?.Title, password?.Username,
                password?.Pwd, password?.PayPwd, password?.GroupId, password?.Note);
            return HandleResponse(passwordResponse);
        }

        [HttpGet("{passwordid}")]
        public IActionResult Get([FromRoute] string passwordId)
        {
            var passwordResponse = _passwords.GetPassword(passwordId, this.GetUserId());
            return HandleResponse(passwordResponse);
        }

        [HttpGet("{groupid}/all")]
        public IActionResult Fetch([FromRoute] string groupId)
        {
            var passwordResponse = _passwords.GetAllPasswordByGroupId(groupId, this.GetUserId());
            return HandleResponse(passwordResponse);
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