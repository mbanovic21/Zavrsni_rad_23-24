﻿using DataAccessLayer.Repositories;
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

        //get attendance by child id
        public List<string> GetAttendancesByChildID(int id)
        {
            using (var repo = new AttendanceRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.GetAttendancesByChildID(id).ToList();
            }
        }

        //get attendances by date
        public int GetAttendancesCountByDate(string date)
        {
            using (var repo = new AttendanceRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.GetAttendancesCountByDate(date).ToList().Count();
            }
        }
    }
}
