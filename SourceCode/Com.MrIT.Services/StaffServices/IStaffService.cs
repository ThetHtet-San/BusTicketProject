using Com.MrIT.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.MrIT.Services
{
    public interface IStaffService
    {
       
        #region Staff
        VmGenericServiceResult CreateStaff(VmStaff staff);

        VmGenericServiceResult UpdateStaff(VmStaff staff);

        List<VmStaff> GetAllStaff();

        VmStaff GetStaff(int id);


        #endregion

        #region Education

        VmGenericServiceResult CreateStaffEducation(VmStaffEducation staffEducation);

        #endregion

        #region Experience

        VmGenericServiceResult CreateStaffExperience(VmStaffExperience staffExperience);

        #endregion
    }
}
