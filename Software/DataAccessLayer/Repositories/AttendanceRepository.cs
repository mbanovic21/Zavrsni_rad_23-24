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
    public class AttendanceRepository : IDisposable
    {
        private readonly PreschoolManagmentModel Context;
        private readonly DbSet<Attendance> Attendances;

        public AttendanceRepository(PreschoolManagmentModel context)
        {
            Context = context;
            Attendances = Context.Set<Attendance>();
        }

        public bool AddAttendance(List<Child> children, Attendance attendanceTemplate)
        {
            int affectedRows = 0;

            var attendances = new List<Attendance>();

            foreach (var child in children)
            {
                var childAttendance = new Attendance
                {
                    Date = attendanceTemplate.Date,
                    isPresent = attendanceTemplate.isPresent,
                    Id_Child = child.Id
                };

                attendances.Add(childAttendance);
            }

            Attendances.AddRange(attendances);

            bool isSaveSuccessful = SaveChangesWithValidation(Context, ref affectedRows);
            return isSaveSuccessful;
        }

        //Get attendances by child id
        public IQueryable<string> GetAttendancesByChildID(int childID)
        {
            var attendances = Attendances
                              .Where(a => a.Id_Child == childID)
                              .Select(a => a.Date);

            return attendances.AsQueryable();
        }


        //SaveChanges()
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
