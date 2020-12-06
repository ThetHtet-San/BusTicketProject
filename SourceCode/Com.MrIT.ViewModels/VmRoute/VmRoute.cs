using System;
using System.Collections.Generic;
using System.Text;

namespace Com.MrIT.ViewModels.VmRoute
{
   public class VmRoute:ViewModelItemBase
    {
        public int destination_ID { get; set; }
        public int departure_ID { get; set; }

        public VmDestination Destinations { get; set; }
        public VmDeparture Departures { get; set; }
    }
}
