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

    public class StaffEducationRepository : GenericRepository<StaffEducation>, IStaffEducationRepository
    {
        public StaffEducationRepository(DataContext context, ILoggerFactory loggerFactory) :
       base(context, loggerFactory, "StaffEducationRepository")
        {

        }

        public List<StaffEducation> GetOldEducationList(int StaffID)
        {
            var records = this.entities.Where(e => e.Active == true && e.SystemActive == true && e.StaffID == StaffID).AsNoTracking().ToList();

            return records;
        }

    }
}
