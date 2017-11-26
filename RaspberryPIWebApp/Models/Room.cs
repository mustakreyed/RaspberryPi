using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaspberryPIWebApp.Models
{
    public class Room
    {
        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public int PiId { get; set; }
        public virtual Pi Pi { get; set; }  
        public virtual List<PiPin> PiPins  { get; set; }
    }
}