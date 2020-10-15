using Com.MrIT.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.MrIT.Services
{
    public interface IStaffService
    {
        VmGenericServiceResult CreateStaff(VmStaff staff);

        VmGenericServiceResult UpdateStaff(VmStaff staff);

        List<VmStaff> GetAllStaff();

        VmStaff GetStaff(int id);

        VmGenericServiceResult CreateStaffEducation(VmStaffEducation staffEducation);
    }
}
