using DataAccessLayer.Repositories;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DBServices
{
    public class WeeklyScheduleServices
    {
        //get ws id by dates
        public int GetWeeklySchedulesIDByDates(string weekDisplay)
        {
            var startDate = weekDisplay.Split(' ')[0];
            var endDate = weekDisplay.Split(' ')[2];

            using (var repo = new WeeklyScheduleRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.GetWeeklySchedulesIDByDates(startDate, endDate);
            }
        }
    }
}
