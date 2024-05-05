using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class UserRepository : IDisposable
    {
        private PMSmodel Context { get; set; }
        private DbSet<User> Users { get; set; }
        public UserRepository(PMSmodel context)
        {
            Context = context;
            Users = Context.Set<User>();
        }

        public string GetID(string id)
        {
            var user = Users.FirstOrDefault(u => u.OIB == id);
            return user?.OIB;
        }

        public User GetUserByUsername(string username) 
        {
            var user = Users.FirstOrDefault(u => u.Username == username);
            return user;
        }

        private void SaveChanges()
        {
            Context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
