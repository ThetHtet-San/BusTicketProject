using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Com.MrIT.DBEntities
{
    [Table("staff_education")]
    public class StaffEducation : GenericEntity
    {

        public string Type { get; set; }
        public string Description { get; set; }
        public int Year { get; set; }

        public int StaffID { get; set; }

        [ForeignKey("StaffID")]
        public virtual Staff Staff { get; set; }

    }
}
