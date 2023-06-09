﻿using Microsoft.Ajax.Utilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDoan.Models;
using System.Data.SqlClient;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Data.Entity;

namespace WebDoan.Controllers
{
    public class BookingController : Controller
    {
        // GET: Booking
        myDataContextDataContext db = new myDataContextDataContext();

        public ActionResult Index()
        {
            var employees = db.NHANVIENs.Where(n => n.MaCV != 1).ToList();
            ViewBag.MaNV = new SelectList(employees, "MaNV", "TenNV");
            ViewBag.TenDV = new SelectList(db.DICHVUs, "MaDV", "TenDV");
            ViewBag.MaCN = new SelectList(db.CHINHANHs, "MaCN", "DiaChi");
            ViewBag.MaKH = new SelectList(db.KHACHHANGs, "MaKH", "TenKH");
            ViewBag.Lich = new SelectList(db.Liches, "MaLich", "GioLamViec");

            return View();
        }
        [HttpPost]
        public ActionResult Index(FormCollection collection, PHIEUDAT pd)
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
            var ghichu = collection["GhiChu"];
            Regex regexPhone = new Regex(@"^(84|0[3|5|7|8|9])+([0-9]{8})\b");
            Match matchPhone = regexPhone.Match(sdt);
            pd.MaNV = int.Parse(manv);
            pd.ThoiGianLap = DateTime.Now;
            pd.MaCN = int.Parse(macn);
            pd.TenKH = tenkh;
            pd.SDT = sdt;
            pd.GhiChu = ghichu;
            pd.TrangThaiPhieuDat = true;
            pd.NHANVIEN = db.NHANVIENs.FirstOrDefault(n => n.MaNV == pd.MaNV);
            ViewBag.MaNV = new SelectList(db.NHANVIENs, "MaNV", "TenNV", pd.MaNV);
            ViewBag.MaCN = new SelectList(db.CHINHANHs, "MaCN", "DiaChi", pd.MaCN);
            if (string.IsNullOrEmpty(pd.TenKH) || string.IsNullOrEmpty(pd.SDT))
            {
                ViewData["empty"] = "không được để trống";
                return View(pd);
            }
            if (!matchPhone.Success)
            {
                ViewData["sdt"] = "Số Điện thoại không đúng định dạng";
                return View(pd);
            }
            TimeSpan gioLamViec;
            if (!TimeSpan.TryParseExact(giolamviec, "hh\\:mm", CultureInfo.InvariantCulture, out gioLamViec))
            {
                ViewData["error"] = "Giờ làm việc không hợp lệ";
                return View(pd);
            }
            DateTime thoiGianHienTai = DateTime.Now;
            DateTime thoiGianKiemTra = thoiGianHienTai.Date + gioLamViec;

            if (thoiGianKiemTra < thoiGianHienTai)
            {
                ViewData["error1"] = "Không được chọn thời gian trong quá khứ";
                return View(pd);
            }

            DateTime thoiGianHen;
            if (!DateTime.TryParseExact(timehen, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out thoiGianHen))
            {
                ViewData["error"] = "Ngày cắt không hợp lệ";
                return View(pd);
            }

            db.PHIEUDATs.InsertOnSubmit(pd);
            db.SubmitChanges();
            return RedirectToAction("BookingSucces", pd);

        }

        public ActionResult BookingSucces(FormCollection collection, PHIEUDAT model)
        {
            ViewBag.MaNV = new SelectList(db.NHANVIENs, "MaNV", "TenNV", model.MaNV);
            ViewBag.Message = "Đặt chổ thành công!";
            ViewBag.MaPD = model.MaPD;
            ViewBag.TenKH = model.TenKH;
            ViewBag.SDT = model.SDT;
            PHIEUDAT pd = db.PHIEUDATs.FirstOrDefault(p => p.MaPD == model.MaPD);
            if (pd != null && pd.NHANVIEN != null)
            {
                ViewBag.TenNV = pd.NHANVIEN.TenNV;
            }
            else
            {
                ViewBag.TenNV = "Không có thông tin nhân viên";
            }
            PHIEUDAT pd1 = db.PHIEUDATs.FirstOrDefault(p => p.MaPD == model.MaPD);
            if (pd1 != null && pd.CHINHANH != null)
            {
                ViewBag.CHINHANH = pd.CHINHANH.DiaChi;
            }
            else
            {
                ViewBag.CHINHANH = "Không có thông tin địa Chỉ";
            }
            ViewBag.TGLAP = model.ThoiGianLap;
            //ViewBag.TGHEN = model.ThoiGianHen;
            DateTime? thoiGianHenNullable = pd.ThoiGianHen;
            DateTime thoiGianHen = thoiGianHenNullable ?? DateTime.MinValue;
            ViewBag.ThoiGianHen = thoiGianHen;
            if (pd.ThoiGianHen.HasValue)
            {
                thoiGianHen = pd.ThoiGianHen.Value;
            }
            else
            {
                // Xử lý giá trị null ở đây nếu cần thiết
                thoiGianHen = DateTime.MinValue;
            }
            ViewBag.ff = model.GioLamViec;
            ViewBag.ghiChu = model.GhiChu;
            ViewBag.State = model.TrangThaiPhieuDat;
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var D_DV = db.PHIEUDATs.First(m => m.MaPD == id);
            return View(D_DV);
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var D_DV = db.PHIEUDATs.Where(m => m.MaPD == id).First();
            db.PHIEUDATs.DeleteOnSubmit(D_DV);
            db.SubmitChanges();
            return RedirectToAction("Index", "Booking");
        }

        public ActionResult Edit(int id)
        {
            try
            {
                ViewBag.MaCN = new SelectList(db.CHINHANHs, "MaCN", "DiaChi");
                ViewBag.MaNV = new SelectList(db.NHANVIENs, "MaNV", "TenNV");
                var E_Sp = db.PHIEUDATs.First(m => m.MaPD == id);
                return View(E_Sp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection, PHIEUDAT phieudat)
        {

            var sp = db.PHIEUDATs.First(m => m.MaPD == id);

            var e_tenkh = collection["TenKH"];
            sp.MaPD = id;
            phieudat.ThoiGianLap = DateTime.Now;

            if (string.IsNullOrEmpty(e_tenkh))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                sp.TenKH = e_tenkh;
                UpdateModel(sp);
                db.SubmitChanges();
                return RedirectToAction("Index", "Booking");
            }
            return this.Edit(id);
        }
    }
}