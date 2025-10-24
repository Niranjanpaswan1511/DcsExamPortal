using System.Security.Cryptography;
using System.Text;

namespace DcsExamPortal.Api.Utils
{
    public static class Utils
    {
        public static string GenerateHmacSHA256(string message, string secret)
        {
            var keyBytes = Encoding.UTF8.GetBytes(secret);
            var messageBytes = Encoding.UTF8.GetBytes(message);

            using (var hmac = new HMACSHA256(keyBytes))
            {
                var hashBytes = hmac.ComputeHash(messageBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower(); 
            }
        }
    }
}
