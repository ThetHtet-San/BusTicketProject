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

        public PageResult<Staff> GetStaffListByPage(string keyword, int page, int recordPerPage = 10)
        {
            var records =  this.entities.Where(e => e.SystemActive == true && e.Active == true
                            && (keyword == "" || e.Name.ToLower().Contains(keyword.ToLower()) || e.Code.ToLower().Contains(keyword.ToLower()) || e.Address.ToLower().Contains(keyword.ToLower())))
                           .OrderBy(e => e.Name)
                           .Skip((recordPerPage * page) - recordPerPage)
                           .Take(recordPerPage).AsNoTracking()
                           .ToList();

            int count =  this.entities.Count(e => e.SystemActive == true && e.Active == true && (keyword == "" || e.Name.ToLower().Contains(keyword.ToLower()) || e.Code.ToLower().Contains(keyword.ToLower()) || e.Address.ToLower().Contains(keyword.ToLower())));

            //var result = new PageResult<Staff>()
            //{
            //    Records = records,
            //    TotalPage = (count + recordPerPage - 1) / recordPerPage, //(count/recorPerPage)
            //    CurrentPage = page,
            //    TotalRecords = count
            //};

            var result = new PageResult<Staff>();
            result.Records = records;
            result.TotalPage = (count + recordPerPage - 1) / recordPerPage;
            result.CurrentPage = page;
            result.TotalRecords = count;

            //104 records / 10 = 10.4 / 10 
            //104 + 10 - 1 = 113 / 10 = 11.3 = 11
            //100 + 10 -1 = 109 /10 = 10.9 = 10
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
