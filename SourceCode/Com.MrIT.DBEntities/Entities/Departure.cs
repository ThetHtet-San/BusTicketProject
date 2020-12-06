using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Com.MrIT.DBEntities.Entities
{
    [Table("departure")]
    public class Departure: GenericEntity
    {
        public string DepartureName { get; set; }
        public IComparable<Route> DepartureID { get; set; }
    }
}
