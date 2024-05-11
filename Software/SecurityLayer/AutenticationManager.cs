using BusinessLogicLayer.DBServices;
using EntityLayer;
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
        private UserServices UserServices = new UserServices();

        public bool AuthenticateUser(string username, string password)
        {
            var credentials = UserServices.GetCredentialsByUsername(username);

            if(credentials != null)
            {
                var hashedPasswordFromDatabase = credentials[0];
                var saltFromDatabase = credentials[1];

                Console.WriteLine($"Iz baze: {hashedPasswordFromDatabase}, {saltFromDatabase}");

                if (!string.IsNullOrEmpty(hashedPasswordFromDatabase) && !string.IsNullOrEmpty(saltFromDatabase))
                {
                    var isCredentialsValid = EncryptionManager.VerifyPassword(password, hashedPasswordFromDatabase, saltFromDatabase);
                    if(isCredentialsValid)
                    {
                        LoggedInUser.User = UserServices.GetUserByUsername(username);
                    }
                    return isCredentialsValid;
                }

                return false;
            }

            return false;
        }

        public string GeneratePassword()
        {
            return EncryptionManager.GeneratePassword();
        }

        public (string hashedPassword, string salt) HashPasswordAndGetSalt(string password)
        {
            (string hashedPassword, string salt) = EncryptionManager.HashPassword(password);
            return (hashedPassword, salt);
        }
    }
}
