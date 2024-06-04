using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class ParentRepository : IDisposable
    {
        private PreschoolManagmentModel Context { get; set; }
        private DbSet<Parent> Parents { get; set; }
        public ParentRepository(PreschoolManagmentModel context)
        {
            Context = context;
            Parents = Context.Set<Parent>();
        }

        //registrate new parent
        public bool RegistrateParents(List<Parent> parentsForRegistration)
        {
            foreach (var parent in parentsForRegistration)
            {
                Parents.Add(parent);
            }
            int affectedRows = 0;

            bool isSaveSuccessful = SaveChangesWithValidation(Context, ref affectedRows);

            return isSaveSuccessful;
        }

        //remove parent
        public bool RemoveParents(List<Parent> parentsForRemoving)
        {
            int affectedRows = 0;
            foreach (var parent in parentsForRemoving)
            {
                var parentForRemove = Parents.FirstOrDefault(p => p.Id == parent.Id);
                Parents.Remove(parentForRemove);
            }

            bool isSaveSuccessful = SaveChangesWithValidation(Context, ref affectedRows);

            return isSaveSuccessful;
        }

        // remove parents by child
        public bool RemoveParentsByChild(Child child)
        {
            int affectedRows = 0;

            var parents = from p in Parents
                        where p.Children.Any(c => c.Id == child.Id)
                        select p;

            foreach (var parent in parents.ToList())
            {
                Parents.Remove(parent);
            }

            bool isSaveSuccessful = SaveChangesWithValidation(Context, ref affectedRows);

            return isSaveSuccessful;
        }

        // get parents by child
        public List<Parent> GetParentsByChild(Child child)
        {
            var parents = from p in Parents
                          where p.Children.Any(c => c.Id == child.Id)
                          select p;

            return parents.ToList();
        }

        // get mothers
        public IQueryable<Parent> GetMothers()
        {
            var mothers = from p in Parents
                          where p.Sex == "Ženski"
                          select p;

            return mothers;
        }

        // get mothers
        public IQueryable<Parent> GetFathers()
        {
            var fathers = from p in Parents
                          where p.Sex == "Muški"
                          select p;

            return fathers;
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
