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
            return View();
        }

        public ActionResult Index1()
        {
            ViewBag.TenDV = new SelectList(db.DICHVUs, "MaDV", "TenDV");
            return View();
        }
    }
}