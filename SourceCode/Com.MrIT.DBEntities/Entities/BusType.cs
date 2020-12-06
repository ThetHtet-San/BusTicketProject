using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Com.MrIT.DBEntities
{
    [Table("bus_type")]
    public class BusType : GenericEntity
    {
        public string BusTypeName { get; set; }
        [ForeignKey("BusID")]
        public virtual Bus Bus { get; set; }
    }
    
}
