using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaspberryPIWebApp.Models
{
    public class Question
    {
        public int QuestionId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; } 
    }
}