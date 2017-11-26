using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Drawing;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using RaspberryPIWebApp.Models;

namespace RaspberryPIWebApp.Controllers
{
    [Authorize]
    public class UserDashboardController : Controller
    {
        ApplicationDbContext db=new ApplicationDbContext();
        public ActionResult Index()
        {
            string loginUser = User.Identity.GetUserId();
            var piList = db.Pis.Where(p => p.ApplicationUserId == loginUser).ToList();
            ViewBag.piList = piList;
            foreach (var pi in piList)
            {
                ViewBag.noOfDevice = db.PiPins.Count(p => p.PiId == pi.PiId);
                ViewBag.onDevice = db.PiPins.Count(p => p.PiId == pi.PiId &&p.PinStatus=="True");
                ViewBag.offDevice = db.PiPins.Count(p => p.PiId == pi.PiId && p.PinStatus == "False");
                var sensors = db.Sensors.FirstOrDefault(s => s.PiId == pi.PiId);
                if (sensors!=null)
                {
                    ViewBag.temp = sensors.Temparature;
                    ViewBag.light = sensors.Light;
                }
                else
                {
                    ViewBag.temp = "No Temparature Sensor";
                    ViewBag.light = "No Light Sensor";

                }
                
            }
            
            return View();
        }
       
        public ActionResult RoomList(int piId)
        {
            string loginUser = User.Identity.GetUserId();
            var piList=db.Pis.Where(p => p.ApplicationUserId == loginUser).ToList();
            var roomList = db.Rooms.Where(r => r.PiId == piId).ToList();
            ViewBag.piList = piList;
            ViewBag.roomList = roomList;
            var pi = db.Pis.FirstOrDefault(p => p.PiId == piId);
            string piName = pi.Name;
            ViewBag.piName = piName;
            return View();
        }
        
        public ActionResult DeviceList(int roomId,int piId)  
        {
            string loginUser = User.Identity.GetUserId();
            var piList = db.Pis.Where(p => p.ApplicationUserId == loginUser).ToList();
            var roomList = db.Rooms.Where(r => r.PiId == piId).ToList();
            ViewBag.piList = piList;
            ViewBag.roomList = roomList;
            var pi = db.Pis.FirstOrDefault(p => p.PiId == piId);
            ViewBag.piName = pi.Name;
            var deviceList = db.PiPins.Where(p => p.PiId == piId && p.RoomId == roomId).ToList();
            ViewBag.deviceList = deviceList;
            var room = db.Rooms.FirstOrDefault(r => r.RoomId == roomId);
            ViewBag.roomName = room.RoomName;
            return View();
        }
       
        public JsonResult ButtonOn(int piPinId)
        {
            var piPin = db.PiPins.FirstOrDefault(p => p.PiPinId == piPinId);
            piPin.PinStatus = "True";
            db.Entry(piPin).State = EntityState.Modified;
            int a=db.SaveChanges();
            return Json(a, JsonRequestBehavior.AllowGet);
        }
       
        public JsonResult ButtonOff(int piPinId)
        {
            var piPin = db.PiPins.FirstOrDefault(p => p.PiPinId == piPinId);
            piPin.PinStatus = "False";
            db.Entry(piPin).State = EntityState.Modified;
            int a = db.SaveChanges();
            return Json(a, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReconfigIndex()
        {
            string loginUser = User.Identity.GetUserId();
            var piList = db.Pis.Where(p => p.ApplicationUserId == loginUser).ToList();
            ViewBag.piList = piList;
            return View();
        }
        [HttpGet]
        public ActionResult ReconfigDeviceNameandRoom(int piId)
        {
            TempData["piId"] = piId;
            string loginUser = User.Identity.GetUserId();
            var piInfo = db.Pis
                .Include(p=>p.Rooms)
                .FirstOrDefault(p => p.ApplicationUserId == loginUser);
          return View(piInfo);
        }
        [HttpPost]
        public ActionResult ReconfigDeviceNameandRoom(string PiName,string[] RoomId, string[] RoomName)
        {
            int piId=Convert.ToInt32(TempData["piId"]);
            var pi = db.Pis.FirstOrDefault(p => p.PiId == piId);
            //Save Pi............
            pi.Name = PiName;
            db.Entry(pi).State = EntityState.Modified;
            db.SaveChanges();

            //Update or Save New Rooms............
            for (int i = 0; i < RoomName.Length; i++)
            {
                
                if (RoomId.Length<=i)
                {
                    //Save if new 
                    Room room = new Room();
                    room.RoomName = RoomName[i];
                    room.PiId = pi.PiId;
                    db.Rooms.Add(room);
                    db.SaveChanges();
                }
                else
                {
                    // Upadte if existed
                    int id = Convert.ToInt32(RoomId[i]);
                    var room = db.Rooms.FirstOrDefault(r => r.RoomId == id);
                    room.RoomName = RoomName[i];
                    db.Entry(room).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }

            return RedirectToAction("ReconfigRoom", new { piId = pi.PiId });
        }
        [HttpGet]
        public ActionResult ReconfigRoom(int piId)
        {
            TempData["piId"] = piId;
            ViewBag.piId = piId;
            var roomList = db.Rooms.Where(r => r.PiId == piId).ToList();
            ViewBag.roomList = roomList;
            List<PiPin> piPinList = db.PiPins.Where(p => p.PiId == piId).ToList();
            return View(piPinList);
        }
        [HttpPost]
        public ActionResult ReconfigRoom(int[] PiPinId, int[] PinNumber, string[] ApplienceName, string[] Location, int[] RoomId)
        {
            int piId = Convert.ToInt32(TempData["piId"]);
            if (PiPinId != null)
            {
                //Update or Save New Rooms............
                for (int i = 0; i < PinNumber.Length; i++)
                {
                    int len = PiPinId.Length;

                    if (len <= i)
                    {
                        SaveNewPinData(PinNumber, ApplienceName, Location, RoomId, i, piId);
                    }
                    else
                    {
                        UpdateExistingPinData(PiPinId, PinNumber, ApplienceName, Location, RoomId, i);
                    }
                }
            }
            else
            {
                // Save New Rooms............
                for (int i = 0; i < PinNumber.Length; i++)
                {
                    SaveNewPinData(PinNumber, ApplienceName, Location, RoomId, i, piId);
                }
            }
           
            return RedirectToAction("Index");
        }


        

        //Save if new 
        private void SaveNewPinData(int[] PinNumber, string[] ApplienceName, string[] Location, int[] RoomId, int i, int piId)
        {
            PiPin piPin = new PiPin();
            piPin.PinNumber = PinNumber[i];
            piPin.PinStatus = "False";
            piPin.ApplienceName = ApplienceName[i];
            piPin.Location = Location[i];
            piPin.RoomId = RoomId[i];
            piPin.PiId = piId;
            db.PiPins.Add(piPin);
            db.SaveChanges();
        }

        // Upadte if existed
        private void UpdateExistingPinData(int[] PiPinId, int[] PinNumber, string[] ApplienceName, string[] Location,
            int[] RoomId, int i)
        {
            int piPinId = Convert.ToInt32(PiPinId[i]);
            var piPin = db.PiPins.FirstOrDefault(r => r.PiPinId == piPinId);
            piPin.PinNumber = PinNumber[i];
            piPin.ApplienceName = ApplienceName[i];
            piPin.Location = Location[i];
            piPin.RoomId = RoomId[i];
            db.Entry(piPin).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}