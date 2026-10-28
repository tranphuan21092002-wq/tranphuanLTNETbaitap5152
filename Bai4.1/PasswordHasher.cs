using System;
using System.Security.Cryptography;

namespace Bai4._1
{
    public static class PasswordHasher
    {
        public static bool VerifyPassword(string password, string storedHash)
        {
            try
            {
                string[] parts = storedHash.Split(':');

                if (parts.Length != 2)
                    return false;

                byte[] salt = HexToBytes(parts[0]);
                byte[] expectedHash = HexToBytes(parts[1]);

                using (var pbkdf2 = new Rfc2898DeriveBytes(
                    password, salt, 100000))
                {
                    byte[] actualHash = pbkdf2.GetBytes(expectedHash.Length);

                    if (actualHash.Length != expectedHash.Length)
                        return false;

                    int difference = 0;

                    for (int i = 0; i < actualHash.Length; i++)
                    {
                        difference |= actualHash[i] ^ expectedHash[i];
                    }

                    return difference == 0;
                }
            }
            catch
            {
                return false;
            }
        }

        private static byte[] HexToBytes(string hex)
        {
            byte[] bytes = new byte[hex.Length / 2];

            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(
                    hex.Substring(i * 2, 2), 16);
            }

            return bytes;
        }
    }
}