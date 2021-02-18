using System;
using System.Collections.Generic;
using System.Text;

namespace SODP.Domain.Model
{
    public class PageInfo
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages => (int)Math.Ceiling(decimal.Divide(TotalItems,ItemsPerPage));

        public string Url { get; set; }
    }
}
