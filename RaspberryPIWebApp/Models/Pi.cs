using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaspberryPIWebApp.Models
{
    public class Pi
    {
        public int PiId { get; set; }
        public string Name { get; set; }
        public int PiModelId { get; set; }
        public int PiHardwareId { get; set; }
        public string ApplicationUserId { get; set; }
        public virtual PiModel PiModel { get; set; }
        public virtual PiHardware PiHardware { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public List<PiPin> PiPins { get; set; }
        public virtual List<Room> Rooms  { get; set; }
       
    }   
}