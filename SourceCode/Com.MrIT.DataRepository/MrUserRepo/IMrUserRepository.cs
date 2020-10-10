using Com.MrIT.Common;
using Com.MrIT.DBEntities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.MrIT.DataRepository
{
    public interface IMrUserRepository : IGenericRepository<MrUser>
    {
        MrUser ValidateUser(string email, string password, string type, bool VIP = false);

        int GetUserCountByEmail(string email, string userType, string fullName = "");

        PageResult<MrUser> GetPageResultByMrUser(string keyword, int page, int totalRecords, string userType);

        MrUser GetMrUserByID(int id);

        int ValidatePassword(int userID, string password, string userType);
    }
}
