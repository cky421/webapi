using System;
using Microsoft.IdentityModel.Tokens;
using static WebApi.Common.Helper;

namespace WebApi.Common
{
    public static class Config
    {
        public const string ApplicationName = "WebApi";
        public const string IdentityAudience = "home_user";
        public const string IdentityIssuer = "youngytj@sina.com";
        public const string IdentityType = "Bearer";
        public const string GenericIdentityType = "TokenAuth";
        public const string MongoDbConnection = "mongodb://localhost:27017";
        public const string AdminName = "admin";
        public const string AdminPwd = "admin";
        public static readonly string[] Origins = {"http://localhost:8080"};

        //Auth
        public static string Audience { get; } = IdentityAudience;
        public static string Issuer { get; } = IdentityIssuer;
        public static RsaSecurityKey Key { get; } = new RsaSecurityKey(GenerateKey());
        public static SigningCredentials SigningCredentials { get; } = new SigningCredentials(Key, SecurityAlgorithms.RsaSha256Signature);
        public static TimeSpan ExpiresSpan { get; } = TimeSpan.FromHours(6);
        public static string TokenType { get; } = IdentityType;
    }
}