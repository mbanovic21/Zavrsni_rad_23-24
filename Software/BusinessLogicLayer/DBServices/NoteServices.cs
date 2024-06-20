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
        public bool AddNote(Note note)
        {
            using (var repo = new NoteRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.AddNote(note);
            }
        }

        public List<Note> GetNotesByChild(Child child)
        {
            using (var repo = new NoteRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.GetNotesByChild(child).ToList();
            }
        }
    }
}
