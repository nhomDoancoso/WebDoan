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

        public ActionResult Create()
        {
            ViewBag.MaLoaiSP = new SelectList(db.LOAISANPHAMs, "MaLoaiSP", "TenLoaiSP");
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection, SANPHAM sp)
        {
            var masp = collection["MaSP"];
            var tensp = collection["TenSP"];
            var maloaiSp = collection["MaloaiSP"];
            var gia = collection["Gia"];
            var soluong = collection["SoLuong"];
            var hinhanh = collection["HinhAnh"];

            var checkMaDV = db.SANPHAMs.FirstOrDefault(x => x.MaSP.ToString() == masp);


            if(checkMaDV != null)
            {
                ViewData["masp"] = "mã sp đã tồn tại ";
                return this.Create();
            }
            if (sp.Gia <= 0)
            {
                ViewData["Price"] = "không được để giá âm";
                return this.Create();
            }
            if (sp.TenSP.IsNullOrWhiteSpace())
            {
                ViewData["ViewErr"] = "Không được để trống";
                return this.Create();
            }
            db.SANPHAMs.InsertOnSubmit(sp);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }

    }
}