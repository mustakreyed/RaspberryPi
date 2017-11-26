using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RaspberryPIWebApp.Models;
using Microsoft.Owin.Security;

namespace RaspberryPIWebApp.Controllers
{
    [AllowAnonymous]
   
    public class HomeController : Controller
    {
        ApplicationDbContext db=new ApplicationDbContext();
       
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Question(Question question)
        {
            if (ModelState.IsValid)
            {
                db.Questions.Add(question);
                int result=db.SaveChanges();
                TempData["result"] = result;
                return RedirectToAction("Index");
            }
           return View(question);
        }
        [HttpPost]
        public string VerifyRpiMac(string mac, string password)
        {
            var rpi = db.PiHardwares.FirstOrDefault(m => m.Mac == mac && m.Password == password);

            if (rpi != null)
            {
                TempData["PiHardwareId"] = rpi.PiHardwareId;
                return "success";
            }
            else
            {
                return "fail";
            }
        }


    }
}