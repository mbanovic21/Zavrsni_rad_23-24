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
    public class DayRepository : IDisposable
    {
        public PreschoolManagmentModel Context;
        public DbSet<Day> Days;

        public DayRepository(PreschoolManagmentModel context)
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

        public IQueryable<Day> getDaysByWeeklySchduleID(int weeklyScheduleID)
        {
            var query = from d in Days
                        where d.Id_WeeklySchedule == weeklyScheduleID
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

        public IQueryable<Day> getDaysByWeeklySchduleAndUsername(int weeklyScheduleID, string username)
        {
            var query = from d in Days
                        where d.Id_WeeklySchedule == weeklyScheduleID && d.Users.Any(u => u.Username == username)
                        select d;

            return query;
        }

        public bool isDateAlredyTaken(string date, string day)
        {
            var selectedDate = Days.FirstOrDefault(d => d.Date == date && d.Name == day);
            return selectedDate != null ? true : false;
        }

        public Day getDayByDateAndName(string date, string day)
        {
            return Days.FirstOrDefault(d => d.Date == date && d.Name == day);
        }

        public bool isDayUpdated(Day updatedDay)
        {
            var selectedDay = Days.FirstOrDefault(d => d.Id == updatedDay.Id);

            if (selectedDay != null)
            {
                // Obrišite postojeće korisnike
                selectedDay.Users.Clear();

                // Dodajte nove korisnike
                foreach (var user in updatedDay.Users)
                {
                    // Provjerite je li korisnik već pridružen kontekstu
                    var existingUser = Context.Users.Find(user.Id);
                    if (existingUser != null)
                    {
                        // Ako je korisnik već pridružen kontekstu, preskočite dodavanje
                        selectedDay.Users.Add(existingUser);
                    } else
                    {
                        // Ako korisnik nije pridružen kontekstu, dodajte ga
                        selectedDay.Users.Add(user);
                    }
                }

                // Spremite promjene
                Context.SaveChanges();

                return true;
            }
            return false;
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
