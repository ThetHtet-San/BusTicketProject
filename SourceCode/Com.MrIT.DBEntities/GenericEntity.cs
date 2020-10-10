using System;

namespace Com.MrIT.DBEntities
{
    public class GenericEntity
    {
        //[Key]
        public int ID { get; set; }

        public bool Active { get; set; }

        public bool SystemActive { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }


        public GenericEntity()
        { }
    }
}
