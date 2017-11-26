using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RaspberryPIWebApp.Models;

namespace RaspberryPIWebApp.Controllers
{
    public class PiHardwaresController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PiHardwares
        public ActionResult Index()
        {
            var piHardwares = db.PiHardwares.Include(p => p.PiModel);
            return View(piHardwares.ToList());
        }

        // GET: PiHardwares/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PiHardware piHardware = db.PiHardwares.Find(id);
            if (piHardware == null)
            {
                return HttpNotFound();
            }
            return View(piHardware);
        }

        // GET: PiHardwares/Create
        public ActionResult Create()
        {
            ViewBag.PiModelId = new SelectList(db.PiModels, "PiModelId", "ModelName");
            return View();
        }

        // POST: PiHardwares/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PiHardwareId,Mac,Password,PiModelId")] PiHardware piHardware)
        {
            if (ModelState.IsValid)
            {
                db.PiHardwares.Add(piHardware);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PiModelId = new SelectList(db.PiModels, "PiModelId", "ModelName", piHardware.PiModelId);
            return View(piHardware);
        }

        // GET: PiHardwares/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PiHardware piHardware = db.PiHardwares.Find(id);
            if (piHardware == null)
            {
                return HttpNotFound();
            }
            ViewBag.PiModelId = new SelectList(db.PiModels, "PiModelId", "ModelName", piHardware.PiModelId);
            return View(piHardware);
        }

        // POST: PiHardwares/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PiHardwareId,Mac,Password,PiModelId")] PiHardware piHardware)
        {
            if (ModelState.IsValid)
            {
                db.Entry(piHardware).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PiModelId = new SelectList(db.PiModels, "PiModelId", "ModelName", piHardware.PiModelId);
            return View(piHardware);
        }

        // GET: PiHardwares/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PiHardware piHardware = db.PiHardwares.Find(id);
            if (piHardware == null)
            {
                return HttpNotFound();
            }
            return View(piHardware);
        }

        // POST: PiHardwares/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PiHardware piHardware = db.PiHardwares.Find(id);
            db.PiHardwares.Remove(piHardware);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
