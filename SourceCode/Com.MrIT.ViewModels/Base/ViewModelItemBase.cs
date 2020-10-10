using System;
using System.ComponentModel.DataAnnotations;

namespace Com.MrIT.ViewModels
{
    public class ViewModelItemBase
    {
        public int ID
        {
            get;
            set;
        }

        public string EncryptId
        {
            get;
            set;
        }

        [Display(Name = "Created On")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MMM.yyyy HH:mm:ss}")]
        public DateTime CreatedOn
        {
            get;
            set;
        }

        [Display(Name = "Modified On")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MMM.yyyy HH:mm:ss}")]
        public DateTime ModifiedOn
        {
            get;
            set;
        }

        [Display(Name = "Created By")]
        public string CreatedBy
        {
            get;
            set;
        }

        [Display(Name = "Modified By")]
        public string ModifiedBy
        {
            get;
            set;
        }


        [Display(Name = "Enable")]
        public bool Active
        {
            get;
            set;
        }

       
        public bool SystemActive
        {
            get;
            set;
        }




    }
}
