
using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

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
    }
}