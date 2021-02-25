using System;
using System.Collections.Generic;
using System.Text;

namespace SODP.Model
{
    public class Designer
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        public ICollection<Licence> Licences { get; set; }
        public ICollection<Certificate> Certificates { get; set; }
    }
}
