﻿using Microsoft.Ajax.Utilities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDoan.Models;

namespace WebDoan.Areas.Admin.Controllers
{
    public class SanPhamController : Controller
    {
        // GET: Admin/SanPham
        myDataContextDataContext db = new myDataContextDataContext();

        public ActionResult Index(int? page, string SearchString)
        {
            int pageSize = 8;
            int pageNum = page ?? 1;
            var SearchAll = db.SANPHAMs.OrderBy(s => s.TenSP);
            var SearchSp = db.SANPHAMs.OrderBy(m => m.TenSP).Where(sp => sp.TenSP.ToUpper().Contains(SearchString.ToUpper()));
            page = 1;
            if (SearchString == null || SearchString == "")
                return View(SearchAll.ToPagedList(pageNum, pageSize));
            else if (SearchSp != null)
                return View(SearchSp.ToPagedList(pageNum, pageSize));
            else
                return View(SearchAll.ToPagedList(pageNum, pageSize));
        }

        public string ProcessUpload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return "";
            }
            file.SaveAs(Server.MapPath("~/Content/images/" + file.FileName));
            return "/Content/images/" + file.FileName;
        }

        public ActionResult Create()
        {
          
            var listLoaiSP = db.LOAISANPHAMs.ToList();
            var selectListLoaiSP = new SelectList(listLoaiSP, "MaLoaiSP", "TenLoaiSP");

            // Gán danh sách loại sản phẩm vào ViewData
            ViewData["LoaiSP"] = selectListLoaiSP;
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection, SANPHAM sp)
        {
            if (Session["TaiKhoanAdmin"] == null || Session["TaiKhoanAdmin"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "LoginAdmin");
            }
            int maTK = (int)Session["TaiKhoanAdmin"];
            var masp = collection["MaSP"];
            var tensp = collection["TenSP"];
            var maloaiSp = collection["MaloaiSP"];
            var gia = collection["Gia"];
            var soluong = collection["SoLuong"];
            var hinhanh = collection["HinhAnh"];
            var checkMaDV = db.SANPHAMs.FirstOrDefault(x => x.MaSP.ToString() == masp);
            var listLoaiSP = db.LOAISANPHAMs.ToList();
            var selectListLoaiSP = new SelectList(listLoaiSP, "MaLoaiSP", "TenLoaiSP");
            ViewData["LoaiSP"] = selectListLoaiSP;


            if (sp.Gia <= 0)
            {
                ViewData["Price"] = "không được để giá âm";
                return this.Create();
            }
            if (sp.SoLuong <= 0)
            {
                ViewData["sl"] = "không được để sl âm";
                return this.Create();
            }
            if (string.IsNullOrEmpty(tensp) && string.IsNullOrEmpty(gia) && string.IsNullOrEmpty(masp) && string.IsNullOrEmpty(soluong))
            {
                ViewData["ViewErr"] = "Không được để trống";
                return this.Create();
            }
            double GIA;
            if (!double.TryParse(gia, out GIA))
            {
                ViewData["Price2"] = "Giá không hợp lệ!";
                return View(sp);
            } // Gán danh sách loại sản phẩm vào ViewData
            if (string.IsNullOrEmpty(maloaiSp))
            {
                ViewData["MaLoaiSPErr"] = "Mã loại sản phẩm không được để trống";
                return View(sp);
            }
            db.SANPHAMs.InsertOnSubmit(sp);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }

       
        public ActionResult Edit(int id)
        {
            try
            {
                ViewBag.MaLoaiSP = new SelectList(db.LOAISANPHAMs, "MaLoaiSP", "TenLoaiSP");
                var E_Sp = db.SANPHAMs.First(m => m.MaSP == id);
                return View(E_Sp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection, SANPHAM sanpham)
        {
            var sp = db.SANPHAMs.First(m => m.MaSP == id);

            var E_tendv = collection["TenSP"];
            var e_gia = collection["Gia"];
            var e_soluong = collection["SoLuong"];
            sp.MaSP = id;
            ViewBag.MaLoaiSP = new SelectList(db.LOAISANPHAMs, "MaLoaiSP", "TenLoaiSP");
            if (string.IsNullOrEmpty(E_tendv) || string.IsNullOrEmpty(e_gia) || string.IsNullOrEmpty(e_soluong))
            {
                ViewData["SS"] = "Không được để trống";
                return this.Create();
            }
            else
            {
                sp.TenSP = E_tendv;
                UpdateModel(sp);
                db.SubmitChanges();
                return RedirectToAction("Index", "SanPham");
            }
            return this.Edit(id);
        }

        public ActionResult Detail(int id)
        {
            var D_SanPham = db.SANPHAMs.Where(m => m.MaSP == id).First();
            return View(D_SanPham);
        }

        public ActionResult Delete(int id)
        {
            var D_DV = db.SANPHAMs.First(m => m.MaSP == id);
            return View(D_DV);
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var D_DV = db.SANPHAMs.Where(m => m.MaSP == id).First();
            db.SANPHAMs.DeleteOnSubmit(D_DV);
            db.SubmitChanges();
            return RedirectToAction("Index", "SanPham");
        }
    }
}