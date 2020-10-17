using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Com.MrIT.DBEntities
{
    [Table("staff_experience")]
    public class StaffExperience : GenericEntity
    {
        public int StaffID { get; set; }
        public string Position { get; set; }
        public string Company { get; set; }
        public int FromYear { get; set; }
        public int ToYear { get; set; }

        [ForeignKey("StaffID")]
        public virtual Staff Staff { get; set; }

    }
}
