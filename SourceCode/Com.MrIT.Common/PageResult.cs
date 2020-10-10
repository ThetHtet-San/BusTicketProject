using System;
using System.Collections.Generic;
using System.Text;

namespace Com.MrIT.Common
{
    public class PageResult<T> where T : class
    {
        public int TotalRecords { get; set; }

        public int TotalPage { get; set; }

        public int CurrentPage { get; set; }

        public int PreviousPage { get; set; }

        public int NextPage { get; set; }

        public List<T> Records { get; set; }
    }

    public class Record<T> where T : class
    {
        public List<T> Records { get; set; }
    }
}
