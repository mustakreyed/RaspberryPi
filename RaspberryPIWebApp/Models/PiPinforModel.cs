using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaspberryPIWebApp.Models
{
    public class PiPinforModel
    {
        public int PiPinforModelId { get; set; }
        public int PiPinNumber { get; set; }
        public string Description { get; set; }
        public int PiModelId { get; set; }
        public virtual PiModel PiModel { get; set; }
    }
}