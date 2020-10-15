using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.MrIT.ViewModels
{
    public class VmStaffEducation:ViewModelItemBase 
    {
        public string Type { get; set; }
        public string Description { get; set; }
        public int Year { get; set; }

        public int StaffID { get; set; }

       
        public VmStaff Staff { get; set; }

    }

   
}
