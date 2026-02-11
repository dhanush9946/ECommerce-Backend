

using System.Security.Cryptography;
using System.Text;

namespace ECommerce.Infrastructure.Helper
{
    public static class RazorpaySignatureHelper
    {
        public static string GenerateSignature(string payload, string secret)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}
