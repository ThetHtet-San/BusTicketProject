using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com.MrIT.DBEntities
{
    [Table("mruser")]
    public class MrUser : GenericEntity
    {
        public MrUser()
        {

        }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
        public string UserRole { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public byte[] ProfileImage { get; set; }
        public bool IsLocked { get; set; }
        public bool IsActivated { get; set; }
        public string DisableReason { get; set; }
    }
}
