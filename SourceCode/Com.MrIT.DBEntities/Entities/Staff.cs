using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Com.MrIT.DBEntities
{
    [Table("staff")]
    public class Staff : GenericEntity
    {
        public string Code1 { get; set; }
        public string Code2 { get; set; }


        public string Name { get; set; }
        public string Address { get; set; }



        public ICollection<StaffEducation> Educations { get; set; }

        public ICollection<StaffExperience> Experiences { get; set; }

    }
}
