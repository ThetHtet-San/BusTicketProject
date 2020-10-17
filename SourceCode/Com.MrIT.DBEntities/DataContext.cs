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

        //public virtual DbSet<Sample> Samples { get; set; }

        public virtual DbSet<Staff> Staffs { get; set; }

        public virtual DbSet<StaffEducation> StaffEducations { get; set; }

        public virtual DbSet<StaffExperience> StaffExperiences { get; set; }
    }
}
