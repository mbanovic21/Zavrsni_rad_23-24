using DataAccessLayer.Repositories;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DBServices
{
    public class AttendanceServices
    {
        //add attendance
        public bool AddAttendance(List<Child> children, Attendance attendance)
        {
            using(var repo = new AttendanceRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.AddAttendance(children, attendance);
            }
        }
    }
}
