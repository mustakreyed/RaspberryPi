using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaspberryPIWebApp.Models
{
    public class PiModel
    {
        public int PiModelId { get; set; }
        public string ModelName { get; set; }
        public int NumberofPin { get; set; }
        public virtual List<PiPinforModel> PiPinforModels { get; set; }
        public virtual List<PiHardware> PiHardwares { get; set; }
    }
}