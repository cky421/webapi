using System;
using Microsoft.IdentityModel.Tokens;

namespace TheOne.Common.Auth
{
    public class TokenAuthOption 
    { 
        public static string Audience { get; } = "home_user"; 
        public static string Issuer { get; } = "youngytj@sina.com"; 
        public static RsaSecurityKey Key { get; } = new RsaSecurityKey(RSAKeyHelper.GenerateKey()); 
        public static SigningCredentials SigningCredentials { get; } = new SigningCredentials(Key, SecurityAlgorithms.RsaSha256Signature); 
 
        public static TimeSpan ExpiresSpan { get; } = TimeSpan.FromMinutes(1); 
        public static string TokenType { get; } = "Bearer";  
    } 
}
