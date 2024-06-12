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
    public class GroupRepository : IDisposable
    {
        private PreschoolManagmentModel Context;
        private DbSet<Group> Group;

        public GroupRepository(PreschoolManagmentModel context)
        {
            Context = context;
            Group = Context.Set<Group>();
        }

        //group id by name 
        public int? GetGroupIdByName(string name)
        {
            var group = Group.FirstOrDefault(g => g.Name == name);
            if (group != null)
            {
                return group.Id;
            }
            return null;
        }

        //group by name
        public IQueryable<Group> GetGroupByName(string name)
        {
            var queri = from g in Group where g.Name == name select g;
            return queri;
        }

        //add group
        public bool AddGroup(Group newGroup)
        {
            Group.Add(newGroup);
            
            int affectedRows = 0;
            bool isSaveSuccessful = SaveChangesWithValidation(Context, ref affectedRows);

            return isSaveSuccessful;
        }

        //get all groups
        public IQueryable<Group> GetAllGroups()
        {
            var queri = from g in Group select g;
            return queri;
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
