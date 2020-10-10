using Com.MrIT.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.MrIT.Services
{
    public interface IMrUserService
    {
        VmMrUser ValidateUser(VmMrUser user);

        VmGenericServiceResult CheckEmailForRegister(string email, string userType, string fullName = "");

        VmGenericServiceResult AddMrUser(VmMrUser mrUser);

        VmGenericServiceResult UpdateMrUser(VmMrUser mrUser);

        VmMrUserPage GetMrUserByPage(string keyword, int page, int totalRecord);

   

        VmMrUser GetMrUserByID(int id);

       

        VmGenericServiceResult DisableAccount(int userID, string password, string userType);

        VmGenericServiceResult ChangePassword(int userID, string currentPass, string newPass, string userType);
    }
}
