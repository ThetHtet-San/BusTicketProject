using Com.MrIT.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.MrIT.ViewModels
{
    public class VmBus:ViewModelItemBase 
    {
        public string BusNo { get; set; }
        public int SeatAvailable { get; set; }
        public List<VmBusType> busTypes { get; set; }

        public List<VmSeat> seats { get; set; }
    }

    public class VmBusPage
    {
        public PageResult<VmBus> Result { get; set; }
    }
}

