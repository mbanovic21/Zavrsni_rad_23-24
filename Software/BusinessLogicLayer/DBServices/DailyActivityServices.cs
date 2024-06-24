using DataAccessLayer.Repositories;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DBServices
{
    public class DailyActivityServices
    {
        //all da
        public List<DailyActivity> GetAllActivitiesByDate(string date)
        {
            using (var repo = new DailyActivityRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.GetAllActivitiesByDate(date).ToList();
            }

        }

        //add da
        public bool AddDailyActivity(DailyActivity activity, Day day)
        {
            using (var repo = new DailyActivityRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.AddDailyActivity(activity, day);
            }
        }

        //charts da
        public List<(string EmployeeName, string DayOfWeek, int ActivityCount)> GetEmployeeActivities()
        {
            using (var repo = new DailyActivityRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.GetEmployeeActivities();
            }
        }

        //update da
        public bool UpdateDailyActivity(DailyActivity dailyActivity, Day day)
        {
            using (var repo = new DailyActivityRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.UpdateDailyActivity(dailyActivity, day);
            }
        }

        //remove da
        public bool DeleteDailyActivity(int dailyActivityId)
        {
            using (var repo = new DailyActivityRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.DeleteDailyActivity(dailyActivityId);
            }
        }
    }
}
