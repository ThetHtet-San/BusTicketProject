using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com.MrIT.DBEntities
{
    [Table("log_error")]
    public class ErrorLog : GenericEntity
    {
        public string Error { get; set; }
        public string StackTrace { get; set; }
        public string Message { get; set; }

        public string URL { get; set; }

    }
}
