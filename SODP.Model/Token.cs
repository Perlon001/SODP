using System;
using System.Collections.Generic;
using System.Text;

namespace SODP.Model
{
    public class Token
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public string Access { get; set; }
        public string RefreshTokenKey { get; set; }
        public string Refresh { get; set; }
    }
}
