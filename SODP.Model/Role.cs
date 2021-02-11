﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SODP.Model
{
    public class Role : IdentityRole<int>
    {
        public Role() : base() { }

        public Role(string roleName) : base(roleName) { }
    }
}
