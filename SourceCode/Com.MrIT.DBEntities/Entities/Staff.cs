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
        public string StaffID { get; set; }
        public string StaffName { get; set; }
        public string StaffPhone { get; set; }
        public string StaffAddress { get; set; }
        public string StaffGender { get; set; }



        public ICollection<StaffEducation> Educations { get; set; }

        public ICollection<StaffExperience> Experiences { get; set; }

    }
}
