using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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

        //get users id
        public string GetID(string id)
        {
            var user = Users.FirstOrDefault(u => u.PIN == id);
            return user?.PIN;
        }

        //get user by username
        public User GetUserByUsername(string username)
        {
            var user = Users.FirstOrDefault(u => u.Username == username);
            return user;
        }

        //registrate new user
        public bool RegistrateUser(User userForRegistration)
        {
            Users.Add(userForRegistration);
            int affectedRows = 0;

            bool isSaveSuccessful = SaveChangesWithValidation(Context, ref affectedRows);

            return isSaveSuccessful;
        }

        //remove user
        public bool RemoveUser(string username, string pin)
        {
            int affectedRows = 0;
            var userForRemove = Users.FirstOrDefault(u => u.Username == username || u.PIN == pin);
            Users.Remove(userForRemove);

            bool isSaveSuccessful = SaveChangesWithValidation(Context, ref affectedRows);
            
            return isSaveSuccessful;
        }

        //get users username
        public User GetUserUsername(string username)
        {
            return Users.FirstOrDefault(u => u.Username == username);
        }

        //pattern-username
        public IQueryable<User> GetUserByUsernamePattern(string pattern)
        {
            var query = from u in Users
                        where u.Username.Contains(pattern)
                        select u;

            return query;
        }

        //pattern-PIN
        public IQueryable<User> GetUserByPINPattern(string pattern)
        {
            var query = from u in Users
                        where u.PIN.Contains(pattern)
                        select u;

            return query;
        }

        //pattern-Firstname
        public IQueryable<User> GetUserByFirstNamePattern(string pattern)
        {
            var query = from u in Users
                        where u.FirstName.Contains(pattern)
                        select u;

            return query;
        }

        //pattent-Lastname
        public IQueryable<User> GetUserByLastNamePattern(string pattern)
        {
            var query = from u in Users
                        where u.LastName.Contains(pattern)
                        select u;

            return query;
        }

        //pattern-flname
        public IQueryable<User> GetUserByFirstNameAndLastNamePattern(string pattern)
        {
            var query = from u in Users
                        where u.FirstName.Contains(pattern) && u.LastName.Contains(pattern)
                        select u;

            return query;
        }

        //pattern-email
        public IQueryable<User> GetUserByEmailPattern(string pattern)
        {
            var query = from u in Users
                        where u.Email.Contains(pattern)
                        select u;

            return query;
        }

        //all users
        public IQueryable<User> GetAllUsers()
        {
            var query = from u in Users
                        select u;

            return query;
        }

        //update user
        public bool updateUser(User user)
        {
            var selectedUser = Context.Users.FirstOrDefault(u => u.Id == user.Id);
            if (selectedUser != null)
            {
                selectedUser.ProfileImage = user.ProfileImage;
                selectedUser.FirstName = user.FirstName;
                selectedUser.LastName = user.LastName;
                selectedUser.Username = user.Username;
                selectedUser.PIN = user.PIN;
                selectedUser.Email = user.Email;
                selectedUser.Telephone = user.Telephone;
                selectedUser.DateOfBirth = user.DateOfBirth;
                selectedUser.Sex = user.Sex;
                selectedUser.Id_role = user.Id_role;
                selectedUser.Password = user.Password;
                selectedUser.Salt = user.Salt;

                Context.SaveChanges();
                return true;
            } else
            {
                return false;
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
