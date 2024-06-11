using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public void AddNewPreschoolYear(PreeschoolYear preeschoolYear, List<Group> groups)
        {
            foreach (var g in groups)
            {
                Context.Groups.Attach(g);
                preeschoolYear.Groups.Add(g);
            }

            PreschoolYears.Add(preeschoolYear);
            Context.SaveChanges();
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

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
