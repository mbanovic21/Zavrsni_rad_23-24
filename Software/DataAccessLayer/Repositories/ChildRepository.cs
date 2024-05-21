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
            var userForRemove = Children.FirstOrDefault(u => u.Id == id);
            Children.Remove(userForRemove);

            bool isSaveSuccessful = SaveChangesWithValidation(Context, ref affectedRows);

            return isSaveSuccessful;
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
