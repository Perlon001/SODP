using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SODP.Model
{
    public class UserToken : IdentityUserToken<int>
    {
        public UserToken() : base() { }
    }
}
