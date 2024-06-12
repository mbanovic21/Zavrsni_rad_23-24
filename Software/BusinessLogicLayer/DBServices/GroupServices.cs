using DataAccessLayer.Repositories;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DBServices
{
    public class GroupServices
    {
        public List<Group> GetAllGroups()
        {
            using (var repo = new GroupRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.GetAllGroups().ToList();
            }

        }

        public List<Group> GetGroupByName(string name)
        {
            using (var repo = new GroupRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.GetGroupByName(name).ToList();
            }

        }

        public void AddGroup(Group group)
        {
            using (var repo = new GroupRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                repo.AddGroup(group);
            }
        }
        public int? GetGroupIdByName(string name)
        {
            using (var repo = new GroupRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.GetGroupIdByName(name);
            }
        }
    }
}
