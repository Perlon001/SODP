using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SODP.Model
{
    public class User : IdentityUser<int>
    {
        public User() : base() { }
        public User(string userName) : base(userName) { }

        public string Forename { get; set; }
        public string Surname { get; set; }

        public override string ToString()
        {
            return Forename.ToString().Trim() + " " + Surname.ToString().Trim();
        }
    }
}
