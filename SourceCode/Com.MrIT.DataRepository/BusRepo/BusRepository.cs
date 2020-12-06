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
    public class BusRepository : GenericRepository<Bus>, IBusRepository
    {
        public BusRepository(DataContext context, ILoggerFactory loggerFactory) :
       base(context, loggerFactory, "BusRepository")
        {

        }
        public Bus GetBus(int id)
        {
            var record = this.entities.Include(e => e.busTypes).Include(e => e.busTypes)
                  .Where(e => e.ID == id).SingleOrDefault();

            return record;
        }

    }
}
