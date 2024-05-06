using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLayer
{
    internal class EncryptionManager
    {
        private const int SaltSize = 32; // Duljina soli u bajtovima
        private const int Iterations = 10000; // Broj iteracija za sporu hash funkciju

        public (string hashedPassword, string salt) HashPassword(string password)
        {
            // Generira nasumičnu sol
            string salt = GenerateSalt();

            // Konvertira lozinku i sol u niz bajtova
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] saltBytes = Encoding.UTF8.GetBytes(salt);

            // Kombinira lozinku i sol
            byte[] combinedBytes = new byte[passwordBytes.Length + saltBytes.Length];
            Array.Copy(passwordBytes, 0, combinedBytes, 0, passwordBytes.Length);
            Array.Copy(saltBytes, 0, combinedBytes, passwordBytes.Length, saltBytes.Length);

            // Stvori kriptografski providor s brojem iteracija
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, Iterations))
            {
                // Izračuna sažetak lozinke i soli, Password-Based Key Derivation Function (pbkdf2)
                byte[] hashBytes = pbkdf2.GetBytes(SaltSize); // 32 bajta za SHA-256

                // Pretvori sažetak u heksadecimalni format
                string hashedPassword = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

                return (hashedPassword, salt);
            }
        }

        public bool VerifyPassword(string password, string hashedPassword, string salt)
        {
            // Konvertira lozinku i sol u niz bajtova
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] saltBytes = Encoding.UTF8.GetBytes(salt);

            // Kombinir lozinku i sol
            byte[] combinedBytes = new byte[passwordBytes.Length + saltBytes.Length];
            Array.Copy(passwordBytes, 0, combinedBytes, 0, passwordBytes.Length);
            Array.Copy(saltBytes, 0, combinedBytes, passwordBytes.Length, saltBytes.Length);

            // Stvara kriptografski providor s brojem iteracija
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, Iterations))
            {
                // Izračuna sažetak lozinke i soli
                byte[] hashBytes = pbkdf2.GetBytes(32); // 32 bajta za SHA-256

                // Pretvori sažetak u heksadecimalni format
                string hashedPasswordToCompare = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

                // Usporedi sažetke
                Console.WriteLine($"Password to compare: {hashedPasswordToCompare} vs. {hashedPassword}");
                return hashedPasswordToCompare.Equals(hashedPassword);
            }
        }

        private string GenerateSalt()
        {
            // Stvori generator slučajnih brojeva
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] saltBytes = new byte[SaltSize];

                // Popuni niz bajtova s nasumičnim vrijednostima
                rng.GetBytes(saltBytes);

                // Pretvori niz bajtova u heksadecimalni format
                string salt = BitConverter.ToString(saltBytes).Replace("-", "");

                return salt;
            }
        }
    }
}
