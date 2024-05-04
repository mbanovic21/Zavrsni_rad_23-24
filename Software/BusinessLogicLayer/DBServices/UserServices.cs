using DataAccessLayer.Repositories;
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
            using (var repo = new UserRepository(new DataAccessLayer.PMSmodel()))
            {
                var ID = repo.GetID(id);
                if(ID == id.ToString())
                {
                    return true;
                } else
                {
                    return false;
                }
            }
        }
    }
}
