using Microsoft.Ajax.Utilities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDoan.Models;

namespace WebDoan.Areas.Admin.Controllers
{
    public class DichVuController : Controller
    {
        // GET: Admin/DichVu
        // GET: Admin/DichVu
        myDataContextDB db = new myDataContextDB();
        public ActionResult Index(int? page, string SearchString)
        {
            int pageSize = 8;
            int pageNum = page ?? 1;
            var SearchAll = db.DICHVU.OrderBy(s => s.TenDV);
            var SearchSp = db.DICHVU.OrderBy(m => m.TenDV).Where(sp => sp.TenDV.ToUpper().Contains(SearchString.ToUpper()));
            page = 1;
            if (SearchString == null || SearchString == "")
                return View(SearchAll.ToPagedList(pageNum, pageSize));
            else if (SearchSp != null)
                return View(SearchSp.ToPagedList(pageNum, pageSize));
            else
                return View(SearchAll.ToPagedList(pageNum, pageSize));
        }

        public ActionResult Create()
        {
            ViewBag.MaLoaiDV = new SelectList(db.LOAIDICHVU, "MaLoaiDV", "TenLoaiDV");
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection, DICHVU dichvu)
        {
            var madv = collection["MaDV"];
            var gia = collection["Gia"];
            var checkMaDV = db.DICHVU.FirstOrDefault(x => x.MaLoaiDV == madv);
            if (dichvu.Gia <= 0)
            {
                ViewData["Price"] = "không được để giá âm";
                return this.Create();
            }
            if (dichvu.TenDV.IsNullOrWhiteSpace() && dichvu.MaLoaiDV.IsNullOrWhiteSpace())
            {
                ViewData["ViewErr"] = "Không được để trống";
                return this.Create();
            }
            db.DICHVU.Add(dichvu);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            try
            {
                ViewBag.MaLoaiDV = new SelectList(db.LOAIDICHVU, "MaLoaiDV", "TenLoaiDV");
                var E_Sp = db.DICHVU.First(m => m.MaDV == id);
                return View(E_Sp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection, DICHVU dichvu)
        {

            var sp = db.DICHVU.First(m => m.MaDV == id);

            var E_tendv = collection["TenDV"];
            sp.MaDV = id;
            if (dichvu.Gia <= 0)
            {
                ViewData["Price"] = "không được để giá âm";
                return this.Create();
            }
            if (string.IsNullOrEmpty(E_tendv))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                sp.TenDV = E_tendv;
                UpdateModel(sp);
                db.SaveChanges();
                return RedirectToAction("Index", "DichVu");
            }
            return this.Edit(id);
        }

        public ActionResult Detail(int id)
        {
            var D_DichVu = db.DICHVU.Where(m => m.MaDV == id).First();
            return View(D_DichVu);
        }

        public ActionResult Delete(int id)
        {
            var D_DV = db.DICHVU.First(m => m.MaDV == id);
            return View(D_DV);
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var D_DV = db.DICHVU.Where(m => m.MaDV == id).First();
            db.DICHVU.Remove(D_DV);
            db.SaveChanges();
            return RedirectToAction("listSP", "HomeAdmin");
        }
    }
}