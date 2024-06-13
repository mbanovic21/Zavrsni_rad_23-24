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

        //set all dates when crating new preschool year
        public void SetStartAndEndDateWhenCreatingNewPreschoolYear(string lastTwoYearNumbers)
        {
            var fullYearString = $"20{lastTwoYearNumbers}";
            var fullYear = int.Parse(fullYearString);
            var listWeeklySchedules = new List<WeeklySchedule>();

            DateTime startDate = new DateTime(fullYear, 1, 1);

            while (startDate.Year == fullYear)
            {
                DateTime endDate = startDate.AddDays(6);
                var weekDisplayStartDate = $"{startDate:dd.MM.yyyy.}";
                var weekDisplayEndDate = $"{endDate:dd.MM.yyyy.}";

                var newWeek = new WeeklySchedule
                {
                    StartDate = weekDisplayStartDate,
                    EndDate = weekDisplayEndDate
                };
                listWeeklySchedules.Add(newWeek);
                startDate = startDate.AddDays(7);
            }

            using (var repo = new WeeklyScheduleRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                repo.SetStartAndEndDateWhenCreatingNewPreschoolYear(listWeeklySchedules);
            }
        }
    }
}
