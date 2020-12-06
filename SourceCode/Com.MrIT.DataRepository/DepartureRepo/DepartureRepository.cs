using System;
using System.Collections.Generic;
using System.Text;
using Com.MrIT.DBEntities;
using Com.MrIT.DBEntities.Entities;
using Microsoft.Extensions.Logging;


namespace Com.MrIT.DataRepository
{
   public class DepartureRepository : GenericRepository<Departure>,IDepartureRepository
    {
        public DepartureRepository(DataContext context,ILoggerFactory loggerFactory):
            base(context,loggerFactory,"DepartureRepository")
        {

        }
    }
}
