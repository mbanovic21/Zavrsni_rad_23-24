using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class ChildRepository : IDisposable
    {
        private PreschoolManagmentModel Context { get; set; }
        private DbSet<Child> Children { get; set; }
        public ChildRepository(PreschoolManagmentModel context)
        {
            Context = context;
            Children = Context.Set<Child>();
        }

        //all children
        public IQueryable<Child> GetAllChildren()
        {
            var query = from c in Children
                        select c;

            return query;
        }

        //pattern-PIN
        public IQueryable<Child> GetChildByPINPattern(string pattern)
        {
            var query = from c in Children
                        where c.PIN.Contains(pattern)
                        select c;

            return query;
        }

        //pattern-Firstname
        public IQueryable<Child> GetChildByFirstNamePattern(string pattern)
        {
            var query = from c in Children
                        where c.FirstName.Contains(pattern)
                        select c;

            return query;
        }

        //pattent-Lastname
        public IQueryable<Child> GetChildByLastNamePattern(string pattern)
        {
            var query = from c in Children
                        where c.LastName.Contains(pattern)
                        select c;

            return query;
        }

        //pattern-flname
        public IQueryable<Child> GetChildByFirstNameAndLastNamePattern(string pattern)
        {
            var query = from c in Children
                        where c.FirstName.Contains(pattern) && c.LastName.Contains(pattern)
                        select c;

            return query;
        }

        //pattent-nationality
        public IQueryable<Child> GetChildByNationalityPattern(string pattern)
        {
            var query = from c in Children
                        where c.Nationality.Contains(pattern)
                        select c;

            return query;
        }

        //pattent-development-status
        public IQueryable<Child> GetChildByDevelopmentStatusPattern(string pattern)
        {
            var query = from c in Children
                        where c.DevelopmentStatus.Contains(pattern)
                        select c;

            return query;
        }

        //pattent-medical-indormation
        public IQueryable<Child> GetChildByMedicalInformationPattern(string pattern)
        {
            var query = from c in Children
                        where c.MedicalInformation.Contains(pattern)
                        select c;

            return query;
        }

        //pattent-birth-place
        public IQueryable<Child> GetChildByBirthPlacePattern(string pattern)
        {
            var query = from c in Children
                        where c.BirthPlace.Contains(pattern)
                        select c;

            return query;
        }

        //remove child
        public bool RemoveChild(int id)
        {
            int affectedRows = 0;
            var childForRemove = Children.FirstOrDefault(u => u.Id == id);
            Children.Remove(childForRemove);

            bool isSaveSuccessful = SaveChangesWithValidation(Context, ref affectedRows);

            return isSaveSuccessful;
        }

        //update child
        public bool updateChild(Child child)
        {
            var selectedChild = Context.Children.FirstOrDefault(u => u.Id == child.Id);
            if (selectedChild != null)
            {
                selectedChild.ProfileImage = child.ProfileImage;
                selectedChild.PIN = child.PIN;
                selectedChild.FirstName = child.FirstName;
                selectedChild.LastName = child.LastName;
                selectedChild.DateOfBirth = child.DateOfBirth;
                selectedChild.Sex = child.Sex;
                selectedChild.Adress = child.Adress;
                selectedChild.Nationality = child.Nationality;
                selectedChild.DevelopmentStatus = child.DevelopmentStatus;
                selectedChild.MedicalInformation = child.MedicalInformation;
                selectedChild.BirthPlace = child.BirthPlace;
                selectedChild.Id_Group = child.Id_Group;

                Context.SaveChanges();
                return true;
            } else
            {
                return false;
            }
        }

        //add child
        public bool addChild(Child child, List<Parent> parents)
        {
            foreach (var parent in parents)
            {
                var existingParent = Context.Parents.FirstOrDefault(p => p.Id == parent.Id);
                if (existingParent != null)
                {
                    child.Parents.Add(existingParent);
                } else return false;
            }

            Context.Children.Add(child);

            int affectedRows = 0;

            bool isSaveSuccessful = SaveChangesWithValidation(Context, ref affectedRows);

            return isSaveSuccessful;
        }

        //get child by PIN
        public Child getChildByPIN(string pin)
        {
            return Children.FirstOrDefault(c => c.PIN == pin);
        }

        // get children by parent
        public List<Child> GetChildrenByParent(Parent parent)
        {
            var children = from c in Children
                           where c.Parents.Any(p => p.Id == parent.Id)
                           select c;

            return children.ToList();
        }

        private bool SaveChangesWithValidation(DbContext context, ref int affectedRows)
        {
            try
            {
                affectedRows = context.SaveChanges();
            } catch (DbEntityValidationException ex)
            {
                // Iterirajte kroz sve entitete koji su imali valjanosne greške
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    // Iterirajte kroz sve greške valjanosti za svaki entitet
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Console.WriteLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                    }
                }

                // Vratite false jer je došlo do greške pri spremanju
                return false;
            }

            // Vratite true ako je barem jedan red promijenjen u bazi podataka
            return affectedRows > 0;
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
