using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com.MrIT.DBEntities
{
    [Table("email_template")]
    public class EmailTemplate : GenericEntity
    {
        public string Template { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }
}
