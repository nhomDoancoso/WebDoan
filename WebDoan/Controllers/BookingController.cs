using Microsoft.Ajax.Utilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDoan.Models;
using System.Data.SqlClient;

namespace WebDoan.Controllers
{
    public class BookingController : Controller
    {
        // GET: Booking
        myDataContextDataContext db = new myDataContextDataContext();

        public ActionResult Index()
        {
            ViewBag.MaNV = new SelectList(db.NHANVIENs, "MaNV", "TenNV");
            ViewBag.TenDV = new SelectList(db.DICHVUs, "MaDV", "TenDV");
            ViewBag.MaCN = new SelectList(db.CHINHANHs, "MaCN", "DiaChi");
            ViewBag.MaKH = new SelectList(db.KHACHHANGs, "MaKH", "TenKH");
            ViewBag.Lich = new SelectList(db.Liches, "MaLich", "GioLamViec");

            return View();
        }
        [HttpPost]
        public ActionResult index(FormCollection collection, PHIEUDAT pd)
        {
            var mapd = collection["mapd"];
            var makh = collection["makh"];
            var manv = collection["manv"];
            //var timelap = collection["ThoiGianLap"];
            var timehen = collection["ThoiGianHen"];
            var giolamviec = collection["GioLamViec"];
            var macn = collection["macn"];
            var tenkh = collection["tenkh"];
            var sdt = collection["sdt"];
            pd.MaNV = int.Parse(manv);
            pd.ThoiGianLap = DateTime.Now;
            pd.ThoiGianHen = DateTime.Parse(timehen);
            pd.GioLamViec = TimeSpan.Parse(giolamviec);
            pd.MaCN = int.Parse(macn);
            pd.TenKH = tenkh;
            pd.SDT = sdt;
            var checkpd = db.PHIEUDATs.FirstOrDefault(x => x.MaPD.ToString() == mapd);
            db.PHIEUDATs.InsertOnSubmit(pd);
            db.SubmitChanges();
            return RedirectToAction("index");
        }
      
        [HttpPost]
        public ActionResult GetThoiGian(int maTime)
        {
            var lich = db.Liches.FirstOrDefault(x => x.MaLich == maTime);
            if (lich != null)
            {
                return Content(lich.GioLamViec.ToString());
            }
            else
            {
                return Content("");
            }
        }
     
    }
}