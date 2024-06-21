using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
            return Notes.Where(n => n.Id_child == child.Id);
        }

        public bool RemoveNotes(List<Note> notes)
        {
            int affectedRows = 0;

            foreach(var note in notes)
            {
                var noteToRemove = Notes.Find(note.Id);
                if (noteToRemove != null)
                {
                    Notes.Remove(noteToRemove);
                }
            }

            return SaveChangesWithValidation(Context, ref affectedRows);
        }

        public bool UpdateNote(Note note)
        {
            int affectedRows = 0;

            var noteForUpdate = Context.Notes.FirstOrDefault(n => n.Id == note.Id);

            if (noteForUpdate != null)
            {
                noteForUpdate.Date = note.Date;
                noteForUpdate.Behaviour = note.Behaviour;
                noteForUpdate.Description = note.Description;

                bool success = SaveChangesWithValidation(Context, ref affectedRows);
                if (!success)
                {
                    Console.WriteLine("Failed to save changes.");
                }
                return success;
            } else
            {
                Console.WriteLine($"Note with Id {note.Id} not found.");
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
