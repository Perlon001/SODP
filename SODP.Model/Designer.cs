using System;
using System.Collections.Generic;
using System.Text;

namespace SODP.Model
{
    public class Designer
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public ICollection<Licence> Licences { get; set; }
        public ICollection<Certificate> Certificates { get; set; }
    }
}
