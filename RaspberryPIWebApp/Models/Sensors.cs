using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaspberryPIWebApp.Models
{
    public class Sensors
    {
        public int SensorsId { get; set; }
        public string Temparature { get; set; }
        public string Light { get; set; }
        public string Water { get; set; }
        public int PiId { get; set; }
        public virtual Pi Pi { get; set; }
    }
}