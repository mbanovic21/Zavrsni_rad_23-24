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
    public class WeeklyScheduleRepository : IDisposable
    {
        public PreschoolManagmentModel Context;
        public DbSet<WeeklySchedule> WeeklySchedules;

        public WeeklyScheduleRepository(PreschoolManagmentModel context)
        {
            Context = context;
            WeeklySchedules = Context.Set<WeeklySchedule>();
        }
        
        //Get ws by dates
        public int GetWeeklySchedulesIDByDates(string startDate, string endDate)
        {
            var weeklySchedule = WeeklySchedules.FirstOrDefault(ws => ws.StartDate == startDate && ws.EndDate == endDate);
            if(weeklySchedule != null)
            {
                return weeklySchedule.Id;
            }
            return -1;
        }

        //Set all dates when crating new preschool year
        public void SetStartAndEndDateWhenCreatingNewPreschoolYear(List<WeeklySchedule> weeklySchedules)
        {
            using (var transaction = Context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var ws in weeklySchedules)
                    {
                        WeeklySchedules.Add(ws);
                    }

                    int affectedRows = 0;
                    bool isSaveSuccessful = SaveChangesWithValidation(Context, ref affectedRows);

                    if (isSaveSuccessful) transaction.Commit();
                    else transaction.Rollback();

                    //return isSaveSuccessful;
                }catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    transaction.Rollback();
                    //return false;
                } 
            }
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
