using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Com.MrIT.DBEntities
{
    [Table("bus")]
    public class Bus : GenericEntity
    {
        public int BusTypeID { get; set; }
        public string BusNo { get; set; }
        public int SeatAvailable { get; set; }
        [ForeignKey("BusTypeID")]
        public virtual BusType BusType { get; set; }
        public ICollection<Seat> seats { get; set; }

    }


}
