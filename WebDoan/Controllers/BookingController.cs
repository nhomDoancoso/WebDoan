﻿using Microsoft.Ajax.Utilities;
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
            ViewBag.MaNV = new SelectList(db.NHANVIENs, "MaNV", "TenNV");
            ViewBag.TenDV = new SelectList(db.DICHVUs, "MaDV", "TenDV");
            ViewBag.MaCN = new SelectList(db.CHINHANHs, "MaCN", "DiaChi");
            ViewBag.MaKH = new SelectList(db.KHACHHANGs, "MaKH", "TenKH");

            return View();
        }
        [HttpPost]
        public ActionResult Index(FormCollection collection, PHIEUDAT pd , Lich lich)
        {
            var MaPD = collection["MaPD"];
            var MAKH = collection["MaKH"];
            var MaNV = collection["MaNV"];
            var TimeLap = collection["TimeLap"];
            var TimeHen = collection["TimeHen"];
            var MaCn = collection["MaCN"];
            var tenkh = collection["TenKH"];
            var sdt = collection["SDT"];
            var checkPD = db.PHIEUDATs.FirstOrDefault(x => x.MaPD.ToString() == MaPD);
            db.PHIEUDATs.InsertOnSubmit(pd);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }

    }
}