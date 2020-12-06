using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Com.MrIT.DBEntities.Entities
{
    [Table("route")]
    public class Route : GenericEntity
    {
        public int destination_ID { get; set; }
        public int departure_ID { get; set; }

       [ ForeignKey("destination_ID")]
       public virtual Destination Destination { get; set; }

        [ForeignKey("departure_ID")]
        public virtual Departure Departure { get; set; }
    }
}
