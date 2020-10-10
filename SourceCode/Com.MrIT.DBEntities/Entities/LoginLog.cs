using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Com.MrIT.DBEntities
{
    [Table("log_login")]
    public class LoginLog : GenericEntity
    {

        public string Username { get; set; }

        public string Browser { get; set; }
        public string IPAddress { get; set; }
        public string MachineIPAddress { get; set; }
    }
}
