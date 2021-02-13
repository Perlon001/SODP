using System;
using System.Collections.Generic;
using System.Text;

namespace SODP.Model
{
    public class PageInfo
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPage => (int)Math.Ceiling((decimal)TotalItems/ItemsPerPage);

        public string Url { get; set; }
    }
}
