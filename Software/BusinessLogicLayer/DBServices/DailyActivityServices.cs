using DataAccessLayer.Repositories;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DBServices
{
    public class DailyActivityServices
    {
        public List<DailyActivity> GetAllActivitiesByDate(string date)
        {
            using (var repo = new DailyActivityRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.GetAllActivitiesByDate(date).ToList();
            }

        }
        public bool AddDailyActivity(DailyActivity activity, Day day)
        {
            using (var repo = new DailyActivityRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.AddDailyActivity(activity, day);
            }
        }

        public List<(string EmployeeName, string DayOfWeek, int ActivityCount)> GetEmployeeActivities()
        {
            using (var repo = new DailyActivityRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.GetEmployeeActivities();
            }
        }
    }
}
