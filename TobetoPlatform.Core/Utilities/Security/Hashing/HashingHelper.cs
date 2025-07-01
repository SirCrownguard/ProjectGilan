// --- Dosya: HashingHelper.cs ---
using System.Security.Cryptography;
using System.Text;

namespace TobetoPlatform.Core.Utilities.Security.Hashing
{
    public static class HashingHelper
    {
        // Verilen şifreden bir hash ve salt oluşturur
        public static void HazırlaPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        // Gelen şifreyi, veritabanındaki hash ve salt ile karşılaştırır
        public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}