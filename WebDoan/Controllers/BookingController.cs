using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDoan.Models;

namespace WebDoan.Controllers
{
    public class BookingController : Controller
    {
        // GET: Booking
        myDataContextDataContext db = new myDataContextDataContext();

        public ActionResult Index()
        {
            ViewBag.TenNV = new SelectList(db.NHANVIENs, "MaNV", "TenNV");
            ViewBag.TenDV = new SelectList(db.DICHVUs, "MaDV", "TenDV");
            ViewBag.MaCN = new SelectList(db.CHINHANHs, "MaCN", "DiaChi");
            return View();
        }
        [HttpPost]
        public ActionResult Index(FormCollection collection, PHIEUDAT pd )
        {

            var MaPD = collection["MaPD"];
            var MAKH = collection["MAKH"];
            var MaNV = collection["MaNV"];
            var TimeLap = collection["TimeLap"];
            var TimeHen = collection["TimeHen"];
            var checkPD = db.PHIEUDATs.FirstOrDefault(x => x.MaPD.ToString() == MaPD);
            db.PHIEUDATs.InsertOnSubmit(pd);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Index1()
        {
            ViewBag.TenDV = new SelectList(db.DICHVUs, "MaDV", "TenDV");
            return View();
        }

        public ActionResult Index2()
        {
            ViewBag.TenNV = new SelectList(db.NHANVIENs, "MaNV", "TenNV");
            ViewBag.MaCN = new SelectList(db.CHINHANHs, "MaCN", "DiaChi");
            return View();
        }

        public ActionResult NgayGio()
        {
            return View();
        }
    }
}