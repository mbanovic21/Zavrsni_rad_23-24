using DataAccessLayer.Repositories;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DBServices
{
    public class UserServices
    {
        //PIN Validation
        public bool IsIDVaild(string id)
        {
            using (var repo = new UserRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                var ID = repo.GetID(id);
                if (ID == id.ToString())
                {
                    return true;
                } else
                {
                    return false;
                }
            }
        }

        //Getting Credentials
        public string[] GetCredentialsByUsername(string username)
        {
            using (var repo = new UserRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                var user = repo.GetUserByUsername(username);

                if (user != null)
                {
                    var hashedPasswordFromDatabase = user.Password;
                    var saltFromDatabase = user.Salt;

                    return new string[] 
                    {
                        hashedPasswordFromDatabase,
                        saltFromDatabase
                    };
                } else
                {
                    return null;
                }
            }
        }

        //Add new user
        public bool RegistrateUser(User userForRegistration)
        {
            using (var repo = new UserRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.RegistrateUser(userForRegistration);
            }
        }

        //Remove user
        public bool RemoveUser(string username, string pin)
        {
            using (var repo = new UserRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.RemoveUser(username, pin);
            }
        }

        public User GetUserByUsername(string username)
        {
            using (var repo = new UserRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.GetUserByUsername(username);
            }
        }
    }
}
