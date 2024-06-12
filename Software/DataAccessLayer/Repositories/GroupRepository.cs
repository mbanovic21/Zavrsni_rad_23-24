using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class GroupRepository : IDisposable
    {
        private PreschoolManagmentModel Context;
        private DbSet<Group> Group;

        public GroupRepository(PreschoolManagmentModel context)
        {
            Context = context;
            Group = Context.Set<Group>();
        }

        public int? GetGroupIdByName(string name)
        {
            var group = Group.FirstOrDefault(g => g.Name == name);
            if (group != null)
            {
                return group.Id;
            }
            return null;
        }

        public IQueryable<Group> GetGroupByName(string name)
        {
            var queri = from g in Group where g.Name == name select g;
            return queri;
        }

        public void AddGroup(Group newGroup)
        {
            Group.Add(newGroup);
            Context.SaveChanges();
        }

        public IQueryable<Group> GetAllGroups()
        {
            var queri = from g in Group select g;
            return queri;
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
