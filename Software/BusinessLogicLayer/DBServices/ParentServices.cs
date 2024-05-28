using DataAccessLayer.Repositories;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DBServices
{
    public class ParentServices
    {
        //Add new parent
        public bool RegistrateParents(List<Parent> parentsForRegistration)
        {
            using (var repo = new ParentRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.RegistrateParents(parentsForRegistration);
            }
        }

        //Remove parent
        public bool RemoveParents(List<Parent> parents)
        {
            using (var repo = new ParentRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.RemoveParents(parents);
            }
        }
    }
}
