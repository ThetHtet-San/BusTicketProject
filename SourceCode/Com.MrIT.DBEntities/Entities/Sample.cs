using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Com.MrIT.DBEntities
{
    [Table("tablename")]
    public class Sample : GenericEntity
    {
        public string TableID { get; set; }
        public string Action { get; set; }
        public string TableName { get; set; }
        public string FieldName { get; set; }
        public string FieldValue { get; set; }

    }
}
