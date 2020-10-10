using Com.MrIT.Common.FoundationClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.MrIT.Common.Configuration
{
    public class AppSettings : BaseSettings
    {
        public int TotalRecordPerPage
        {
            get;
            set;
        }

        public int FirstPage
        {
            get;
            set;
        }
    }
}
