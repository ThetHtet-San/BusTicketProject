using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.MrIT.ViewModels
{
    public class VmStaffExperience:ViewModelItemBase 
    {
        public int StaffID { get; set; }
        public string Position { get; set; }
        public string Company { get; set; }
        public int FromYear { get; set; }
        public int ToYear { get; set; }

        public VmStaff Staff { get; set; }
    }

   
}
