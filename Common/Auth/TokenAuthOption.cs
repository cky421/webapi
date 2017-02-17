using System;
using Microsoft.IdentityModel.Tokens;
using webapi.Common;

namespace WebApi.Common.Auth
{
    public class TokenAuthOption 
    { 
        public static string Audience { get; } = Config.IdentityAudience; 
        public static string Issuer { get; } = Config.IdentityIssuer; 
        public static RsaSecurityKey Key { get; } = new RsaSecurityKey(RSAKeyHelper.GenerateKey()); 
        public static SigningCredentials SigningCredentials { get; } = new SigningCredentials(Key, SecurityAlgorithms.RsaSha256Signature); 
        public static TimeSpan ExpiresSpan { get; } = TimeSpan.FromMinutes(1); 
        public static string TokenType { get; } = Config.IdentityType;  
    } 
}
