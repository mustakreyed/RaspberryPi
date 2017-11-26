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
    public class PiModelsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PiModels
        public ActionResult Index()
        {
            return View(db.PiModels.ToList());
        }

        // GET: PiModels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PiModel piModel = db.PiModels.Find(id);
            if (piModel == null)
            {
                return HttpNotFound();
            }
            return View(piModel);
        }

        // GET: PiModels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PiModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PiModelId,ModelName,NumberofPin")] PiModel piModel)
        {
            if (ModelState.IsValid)
            {
                db.PiModels.Add(piModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(piModel);
        }

        // GET: PiModels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PiModel piModel = db.PiModels.Find(id);
            if (piModel == null)
            {
                return HttpNotFound();
            }
            return View(piModel);
        }

        // POST: PiModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PiModelId,ModelName,NumberofPin")] PiModel piModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(piModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(piModel);
        }

        // GET: PiModels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PiModel piModel = db.PiModels.Find(id);
            if (piModel == null)
            {
                return HttpNotFound();
            }
            return View(piModel);
        }

        // POST: PiModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PiModel piModel = db.PiModels.Find(id);
            db.PiModels.Remove(piModel);
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
