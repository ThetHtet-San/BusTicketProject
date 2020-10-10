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
    public class EmailTemplateRepository : GenericRepository<EmailTemplate>, IEmailTemplateRepository
    {
        public EmailTemplateRepository(DataContext context, ILoggerFactory loggerFactory) :
       base(context, loggerFactory, "EmailTemplateRepository")
        {

        }

        public EmailTemplate GetByName(string name)
        {
            var record =  this.entities.Where(e => e.Active == true && e.Template == name).AsNoTracking().FirstOrDefault();

            return record;
        }
    }
}