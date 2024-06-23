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
    public class DailyActivityRepository : IDisposable
    {
        private PreschoolManagmentModel Context;
        private DbSet<DailyActivity> DailyActivity;

        public DailyActivityRepository(PreschoolManagmentModel context)
        {
            Context = context;
            DailyActivity = Context.Set<DailyActivity>();
        }

        public IQueryable<DailyActivity> GetAllActivitiesByDate(string date)
        {
            var query = from a in DailyActivity
                        where a.Days.Any(d => d.Date == date)
                        select a;

            return query;
        }
        public bool AddDailyActivity(DailyActivity activity, Day day)
        {
            int affectedRows = 0;
            
            Context.Days.Attach(day);
            activity.Days.Add(day);
            DailyActivity.Add(activity);

            bool isSaveSuccessful = SaveChangesWithValidation(Context, ref affectedRows);

            return isSaveSuccessful;
        }

        //Days activity chart
        public List<(string EmployeeName, string DayOfWeek, int ActivityCount)> GetEmployeeActivities()
        {
            var activities = Context.Days
            .Include(d => d.Users)
            .Include(d => d.DailyActivities)
            .SelectMany(d => d.Users, (d, u) => new
            {
                Day = d.Name,
                Employee = u.FirstName + " " + u.LastName,
                ActivityCount = d.DailyActivities.Count
            })
            .ToList()
            .Select(x => (x.Employee, x.Day, x.ActivityCount))
            .ToList();

            return activities;
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
