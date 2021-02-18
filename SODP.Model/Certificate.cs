using System;
using System.Collections.Generic;
using System.Text;

namespace SODP.Model
{
    public class Certificate
    {
        public int Id { get; set; }
        public int DesignerId { get; set; }
        public virtual Designer Designer { get; set; }
        public string Number { get; set; }
        public DateTime StartignDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
