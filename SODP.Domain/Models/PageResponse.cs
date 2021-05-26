using System;
using System.Collections.Generic;
using System.Text;

namespace SODP.Domain.Models
{
    public class PageResponse<T>
    {
        public ICollection<T> Collection { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages => TotalCount % PageSize;
    }
}
