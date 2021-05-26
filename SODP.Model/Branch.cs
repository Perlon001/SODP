using SODP.Model.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SODP.Model
{
    public class Branch
    {
        public int Id { get; set; }
        public string Sign { get; set; }
        public string Title { get; set; }

        public void Normalize()
        {
            Sign = Sign.ToUpper();
            Title = Title.CapitalizeFirstLetter();
        }
    }
}
