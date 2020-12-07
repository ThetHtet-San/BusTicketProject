using Com.MrIT.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Com.MrIT.ViewModels
{
    public class VmBus:ViewModelItemBase 
    {
        public int BusTypeID { get; set; }
        public string BusNo { get; set; }
        public int SeatAvailable { get; set; }

        public VmBusType BusType { get; set; }
        public List<VmSeat> seats { get; set; }
    }

}

