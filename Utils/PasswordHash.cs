using BCrypt.Net;

namespace EpsBackend.Utils
{
    public class PasswordHash
    {
        public static string HashedPassword(string ToHash)
        {
            return BCrypt.Net.BCrypt.HashPassword(ToHash);
        }
    }
}
