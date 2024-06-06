using DataAccessLayer.Repositories;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DBServices
{
    public class DayService
    {
        public void addNewDay(Day day)
        {
            using (var repo = new DayRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                repo.AddNewDay(day);
            }
        }

        public List<Day> getDaysByWeeklySchdulesID(int id)
        {
            using (var repo = new DayRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.getDaysByWeeklySchduleID(id).ToList();
            }
        }

        public List<User> getUsersByDayId(int id)
        {
            using (var repo = new DayRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.getUsersByDayId(id).ToList();
            }
        }

        public List<Day> getDaysByWeeklySchduleAndUsername(int week, string username)
        {
            using (var repo = new DayRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.getDaysByWeeklySchduleAndUsername(week, username.ToString()).ToList();
            }
        }

        public bool isDateAlredyTaken(string date, string day)
        {
            using (var repo = new DayRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.isDateAlredyTaken(date, day);
            }
        }

        public Day getDayByDateAndName(string date, string day)
        {
            using (var repo = new DayRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.getDayByDateAndName(date, day);
            }
        }

        public bool isDayUpdated(Day updatedDay)
        {
            using (var repo = new DayRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.isDayUpdated(updatedDay);
            }
        }
    }
}
