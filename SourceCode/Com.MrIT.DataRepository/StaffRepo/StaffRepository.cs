using Com.MrIT.Common;
using Com.MrIT.DBEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.MrIT.DataRepository
{

    public class StaffRepository : GenericRepository<Staff>, IStaffRepository
    {
        public StaffRepository(DataContext context, ILoggerFactory loggerFactory) :
       base(context, loggerFactory, "StaffRepository")
        {

        }

        public Staff GetStaff(int id)
        {
            var record = this.entities.Include(e => e.Educations).Include(e => e.Experiences)
                  .Where(e => e.ID == id).SingleOrDefault();

            return record;
        }

        public PageResult<Staff> GetStaffListByPage(string keyword, int page, int totalRecords = 10)
        {
            var records =  this.entities.Where(e => e.SystemActive == true && e.Active == true
                            && (keyword == "" || e.Name.ToLower().Contains(keyword.ToLower()) || e.Code.ToLower().Contains(keyword.ToLower()) || e.Address.ToLower().Contains(keyword.ToLower())))
                           .OrderBy(e => e.CreatedOn)
                           .Skip((totalRecords * page) - totalRecords)
                           .Take(totalRecords).AsNoTracking()
                           .ToList();

            int count =  this.entities.Count(e => e.SystemActive == true && e.Active == true && (keyword == "" || e.Name.ToLower().Contains(keyword.ToLower()) || e.Code.ToLower().Contains(keyword.ToLower()) || e.Address.ToLower().Contains(keyword.ToLower())));

            var result = new PageResult<Staff>()
            {
                Records = records,
                TotalPage = (count + totalRecords - 1) / totalRecords,
                CurrentPage = page,
                TotalRecords = count
            };

            result.PreviousPage = 1;
            if(result.CurrentPage  > 1)
            {
                result.PreviousPage = result.CurrentPage - 1;
            }

            result.NextPage = result.TotalPage;
            if(result.CurrentPage < result.TotalPage)
            {
                result.NextPage = result.CurrentPage + 1;
            }

            return result;
        }
    }
}
