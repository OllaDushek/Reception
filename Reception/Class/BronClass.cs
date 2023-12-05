using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reception.Class
{
    public class BronClass
    {
        public int ID { get; set; }
        public DateTime DayStart { get; set; }
        public DateTime DayOver { get; set; }
        public int RoomID { get; set; }
        public int NumberHuman { get; set; }
        public string ClassRoom { get; set; }
        public decimal Sum { get; set; }
    }
}
