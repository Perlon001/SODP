using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SODP.Model
{
    public class RoleClaim : IdentityRoleClaim<int>
    {
        public RoleClaim() : base() { }
    }
}
