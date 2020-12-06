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
    public class SeatRepository : GenericRepository<Seat>,ISeatRepository
    {
        public SeatRepository(DataContext context, ILoggerFactory loggerFactory) :
       base(context, loggerFactory, "SeatRepository")
        {

        }

    }
}
