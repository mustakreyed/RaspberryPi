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
    public class PiPinforModelsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PiPinforModels
        public ActionResult Index()
        {
            var piPinforModels = db.PiPinforModels.Include(p => p.PiModel);
            return View(piPinforModels.ToList());
        }

        // GET: PiPinforModels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PiPinforModel piPinforModel = db.PiPinforModels.Find(id);
            if (piPinforModel == null)
            {
                return HttpNotFound();
            }
            return View(piPinforModel);
        }

        // GET: PiPinforModels/Create
        public ActionResult Create()
        {
            ViewBag.PiModelId = new SelectList(db.PiModels, "PiModelId", "ModelName");
            return View();
        }

        // POST: PiPinforModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PiPinforModelId,PiPinNumber,Description,PiModelId")] PiPinforModel piPinforModel)
        {
            if (ModelState.IsValid)
            {
                db.PiPinforModels.Add(piPinforModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PiModelId = new SelectList(db.PiModels, "PiModelId", "ModelName", piPinforModel.PiModelId);
            return View(piPinforModel);
        }

        // GET: PiPinforModels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PiPinforModel piPinforModel = db.PiPinforModels.Find(id);
            if (piPinforModel == null)
            {
                return HttpNotFound();
            }
            ViewBag.PiModelId = new SelectList(db.PiModels, "PiModelId", "ModelName", piPinforModel.PiModelId);
            return View(piPinforModel);
        }

        // POST: PiPinforModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PiPinforModelId,PiPinNumber,Description,PiModelId")] PiPinforModel piPinforModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(piPinforModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PiModelId = new SelectList(db.PiModels, "PiModelId", "ModelName", piPinforModel.PiModelId);
            return View(piPinforModel);
        }

        // GET: PiPinforModels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PiPinforModel piPinforModel = db.PiPinforModels.Find(id);
            if (piPinforModel == null)
            {
                return HttpNotFound();
            }
            return View(piPinforModel);
        }

        // POST: PiPinforModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PiPinforModel piPinforModel = db.PiPinforModels.Find(id);
            db.PiPinforModels.Remove(piPinforModel);
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
