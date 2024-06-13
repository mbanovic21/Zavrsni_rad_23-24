using DataAccessLayer.Repositories;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DBServices
{
    public class PreschoolYearServices
    {
        public bool AddNewPreschoolYear(PreeschoolYear preeschoolYear, List<Group> group)
        {
            using (var repo = new PreschoolYearRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.AddNewPreschoolYear(preeschoolYear, group);
            }
        }
        public List<Group> GetGroupsForYear(string year)
        {
            using (var repo = new PreschoolYearRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.GetGroupsForYear(year).SelectMany(x => x).ToList();
            }

        }
        public List<string> GetAllYears()
        {
            using (var repo = new PreschoolYearRepository(new DataAccessLayer.PreschoolManagmentModel()))
            {
                return repo.GetAllYearsName().ToList();
            }
        }
    }
}
