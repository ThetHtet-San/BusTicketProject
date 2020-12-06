﻿using Com.MrIT.Common;
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
    public class BusTypeRepository : GenericRepository<BusType>, IBusTypeRepository
    { 
        public BusTypeRepository(DataContext context, ILoggerFactory loggerFactory) :
       base(context, loggerFactory, "BusTypeRepository")
        {

        }
    
    }
}
