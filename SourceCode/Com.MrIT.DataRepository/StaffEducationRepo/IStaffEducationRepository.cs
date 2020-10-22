using Com.MrIT.Common;
using Com.MrIT.DBEntities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.MrIT.DataRepository
{
    public interface IStaffEducationRepository : IGenericRepository<StaffEducation>
    {
        List<StaffEducation> GetOldEducationList(int StaffID);
    }
}
