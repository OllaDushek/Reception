using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reception.Class
{
    public class RoomForCheckIn
    {
        public int ID { get; set; }
        public string Class { get; set; }
        public int Num { get; set; }
        public DateTime DateCheckIn { get; set; }
        public DateTime DateCheckOut { get; set; }
    }
}
