using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using GymManagement.ConsoleUtilities;


namespace GymManagement.ConsoleUtilities
{
    public static class PasswordUtils
    {
        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var passwordBytes = Encoding.UTF8.GetBytes(password);
                var hashBytes = sha256.ComputeHash(passwordBytes);
                return Convert.ToHexString(hashBytes);
            }
        }
    }
}
