using DataAccessLayer.Repositories;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DBServices
{
    public class UserServices
    {
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

    }
}
