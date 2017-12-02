﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using RaspberryPIWebApp.Models;
namespace RaspberryPIWebApp.Controllers
{
    [Authorize]
    public class PiConfigController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
       
        [HttpGet]
        public ActionResult GetRoomNames()
        {
            //int piHardwareId = Convert.ToInt32(TempData["PiHardwareId"]);
            //var hardware = db.PiHardwares.FirstOrDefault(m => m.PiHardwareId == piHardwareId);
            //ViewBag.piHardware = hardware;
            //var userid = User.Identity.GetUserId();
            //var user = db.Users.FirstOrDefault(m => m.Id == userid);
            //ViewBag.user = user;
            return View();
        }

        [HttpPost]
        public ActionResult GetRoomNames(string PiName, string[] RoomName)
        {
            int piHardwareId = Convert.ToInt32(TempData["PiHardwareId"]);
            var hardware = db.PiHardwares.FirstOrDefault(m => m.PiHardwareId == piHardwareId);
            var userid = User.Identity.GetUserId();
            //Save Pi............
            Pi pi = new Pi();
            pi.ApplicationUserId = userid;
            pi.Name = PiName;
            pi.PiHardwareId = piHardwareId;
            pi.PiModelId = hardware.PiModelId;
            db.Pis.Add(pi);
            db.SaveChanges();
           
            //Save Rooms............
            foreach (var aroom in RoomName)
            {
                Room room = new Room();
                room.RoomName = aroom;
                room.PiId = pi.PiId;
                db.Rooms.Add(room);
                db.SaveChanges();
            }
            return RedirectToAction("PinConfig",new {piId=pi.PiId});
        }

        [HttpGet]
        public ActionResult PinConfig(int? piId)
        {
            TempData["piIdpost"] = piId;
            var roomList = db.Rooms.Where(r => r.PiId == piId).ToList();
            ViewBag.roomList = roomList;
            return View();
        }
        [HttpPost]
        public ActionResult PinConfig(int[] PinNumber,string[] ApplienceName, string[] Location, int[] RoomId)
        {
            var piId = Convert.ToInt32(TempData["piIdpost"]);
            for (int i = 0; i < PinNumber.Length; i++)
            {
               PiPin piPin=new PiPin();
                piPin.PinNumber = PinNumber[i];
                piPin.PinStatus = "False";
                piPin.ApplienceName = ApplienceName[i];
                piPin.Location = Location[i];
                piPin.RoomId = RoomId[i];
                piPin.PiId = piId;
                db.PiPins.Add(piPin);
                db.SaveChanges();

            }
           
            return RedirectToAction("Index","Home");
        }
  
        public ActionResult SensorConfig()  
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult SensorConfig(int waterControledPin,int lightControledPin,int temparatureControledPin)
        {
            string loginUser = User.Identity.GetUserId();
            var pi = db.Pis.FirstOrDefault(p => p.ApplicationUserId == loginUser);
            var sensor=db.Sensors.FirstOrDefault(s=>s.PiId==pi.PiId);

            if (sensor != null)
            {
                sensor.WaterControledPin = waterControledPin;
                sensor.LightControledPin = lightControledPin;
                sensor.TemparatureControledPin = temparatureControledPin;
                db.Entry(sensor).State=EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("Index", "UserDashboard");
        }

    }
}