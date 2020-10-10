using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.MrIT.DBEntities
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }

        public virtual DbSet<LoginLog> LoginLogs { get; set; }

        public virtual DbSet<ChangeLog> ChangeLogs { get; set; }

        public virtual DbSet<EmailTemplate> EmailTemplates { get; set; }

        public virtual DbSet<EmailLog> EmailLogs { get; set; }

        public virtual DbSet<ErrorLog> ErrorLogs { get; set; }


        public virtual DbSet<MrUser> MrUsers { get; set; }
    }
}
