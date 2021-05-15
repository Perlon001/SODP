using SODP.Model.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SODP.Model
{
    public class Designer : BaseEntity
    {
        public string Title { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public bool Active { get; set; }

        public override string ToString()
        {
            return Firstname.Trim() + " " + Lastname.Trim();
        }

        public void Normalize()
        {
            Firstname = Firstname.CapitalizeFirstLetter();
            Lastname = Lastname.CapitalizeFirstLetter();
        }
    }
}
