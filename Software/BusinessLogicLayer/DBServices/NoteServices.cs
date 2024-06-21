using DataAccessLayer.Repositories;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class NoteServices
    {
        //add notes
        public bool AddNote(Note note)
        {
            using (var repo = new NoteRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.AddNote(note);
            }
        }

        //get notes by child id
        public List<Note> GetNotesByChild(Child child)
        {
            using (var repo = new NoteRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.GetNotesByChild(child).ToList();
            }
        }

        //remove notes
        public bool RemoveNotes(List<Note> notes)
        {
            using (var repo = new NoteRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.RemoveNotes(notes);
            }
        }

        //update note
        public bool UpdateNote(Note note)
        {
            using (var repo = new NoteRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.UpdateNote(note);
            }
        }
    }
}
