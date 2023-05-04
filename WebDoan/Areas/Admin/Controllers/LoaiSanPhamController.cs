using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDoan.Models;

namespace WebDoan.Areas.Admin.Controllers
{
    public class LoaiSanPhamController : Controller
    {
        myDataContextDataContext db = new myDataContextDataContext();

        // GET: Admin/LoaiSanPham
        public ActionResult Index(int? page, string SearchString)
        {
            int pageSize = 8;
            int pageNum = page ?? 1;
            var SearchAll = db.LOAISANPHAMs.OrderBy(s => s.TenLoaiSP);
            var SearchSp = db.LOAISANPHAMs.OrderBy(m => m.TenLoaiSP).Where(sp => sp.TenLoaiSP.ToUpper().Contains(SearchString.ToUpper()));
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
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection, LOAISANPHAM lsp)
        {
            var maloaisp = collection["MaLoaiSP"];
            var TenLoaiSp = collection["TenLoaiSP"];
            var checkMalsp = db.LOAISANPHAMs.FirstOrDefault(x => x.MaLoaiSP.ToString() == maloaisp);
           if(checkMalsp != null)
            {
                ViewData["viewData"] = "mã loại sản phẩm đã tồn tại";
                return this.Create();
            }
            if (string.IsNullOrEmpty(maloaisp) &&string.IsNullOrWhiteSpace(TenLoaiSp))
            {
                ViewData["ViewErr"] = "Không được để trống";
                return this.Create();
            }
            db.LOAISANPHAMs.InsertOnSubmit(lsp);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            return View();
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var lstLoaiSp = db.LOAISANPHAMs.Where(m => m.MaLoaiSP == id).First();
            db.LOAISANPHAMs.DeleteOnSubmit(lstLoaiSp);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Detail(int id)
        {
            var loaisp = db.LOAISANPHAMs.Where(m => m.MaLoaiSP == id).First();
            return View(loaisp);
        }

        public ActionResult Edit(int id)
        {
            try
            {
                var E_Sp = db.LOAISANPHAMs.First(m => m.MaLoaiSP == id);
                return View(E_Sp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection, LOAISANPHAM lstsp)
        {

            var sp = db.LOAISANPHAMs.First(m => m.MaLoaiSP == id);

            var E_tenLoai = collection["TenLoaiSP"];
            sp.MaLoaiSP = id;
            if (string.IsNullOrEmpty(E_tenLoai))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                sp.TenLoaiSP = E_tenLoai;
                UpdateModel(sp);
                db.SubmitChanges();
                return RedirectToAction("Index", "LoaiSanPham");
            }
            return this.Edit(id);
        }
    }
}