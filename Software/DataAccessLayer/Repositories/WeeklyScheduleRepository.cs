using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
