using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaspberryPIWebApp.Models
{
    public class PiHardware
    {
        public int PiHardwareId { get; set; }   
        public string Mac  { get; set; }
        public string Password { get; set; }
        public int PiModelId { get; set; }
        public virtual PiModel PiModel { get; set; }    
       

    }
}