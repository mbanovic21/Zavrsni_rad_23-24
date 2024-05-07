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
    public class UserRepository : IDisposable
    {
        private PreschoolManagmentModel Context { get; set; }
        private DbSet<User> Users { get; set; }
        public UserRepository(PreschoolManagmentModel context)
        {
            Context = context;
            Users = Context.Set<User>();
        }

        public string GetID(string id)
        {
            var user = Users.FirstOrDefault(u => u.PIN == id);
            return user?.PIN;
        }

        public User GetUserByUsername(string username)
        {
            var user = Users.FirstOrDefault(u => u.Username == username);
            return user;
        }

        public bool RegistrateUser(User userForRegistration)
        {
            Users.Add(userForRegistration);
            int affectedRows = 0;

            try
            {
                affectedRows = Context.SaveChanges();
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
