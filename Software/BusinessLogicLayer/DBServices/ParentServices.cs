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
        //Add new parents
        public bool RegistrateParents(List<Parent> parentsForRegistration)
        {
            using (var repo = new ParentRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.RegistrateParents(parentsForRegistration);
            }
        }

        //Add new parent
        public bool RegistrateParent(Parent parentForRegistration)
        {
            using (var repo = new ParentRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.RegistrateParent(parentForRegistration);
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

        //Get mothers
        public List<Parent> GetMothers()
        {
            using (var repo = new ParentRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.GetMothers().ToList();
            }
        }

        //Get fathers
        public List<Parent> GetFathers()
        {
            using (var repo = new ParentRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.GetFathers().ToList();
            }
        }

        //Add child to father
        public bool isChildSetToFather(int fathersID, Child child)
        {
            using (var repo = new ParentRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.isChildSetToFather(fathersID, child);
            }
        }

        //Add child to mother
        public bool isChildSetToMother(int mothersID, Child child)
        {
            using (var repo = new ParentRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.isChildSetToMother(mothersID, child);
            }
        }
    }
}
