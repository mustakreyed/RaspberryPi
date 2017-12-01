using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaspberryPIWebApp.Models
{
    public class PiPin
    {
        public int PiPinId { get; set; }
        public string ApplienceName { get; set; }
        public string Location { get; set; }
        public string PinStatus { get; set; }
        public int PinNumber { get; set; }
        public int RoomId { get; set; } 
        public int PiId { get; set; }
        public virtual Pi Pi { get; set; }
        public virtual Room Room { get; set; }
      
    }
}