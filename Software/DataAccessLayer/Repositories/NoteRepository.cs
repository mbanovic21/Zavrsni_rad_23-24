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
    public class NoteRepository : IDisposable
    {
        private PreschoolManagmentModel Context;
        private DbSet<Note> Notes;

        public NoteRepository(PreschoolManagmentModel context)
        {
            Context = context;
            Notes = Context.Set<Note>();
        }

        public bool AddNote(Note note)
        {
            Notes.Add(note);
            
            int affectedRows = 0;
            bool isSaveSuccessful = SaveChangesWithValidation(Context, ref affectedRows);

            return isSaveSuccessful;
        }

        public IQueryable<Note> GetNotesByChild(Child child)
        {
            var query = from n in Notes where n.Id_child == child.Id select n;
            return query;
        }

        private bool SaveChangesWithValidation(DbContext context, ref int affectedRows)
        {
            try
            {
                affectedRows = context.SaveChanges();
            } catch (DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Console.WriteLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                    }
                }
                return false;
            }
            return affectedRows > 0;
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
