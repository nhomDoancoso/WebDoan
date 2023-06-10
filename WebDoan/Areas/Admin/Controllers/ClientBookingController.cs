using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDoan.Models;

namespace WebDoan.Areas.Admin.Controllers
{
    public class ClientBookingController : Controller
    {
        // GET: Admin/ClientBooking
        myDataContextDataContext db = new myDataContextDataContext();

        public ActionResult Index()
        {
            var lst = from ss in db.PHIEUDATs select ss;
            return View(lst);   
        }

        public ActionResult Confirm(int id)
        {

            var dbContext = new myDataContextDataContext();
            var phieudat = dbContext.PHIEUDATs.Where(p => p.MaPD == id).FirstOrDefault();

            if (phieudat != null)
            {
                phieudat.TrangThaiPhieuDat = !phieudat.TrangThaiPhieuDat;
                dbContext.SubmitChanges(); 
            }

            return RedirectToAction("Index", "ClientBooking", new { id = id });
          
        }
        public ActionResult Delete(int id)
        {
            
            
            // Kiểm tra xem người dùng có phải quản lý hay không
            //if (Session["CHucVU1"].ToString() == "Quản Lý")
            //{
            //     sreturn RedirectToAction("Index", "Home"); 
            //}
                var D_DV = db.PHIEUDATs.First(m => m.MaPD == id);
                return View(D_DV);
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
           
            var D_DV = db.PHIEUDATs.Where(m => m.MaPD == id).First();
            db.PHIEUDATs.DeleteOnSubmit(D_DV);
            db.SubmitChanges();
            return RedirectToAction("Index", "ClientBooking");
        }

        public ActionResult Edit(int id)
        {
            var employees = db.NHANVIENs.Where(n => n.MaCV != 1).ToList();
            ViewBag.MaNV = new SelectList(employees, "MaNV", "TenNV");
            ViewBag.TenDV = new SelectList(db.DICHVUs, "MaDV", "TenDV");
            ViewBag.MaCN = new SelectList(db.CHINHANHs, "MaCN", "DiaChi");
            ViewBag.MaKH = new SelectList(db.KHACHHANGs, "MaKH", "TenKH");
            ViewBag.Lich = new SelectList(db.Liches, "MaLich", "GioLamViec");
            var lstCombo = db.PHIEUDATs.First(m => m.MaPD == id);
            return View(lstCombo);
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection, PHIEUDAT pd)
        {
            var E_PD = db.PHIEUDATs.First(m => m.MaPD == id);
            ViewBag.MaNV = new SelectList(db.NHANVIENs, "MaNV", "TenNV", pd.MaNV);
            ViewBag.MaCN = new SelectList(db.CHINHANHs, "MaCN", "DiaChi", pd.MaCN);
            var E_MaPD = collection["MaPD"];
            var E_Manv = collection["MaNV"];
            var E_THoiGianLap = collection["ThoiGianLap"];
            var E_ThoiGianHen = collection["ThoiGianHen"];
            var E_MaCN = collection["MaCN"];
            var E_TenKH = collection["TenKH"];
            var E_SDT = collection["SDT"];
            var E_GioLamViec = collection["GioLamViec"];
            var E_TranfThaiPhieuDat = collection["TrangThaiPhieuDat"];
            var E_GhiChu = collection["GhiChu"];
            E_PD.MaPD = id;

            if (string.IsNullOrEmpty(E_TenKH))
            {
                ViewData["null"] = "không được để trống";
                return View();
            }
            else
            {
                E_PD.TenKH = E_TenKH;
                UpdateModel(E_PD);
                db.SubmitChanges();
                return RedirectToAction("Index");
            }
            return this.Edit(id);
        }
    }
}