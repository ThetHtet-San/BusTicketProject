﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.MrIT.ViewModels
{
    public class VmStaff:ViewModelItemBase 
    {
        [Display(Name = "Emp Code")]
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

    }

   
}