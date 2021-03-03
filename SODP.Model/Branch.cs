using System;
using System.Collections.Generic;
using System.Text;

namespace SODP.Model
{
    public class Branch
    {
        public int Id { get; set; }
        public string Sign { get; set; }
        public string Name { get; set; }
        public ICollection<Licence> Licences { get; set; }
    }
}
