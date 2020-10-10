using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com.MrIT.DBEntities
{
    [Table("log_email")]
    public class EmailLog : GenericEntity
    {
        public string Receiver { get; set; }
        public string ReceiverCC { get; set; }
        public string Subject { get; set; }

        public string Body { get; set; }

        public string AttachmentFiles { get; set; }

        public bool IsSuccess { get; set; }
    }
}
