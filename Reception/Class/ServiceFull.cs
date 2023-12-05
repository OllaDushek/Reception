using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reception.Class
{
    public class ServiceFull
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public DateTime? DayStart { get; set; }
        public DateTime? DayOver { get; set; }
    }
}
