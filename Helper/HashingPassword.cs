using System.Security.Cryptography;
using System.Text;

namespace HospitalSystemTeamTask.Helper
{
    public class HashingPassword
    {
        public static string Hshing(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Convert the input password to a byte array
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                // Compute the hash
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);

                // Convert the hash to a string (hexadecimal representation)
                StringBuilder hashString = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    hashString.Append(b.ToString("x2")); // Converts each byte to a hex string
                }
                return hashString.ToString();
            }
        }
    }
}
