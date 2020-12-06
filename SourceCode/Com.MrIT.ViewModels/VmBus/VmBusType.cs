using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.MrIT.ViewModels
{
    public class VmBusType:ViewModelItemBase 
    {
        public string BusTypeName { get; set; }

        public VmBus Bus { get; set; }

    }

   
}
