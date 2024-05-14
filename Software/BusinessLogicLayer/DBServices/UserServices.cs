using DataAccessLayer.Repositories;
using EntityLayer;
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

        //Getting user by username
        public User GetUserByUsername(string username)
        {
            using (var repo = new UserRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.GetUserByUsername(username);
            }
        }

        //Getting user by username pattern
        public List<User> GetUserByUsernamePattern(string pattern)
        {
            using (var repo = new UserRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.GetUserByUsernamePattern(pattern).ToList();
            }
        }

        //Getting user by PIN pattern
        public List<User> GetUserByPINPattern(string pattern)
        {
            using (var repo = new UserRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.GetUserByPINPattern(pattern).ToList();
            }
        }

        //Getting user by Firstname pattern
        public List<User> GetUserByFirstNamePattern(string pattern)
        {
            using (var repo = new UserRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.GetUserByFirstNamePattern(pattern).ToList();
            }
        }

        //Getting user by Lastname pattern
        public List<User> GetUserByLastNamePattern(string pattern)
        {
            using (var repo = new UserRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.GetUserByLastNamePattern(pattern).ToList();
            }
        }

        //Getting user by firstname and Lastname pattern
        public List<User> GetUserByFirstNameAndLastNamePattern(string pattern)
        {
            using (var repo = new UserRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.GetUserByFirstNameAndLastNamePattern(pattern).ToList();
            }
        }

        //Getting user by email pattern
        public List<User> GetUserByEmailPattern(string pattern)
        {
            using (var repo = new UserRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.GetUserByEmailPattern(pattern).ToList();
            }
        }

        //Getting all users
        public List<User> GetAllUsers()
        {
            using (var repo = new UserRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.GetAllUsers().ToList();
            }
        }
    }
}
