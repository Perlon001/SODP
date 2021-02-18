using System;
using System.Collections.Generic;
using System.Text;

namespace SODP.Domain.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        public IList<string> Roles { get; set; }
    }
}
