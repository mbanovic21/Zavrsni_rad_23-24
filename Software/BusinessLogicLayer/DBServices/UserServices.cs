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

        public bool AreCredentialsValid(string username, string password)
        {
            using (var repo = new UserRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                var user = repo.GetUserByUsername(username);

                if (user != null)
                {
                    if (user.Username == username && user.Password == password)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
    }
}
