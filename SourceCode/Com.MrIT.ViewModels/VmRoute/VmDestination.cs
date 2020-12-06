using System;
using System.Collections.Generic;
using System.Text;

namespace Com.MrIT.ViewModels.VmRoute
{
   public class VmDestination:ViewModelItemBase
    {
        public string DestinationName { get; set; }

        public List<VmRoute> DestinationID { get; set; }
    }
}
