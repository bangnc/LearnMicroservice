using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Common.Security
{
    public static class OtpGenerator
    {
        public static string Generate(int length = 6)
        {
            if (length <= 0)
                throw new ArgumentException("OTP length must be greater than 0.");

            var otp = "";

            for (int i = 0; i < length; i++)
            {
                otp += RandomNumberGenerator.GetInt32(0, 10);
            }

            return otp;
        }
        public static string Hash(string otp)
        {
            using var sha = SHA256.Create();

            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(otp));

            return Convert.ToHexString(bytes);
        }
    }
}
