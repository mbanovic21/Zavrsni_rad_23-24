﻿using EntityLayer.Entities;
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
            var query = from user in Users
                        where user.OIB == id
                        select user.OIB;

            return query.ToString();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
