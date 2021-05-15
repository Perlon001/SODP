using System;
using System.Collections.Generic;
using System.Text;

namespace SODP.Domain.DTO
{
    public class UserDTO : BaseDTO
    {
        public string UserName { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public IList<string> Roles { get; set; }
    }
}
