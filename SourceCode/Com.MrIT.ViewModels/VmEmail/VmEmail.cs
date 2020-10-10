using System;
using System.Collections.Generic;
using System.Text;

namespace Com.MrIT.ViewModels
{
    public class VmRegistrationEmail
    {

        public string Email { get; set; }

        public string Name { get; set; }

        public string URL { get; set; }
        
    }

    public class VmEnrollmentEmail
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string CourseName { get; set; }

        public int InstituteID { get; set; }
        public string InstituteName { get; set; }

        public string EnrollmentCode { get; set; }

        public string EnrollmentStatus { get; set; }

        public string URL { get; set; }
    }

    public class VmContactUsEmail
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
