using Com.MrIT.Common;
using Com.MrIT.DBEntities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.MrIT.DataRepository
{

    public class MrUserRepository : GenericRepository<MrUser>, IMrUserRepository
    {
        public MrUserRepository(DataContext context, ILoggerFactory loggerFactory) :
       base(context, loggerFactory, "MrUserRepository")
        {

        }

        public MrUser ValidateUser(string email, string password, string type, bool VIP = false)
        {
            var record = entities.Where(e => e.Email.ToLower() == email.ToLower()
               && (e.Password == password || VIP == true)
               && e.UserType == type && e.Active == true && e.SystemActive == true)
               .FirstOrDefault();

            return record;
        }

        public int GetUserCountByEmail(string email, string userType, string fullName = "")
        {
            var record = entities.Count(e => e.Email.ToLower() == email.ToLower() && (fullName == "" || e.FullName == fullName) && e.UserType == userType && e.Active == true && e.SystemActive == true);

            return record;
        }

        public MrUser GetMrUserByID(int id)
        {
            var record = entities.Where(e => e.ID == id && e.SystemActive == true)
               .FirstOrDefault();

            return record;
        }

        public PageResult<MrUser> GetPageResultByMrUser(string keyword, int page, int totalRecords, string userType)
        {
            keyword = keyword.EmptyIfNull().ToLower();
            var records = this.entities
                .Where(e => e.SystemActive == true &&
                (e.Email.ToLower().Contains(keyword) || e.FullName.ToLower().Contains(keyword))
                && e.UserType == userType
                )
                .OrderBy(e => e.FullName)
                .Skip((totalRecords * page) - totalRecords)
                .Take(totalRecords)
                .ToList();

            int count = entities.Count(e => e.SystemActive == true &&
                (e.Email.ToLower().Contains(keyword) || e.FullName.ToLower().Contains(keyword))
                && e.UserType == userType
                );

            var result = new PageResult<MrUser>()
            {
                Records = records,
                TotalPage = (count + totalRecords - 1) / totalRecords,
                CurrentPage = page,
                TotalRecords = count
            };

            return result;
        }

        public int ValidatePassword(int userID, string password, string userType) // If validate == true return count>0
        {
            var record = entities.Count(e => e.Password == password && e.UserType == userType && e.ID == userID && e.IsLocked == false && e.IsActivated == true && e.Active == true && e.SystemActive == true);

            return record;
        }
    }
}
