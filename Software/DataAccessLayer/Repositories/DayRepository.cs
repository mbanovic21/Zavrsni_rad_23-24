using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class DayRepository : IDisposable
    {
        public PMSModel Context;
        public DbSet<Day> Days;

        public DayRepository(PMSModel context)
        {
            Context = context;
            Days = Context.Set<Day>();
        }

        public void AddNewDay(Day day)
        {
            foreach (var user in day.Users)
            {
                if (!Context.Users.Local.Any(u => u.Id == user.Id))
                {
                    Context.Users.Attach(user);
                }
            }
            Days.Add(day);
            Context.SaveChanges();
        }

        public IQueryable<Day> getDaysByWeeklySchdule(int week)
        {
            var query = from d in Days
                        where d.Id_WeeklySchedule == week
                        select d;

            return query;
        }

        public IQueryable<User> getUsersByDayId(int id)
        {
            var day = Context.Days.FirstOrDefault(d => d.Id == id);

            var query = from user in day.Users
                        where day.Id == id
                        select user;

            return query.AsQueryable();
        }

        public IQueryable<Day> getDaysByWeeklySchduleAndUsername(int week, string username)
        {
            var query = from d in Days
                        where d.Id_WeeklySchedule == week && d.Users.Any(u => u.Username.ToString() == username.ToString())
                        select d;

            return query;
        }


        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
