namespace WebApi.Common
{
    public static class Config
    {
        public const string ApplicationName = "WebApi";
        public const string IdentityAudience = "home_user";
        public const string IdentityIssuer = "youngytj@sina.com";
        public const string IdentityType = "Bearer";
        public const string MongoDbConnection = "mongodb://localhost:27017";
        public const string AdminName = "admin";
        public const string AdminPwd = "admin";
        public static readonly string[] Origins = {"http://localhost:8080"};
    }
}