using System;
using System.Collections.Generic;
using System.Text;

namespace Com.MrIT.ViewModels.VmRoute
{
    public class VmDeparture:ViewModelItemBase
    {
        public string DepartureName { get; set; }

        public List<VmRoute> DepartureID { get; set; }

    }
}
