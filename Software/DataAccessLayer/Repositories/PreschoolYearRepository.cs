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
    public class PreschoolYearRepository : IDisposable
    {
        private PreschoolManagmentModel Context;
        private DbSet<PreeschoolYear> PreschoolYears;

        public PreschoolYearRepository(PreschoolManagmentModel context)
        {
            Context = context;
            PreschoolYears = Context.Set<PreeschoolYear>();
        }

        public bool AddNewPreschoolYear(PreeschoolYear preeschoolYear, List<Group> groups)
        {
            var existingYear = PreschoolYears.Any(py => py.Year == preeschoolYear.Year);
            if (existingYear) return false;

            foreach (var g in groups)
            {
                Context.Groups.Attach(g);
                preeschoolYear.Groups.Add(g);
            }
            PreschoolYears.Add(preeschoolYear);
            
            int affectedRows = 0;
            bool isSaveSuccessful = SaveChangesWithValidation(Context, ref affectedRows);

            return isSaveSuccessful;
        }

        public IQueryable<ICollection<Group>> GetGroupsForYear(string year)
        {
            var query = from y in PreschoolYears 
                        where y.Year == year 
                        select y.Groups;

            return query.AsQueryable();
        }

        public IQueryable<string> GetAllYearsName()
        {
            var query = from y in PreschoolYears 
                        select y.Year;

            return query;
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
