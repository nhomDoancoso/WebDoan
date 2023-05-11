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
            ViewBag.Lich = new SelectList(db.Liches, "MaTime", "Time");

            return View();
        }
        [HttpPost]
        public ActionResult index(FormCollection collection, PHIEUDAT pd)
        {
            var mapd = collection["mapd"];
            var makh = collection["makh"];
            var manv = collection["manv"];
            var timelap = collection["timelap"];
            var timehen = collection["timehen"];
            var macn = collection["macn"];
            var tenkh = collection["tenkh"];
            var sdt = collection["sdt"];
            pd.MaNV = int.Parse(manv);
            pd.TimeLap = DateTime.Parse(timehen);
            pd.TimeHen = DateTime.ParseExact(timelap, "dd/MM/yyyy hh:mm:ss", null);
            pd.MaCN = int.Parse(macn);
            pd.TenKH = tenkh;
            pd.SDT = sdt;
            var checkpd = db.PHIEUDATs.FirstOrDefault(x => x.MaPD.ToString() == mapd);
            db.PHIEUDATs.InsertOnSubmit(pd);
            db.SubmitChanges();
            return RedirectToAction("index");
        }
        //public ActionResult Index(FormCollection collection, PHIEUDAT pd, Lich lich)
        //{
        //    string connectionString = @"Data Source=HIEPHUYNHB279\SQLEXPRESS;Initial Catalog=QuanLyDatLichCatToc;Integrated Security=True";
        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        connection.Open();
        //        string sql = "SELECT * FROM PHIEUDAT JOIN Lich ON PHIEUDAT.MaPD = Lich.MaTime";
        //        using (SqlCommand command = new SqlCommand(sql, connection))
        //        {
        //            using (SqlDataReader reader = command.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    var MaPD = collection["MaPD"];
        //                    var MAKH = collection["MaKH"];
        //                    var MaNV = collection["MaNV"];
        //                    var TimeLap = collection["TimeLap"];
        //                    var TimeHen = collection["TimeHen"];
        //                    var MaCn = collection["MaCN"];
        //                    var tenkh = collection["TenKH"];
        //                    var sdt = collection["SDT"];
        //                    pd.MaNV = int.Parse(MaNV);
        //                    pd.TimeLap = DateTime.Parse(TimeHen);
        //                    pd.TimeHen = DateTime.Parse(TimeLap);
        //                    pd.MaCN = int.Parse(MaCn);
        //                    pd.TenKH = tenkh;
        //                    pd.SDT = sdt;
        //                    var checkPD = db.PHIEUDATs.FirstOrDefault(x => x.MaPD.ToString() == MaPD);
        //                    db.PHIEUDATs.InsertOnSubmit(pd);
        //                    db.SubmitChanges();
        //                }
        //            }
        //        }
        //    }
        //    return RedirectToAction("Index");
        //}
        [HttpPost]
        public ActionResult GetThoiGian(int maTime)
        {
            var lich = db.Liches.FirstOrDefault(x => x.MaTime == maTime);
            if (lich != null)
            {
                return Content(lich.Time.ToString());
            }
            else
            {
                return Content("");
            }
        }
        //public IActionResult MyAction()
        //{
        //    string connectionString = @"Data Source=HIEPHUYNHB279\SQLEXPRESS;Initial Catalog=QuanLyDatLichCatToc;Integrated Security=True" ;
        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        connection.Open();
        //        string sql = "SELECT * FROM PHIEUDAT JOIN Lich ON PHIEUDAT.MaPD = Lich.MaTime";
        //        using (SqlCommand command = new SqlCommand(sql, connection))
        //        {
        //            using (SqlDataReader reader = command.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    //xử lí 
        //                }
        //            }
        //        }
        //    }
        //    return (IActionResult)View();
        //}
    }
}