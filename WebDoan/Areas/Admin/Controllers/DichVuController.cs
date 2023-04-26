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
        myDataContextDataContext db = new myDataContextDataContext();
        public ActionResult Index(int? page, string SearchString)
        {
            int pageSize = 2;
            int pageNum = page ?? 1;
            var SearchAll = db.DICHVUs.OrderBy(s => s.TenDV);
            var SearchSp = db.DICHVUs.OrderBy(m => m.TenDV).Where(sp => sp.TenDV.ToUpper().Contains(SearchString.ToUpper()));
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
            ViewBag.MaLoaiDV = new SelectList(db.LOAIDICHVUs, "MaLoaiDV", "TenLoaiDV");
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection, DICHVU dichvu)
        {
            var madv = collection["MaDV"];
            var gia = collection["Gia"];
            var checkMaDV = db.DICHVUs.FirstOrDefault(x => x.MaLoaiDV == madv);
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
            db.DICHVUs.InsertOnSubmit(dichvu);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            try
            {
                ViewBag.MaLoaiDV = new SelectList(db.LOAIDICHVUs, "MaLoaiDV", "TenLoaiDV");
                var E_Sp = db.DICHVUs.First(m => m.MaDV == id);
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

            var sp = db.DICHVUs.First(m => m.MaDV == id);

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
                return this.Create();
            }
            else
            {
                sp.TenDV = E_tendv;
                UpdateModel(sp);
                db.SubmitChanges();
                return RedirectToAction("Index", "DichVu");
            }
            return this.Edit(id);
        }

        public ActionResult Detail(int id)
        {
            var D_DichVu = db.DICHVUs.Where(m => m.MaDV == id).First();
            return View(D_DichVu);
        }

        public ActionResult Delete(int id)
        {
            var D_DV = db.DICHVUs.First(m => m.MaDV == id);
            return View(D_DV);
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var D_DV = db.DICHVUs.Where(m => m.MaDV == id).First();
            db.DICHVUs.DeleteOnSubmit(D_DV);
            db.SubmitChanges();
            return RedirectToAction("Index", "DichVu");
        }
    }
}