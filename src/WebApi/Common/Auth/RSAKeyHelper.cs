using System.Security.Cryptography;

namespace WebApi.Common.Auth
{
    public class RsaKeyHelper
    {
        public static RSAParameters GenerateKey()
        {
            using (var key = RSA.Create())
            {
                return key.ExportParameters(true);
            }
        }
    }
}