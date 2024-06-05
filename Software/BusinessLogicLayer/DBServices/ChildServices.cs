using DataAccessLayer.Repositories;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DBServices
{
    public class ChildServices
    {
        //Getting all Children
        public List<Child> GetAllChildren()
        {
            using (var repo = new ChildRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.GetAllChildren().ToList();
            }
        }

        //Getting Child by PIN pattern
        public List<Child> GetChildByPINPattern(string pattern)
        {
            using (var repo = new ChildRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.GetChildByPINPattern(pattern).ToList();
            }
        }

        //Getting Child by Firstname pattern
        public List<Child> GetChildByFirstNamePattern(string pattern)
        {
            using (var repo = new ChildRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.GetChildByFirstNamePattern(pattern).ToList();
            }
        }

        //Getting Child by Lastname pattern
        public List<Child> GetChildByLastNamePattern(string pattern)
        {
            using (var repo = new ChildRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.GetChildByLastNamePattern(pattern).ToList();
            }
        }

        //Getting Child by firstname and Lastname pattern
        public List<Child> GetChildByFirstNameAndLastNamePattern(string pattern)
        {
            using (var repo = new ChildRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.GetChildByFirstNameAndLastNamePattern(pattern).ToList();
            }
        }

        //Getting Child by nationality
        public List<Child> GetChildByNationalityPattern(string pattern)
        {
            using (var repo = new ChildRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.GetChildByNationalityPattern(pattern).ToList();
            }
        }

        //Getting Child by development status
        public List<Child> GetChildByDevelopmentStatusPattern(string pattern)
        {
            using (var repo = new ChildRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.GetChildByDevelopmentStatusPattern(pattern).ToList();
            }
        }

        //Getting Child by medical information
        public List<Child> GetChildByMedicalInformationPattern(string pattern)
        {
            using (var repo = new ChildRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.GetChildByMedicalInformationPattern(pattern).ToList();
            }
        }

        //Getting Child by birth place
        public List<Child> GetChildByBirthPlacePattern(string pattern)
        {
            using (var repo = new ChildRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.GetChildByBirthPlacePattern(pattern).ToList();
            }
        }

        //Remove child
        public bool RemoveChild(int id)
        {
            using (var repo = new ChildRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.RemoveChild(id);
            }
        }

        //Update child
        public bool isUpdated(Child child)
        {
            using (var repo = new ChildRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.updateChild(child);
            }
        }
        
        //Add child
        public bool RegistrateChild(Child child, List<Parent> parents)
        {
            using (var repo = new ChildRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.addChild(child, parents);
            }
        }

        //Get Child by PIN
        public Child GetChildByPIN(string pin)
        {
            using (var repo = new ChildRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.getChildByPIN(pin);
            }
        }

        //Get childrens by parent
        public List<Child> GetChildrenByParent(Parent parent)
        {
            using (var repo = new ChildRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.GetChildrenByParent(parent);
            }
        }
    }
}
