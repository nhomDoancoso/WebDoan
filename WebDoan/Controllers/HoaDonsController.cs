using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebDoan.Models;

namespace WebDoan.Controllers
{
    public class HoaDonsController : Controller
    {
        // GET: HoaDons
        private myDataContextDataContext db = new myDataContextDataContext();
        // GET: HoaDons
        public ActionResult Index()
        {
            var hoaDon = db.HOADONs.Include(h => h.KHACHHANG).Include(h => h.NHANVIEN);
            return View(hoaDon.ToList());
        }

        // GET: HoaDons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HOADON hoaDon = db.HOADONs.FirstOrDefault(x => x.MaNV == id);
         //   HOADON hoaDon = db.HOADONs.Find(id);
            if (hoaDon == null)
            {
                return HttpNotFound();
            }
            return View(hoaDon);
        }

        // GET: HoaDons/Create
        public ActionResult Create()
        {
            ViewBag.MaKH = new SelectList(db.KHACHHANGs, "MaKH", "TenKH");
            ViewBag.MaNV = new SelectList(db.NHANVIENs, "MaNV", "TenNV");
            return View();
        }

        // POST: HoaDons/Create
        // more details see https://go.mirosoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaHD,MaKH,MaNV,NgayLapHoaDon")] HOADON hoaDon)
        {
            if (ModelState.IsValid)
            {
                db.HOADONs.InsertOnSubmit(hoaDon);
                db.SubmitChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaKH = new SelectList(db.KHACHHANGs, "MaKH", "TenKH", hoaDon.MaKH);
            ViewBag.MaNV = new SelectList(db.NHANVIENs, "MaNV", "TenNV", hoaDon.MaNV);
            return View(hoaDon);
        }

        // GET: HoaDons/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HOADON hoaDon = db.HOADONs.FirstOrDefault(x => x.MaNV == id);

            // HOADON hoaDon = db.HOADONs.Find(id);
            if (hoaDon == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaKH = new SelectList(db.KHACHHANGs, "MaKH", "TenKH", hoaDon.MaKH);
            ViewBag.MaNV = new SelectList(db.NHANVIENs, "MaNV", "TenNV", hoaDon.MaNV);
            return View(hoaDon);
        }

        // POST: HoaDons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaHD,MaKH,MaNV,NgayLapHoaDon")] HOADON hoaDon)
        {
            //if (ModelState.IsValid)
            //{
            //    db.Entry(hoaDon).State = EntityState.Modified;
            //    db.SubmitChanges();
            //    return RedirectToAction("Index");
            //}
            //ViewBag.MaKH = new SelectList(db.KHACHHANGs, "MaKH", "TenKH", hoaDon.MaKH);
            //ViewBag.MaNV = new SelectList(db.NHANVIENs, "MaNV", "TenNV", hoaDon.MaNV);
            //return View(hoaDon);
            var existingHoaDon = db.HOADONs.SingleOrDefault(hd => hd.MaHD == hoaDon.MaHD);

            if (existingHoaDon == null)
            {
                return HttpNotFound();
            }

            existingHoaDon.MaKH = hoaDon.MaKH;
            existingHoaDon.MaNV = hoaDon.MaNV;
            existingHoaDon.NgayLap = hoaDon.NgayLap;

            db.SubmitChanges();
            return RedirectToAction("Index");
        }

        // GET: HoaDons/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //HOADON hoaDon = db.HOADONs.Find(id);
            HOADON hoaDon = db.HOADONs.FirstOrDefault(x => x.MaNV == id);
            if (hoaDon == null)
            {
                return HttpNotFound();
            }
            return View(hoaDon);
        }

        // POST: HoaDons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //HOADON hoaDon = db.HOADONs.Find(id);
            HOADON hoaDon = db.HOADONs.FirstOrDefault(x => x.MaNV == id);
            db.HOADONs.DeleteOnSubmit(hoaDon);
            db.SubmitChanges();
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