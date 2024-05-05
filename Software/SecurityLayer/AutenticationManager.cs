using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLayer
{
    public class AutenticationManager
    {
        private EncryptionManager EncryptionManager = new EncryptionManager();

        public bool AuthenticateUser(string username, string password)
        {
            // Dohvatite lozinku i sol iz baze podataka za korisnika s korisničkim imenom 'username'
            string hashedPasswordFromDatabase = ""; // Dohvatite pohranjenu enkriptiranu lozinku iz baze
            string saltFromDatabase = ""; // Dohvatite sol iz baze

            // Provjerite jesu li korisnički podaci ispravni
            if (!string.IsNullOrEmpty(hashedPasswordFromDatabase) && !string.IsNullOrEmpty(saltFromDatabase))
            {
                // Provjerite lozinku koristeći PasswordManager
                return EncryptionManager.VerifyPassword(password, hashedPasswordFromDatabase, saltFromDatabase);
            }

            return false;
        }
    }
}
