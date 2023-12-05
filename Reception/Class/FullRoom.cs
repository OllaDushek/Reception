using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reception.Class
{
    public class FullRoom
    {
        public int ID { get; set; }
        public byte[] Photo { get; set; }
        public int NumberHuman { get; set; }
        public string ClassRoom { get; set; }
        public decimal Cost { get; set; }
        public string StatusRoom { get; set; }
    }
}
