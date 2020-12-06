using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Com.MrIT.DBEntities.Entities
{
    [Table("destination")]
    public class Destination : GenericEntity
    {
        public string DestinationName { get; set; }

        public ICollection<Route> DestinationID { get; set; }
    }
}
