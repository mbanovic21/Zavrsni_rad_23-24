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

        // remove parents by child
        public bool RemoveParentsByChild(Child child)
        {
            using (var repo = new ParentRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.RemoveParentsByChild(child);
            }
        }

        //get parents by child
        public List<Parent> GetParentsByChild(Child child)
        {
            using (var repo = new ParentRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.GetParentsByChild(child);
            }
        }

        //Get mother by child
        public Parent GetMotherByChild(Child child)
        {
            using (var repo = new ParentRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.GetMotherByChild(child);
            }
        }

        //Get father by child
        public Parent GetFatherByChild(Child child)
        {
            using (var repo = new ParentRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.GetFatherByChild(child);
            }
        }
    }
}
