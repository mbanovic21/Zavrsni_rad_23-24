using BusinessLogicLayer.DBServices;
using EntityLayer;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLayer
{
    public class AuthorizationManager
    {
        public bool GetRole()
        {
            var loggedInUser = LoggedInUser.User;
            var isAdmin = loggedInUser.Id_role == 1 ? true : false;
            return isAdmin;
        }
    }
}
