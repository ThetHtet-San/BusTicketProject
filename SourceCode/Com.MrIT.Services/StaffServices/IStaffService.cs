using Com.MrIT.Common;
using Com.MrIT.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
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

        VmGenericServiceResult DeleteStaff(int id);

        PageResult<VmStaff> GetStaffListByPage(string keyword, int page, int recordPerPage);


        #endregion

        #region Education

        VmGenericServiceResult CreateStaffEducation(VmStaffEducation staffEducation);

        VmGenericServiceResult EditStaffEducation(VmStaffEducation staffEducation);

        VmStaffEducation GetStaffEducation(int id);

        #endregion

        #region Experience

        VmGenericServiceResult CreateStaffExperience(VmStaffExperience staffExperience);

        #endregion
    }
}
