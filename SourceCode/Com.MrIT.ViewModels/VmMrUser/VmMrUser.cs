using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.MrIT.ViewModels
{
    public class VmMrUser : ViewModelItemBase
    {
        [Required]
        [Display(Name = "Full Name")]
        [MaxLength(50, ErrorMessage = "This field must be less than 50 characters")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Email")]
        [MaxLength(50, ErrorMessage = "This field must be less than 50 characters")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [MaxLength(50, ErrorMessage = "This field must be less than 50 characters")]
        public string Password { get; set; }

        [Display(Name = "Mobile No.")]
        [MaxLength(500, ErrorMessage = "This field must be less than 500 characters")]
        public string MobileNo { get; set; }

        [Display(Name = "User Type")]
        [MaxLength(10, ErrorMessage = "This field must be less than 10 characters")]
        public string UserType { get; set; }

        [Display(Name = "User Role")]
        [MaxLength(50, ErrorMessage = "This field must be less than 50 characters")]
        public string UserRole { get; set; }

        [Display(Name = "Address")]
        [MaxLength(1000, ErrorMessage = "This field must be less than 1000 characters")]
        public string Address { get; set; }

        [Display(Name = "City")]
        [MaxLength(500, ErrorMessage = "This field must be less than 500 characters")]
        public string City { get; set; }

        [Display(Name = "State")]
        [MaxLength(500, ErrorMessage = "This field must be less than 500 characters")]
        public string State { get; set; }

        [Display(Name = "Country")]
        [MaxLength(500, ErrorMessage = "This field must be less than 500 characters")]
        public string Country { get; set; }

        [Display(Name = "Postal Code")]
        [MaxLength(10, ErrorMessage = "This field must be less than 10 characters")]
        public string PostalCode { get; set; }

        [Display(Name = "Profile Image")]
        public byte[] ProfileImage { get; set; }

        public string strProfileImage { get; set; }

        [Display(Name = "Is Locked")]
        public bool IsLocked { get; set; }

        [Display(Name = "Is Activated")]
        public bool IsActivated { get; set; }

        [Display(Name = "Disable Reason")]
        public string DisableReason { get; set; }
        

        // User Info
        public string Browser { get; set; }
        public string IPAddress { get; set; }
        public string MachineIPAddress { get; set; }

        public bool isFacebookLogin { get; set; }
    }
}
