using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using WebApi.Models.Responses;

namespace WebApi.Common
{
    public static class Helper
    {
        #region Auth
        public static RSAParameters GenerateKey()
        {
            using (var key = RSA.Create())
            {
                return key.ExportParameters(true);
            }
        }

        public static string GenerateToken(string userId, string userName, DateTime expires)
        {
            var handler = new JwtSecurityTokenHandler();

            var claims = new List<Claim> {new Claim(ClaimTypes.PrimarySid, userId)};
            var identity = new ClaimsIdentity(claims, userName);
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = Config.Issuer,
                Audience = Config.Audience,
                SigningCredentials = Config.SigningCredentials,
                Subject = identity,
                Expires = expires
            });
            return handler.WriteToken(securityToken);
        }
        #endregion

        #region Controller
        public static string GetUserId(this Controller controller)
        {
            var claimsIdentity = controller.User.Identity as ClaimsIdentity;
            if (claimsIdentity?.Claims != null)
            {
                var claim = claimsIdentity.FindFirst(ClaimTypes.PrimarySid);
                if (claim?.Value != null)
                {
                    return claim.Value;
                }
            }

            throw new UnauthorizedAccessException("invalid token");
        }

        public static IActionResult HandleResponse(this Controller controller, Response response)
        {
            var content = JsonConvert.SerializeObject(response);
            IActionResult result;
            switch (response.Result)
            {
                case Results.Succeed:
                    result = controller.Ok(content);
                    break;
                case Results.NotExists:
                    result = controller.NotFound(content);
                    break;
                case Results.Exists:
                case Results.Failed:
                    result = controller.BadRequest(content);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return result;
        }

        public static Response GenerateBadRequestResponse(this Controller controller, string errorMsg)
        {
            var builder = new Response.Builder();
            return builder.SetResult(Results.Failed).SetMessage(errorMsg).Build();
        }
        #endregion
    }
}