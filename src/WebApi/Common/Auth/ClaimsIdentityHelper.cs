
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebApi.Models.Mongodb;

namespace WebApi.Common.Auth
{
    public static class ClaimsIdentityHelper
    {
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

        public static string GenerateToken(User user, DateTime expires)
        {
            var handler = new JwtSecurityTokenHandler();

            var claims = new List<Claim> {new Claim(ClaimTypes.PrimarySid, user.UserId)};
            var identity = new ClaimsIdentity(claims, user.Username);
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
    }
}