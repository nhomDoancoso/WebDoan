using Microsoft.Ajax.Utilities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebDoan.Models;

namespace WebDoan.Areas.Admin.Controllers
{
    public class LoaiDVController : Controller
    {
        // GET: Admin/LoaiDV
        myDataContextDataContext db = new myDataContextDataContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoaiDv(int? page, string SearchString)
        {
            int pageSize = 5;
            int pageNum = page ?? 1;
            var SearchAll = db.LOAIDICHVUs.OrderBy(s => s.TenLoaiDV);
            var SearchSp = db.LOAIDICHVUs.OrderBy(m => m.TenLoaiDV).Where(sp => sp.TenLoaiDV.ToUpper().Contains(SearchString.ToUpper()));
            page = 1;
            if (SearchString == null || SearchString == "")
                return View(SearchAll.ToPagedList(pageNum, pageSize));
            else if (SearchSp != null)
                return View(SearchSp.ToPagedList(pageNum, pageSize));
            else
                return View(SearchAll.ToPagedList(pageNum, pageSize));
            //var lstLoaiDV = from ss in db.LOAIDICHVUs select ss;
            //return View(lstLoaiDV);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection, LOAIDICHVU loaidv)
        {
            if (Session["TaiKhoanAdmin"] == null || Session["TaiKhoanAdmin"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "LoginAdmin");
            }
            int maTK = (int)Session["TaiKhoanAdmin"];
            var madv = collection["MaLoaiDV"];
            var checkLoaiDV = db.LOAIDICHVUs.FirstOrDefault(x => x.MaLoaiDV == madv);
            if (checkLoaiDV != null)
            {
                ViewData["UserExist"] = "mã loại dịch vụ đã tồn tại";
                return this.Create();
            }
            if (loaidv.MaLoaiDV.IsNullOrWhiteSpace() && loaidv.TenLoaiDV.IsNullOrWhiteSpace())
            {
                ViewData["ViewErr"] = "Không được để trống";
                return this.Create();
            }
            db.LOAIDICHVUs.InsertOnSubmit(loaidv);
            db.SubmitChanges();
            return RedirectToAction("LoaiDv");
        }
        public ActionResult Delete(string id)
        {
            var listloaidv = db.LOAIDICHVUs.First(m => m.MaLoaiDV == id);
            return View(listloaidv);
        }
        [HttpPost] 
        public ActionResult Delete(string id, FormCollection collection)
        {
            try
            {

                var listloaidv = db.LOAIDICHVUs.Where(m => m.MaLoaiDV == id).First();
                db.LOAIDICHVUs.DeleteOnSubmit(listloaidv);
                db.SubmitChanges();
                return RedirectToAction("LoaiDv");

            }
            catch (Exception e)
            {
                return RedirectToAction("Error", new { message = e.Message });
            }
            return RedirectToAction("LoaiDv");   
        }

        public ActionResult Detail(string id)
        {
            var loaidv = db.LOAIDICHVUs.Where(m => m.MaLoaiDV == id).First();
            return View(loaidv);
        }

        public ActionResult Edit(string id)
        {
            try
            {
                var E_Sp = db.LOAIDICHVUs.First(m => m.MaLoaiDV == id);
                return View(E_Sp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult Edit(string id, FormCollection collection, LOAIDICHVU loaidv)
        {

            var sp = db.LOAIDICHVUs.First(m => m.MaLoaiDV == id);

            var E_tenLoai = collection["TenLoaiDV"];
            sp.MaLoaiDV = id;
            if (string.IsNullOrEmpty(E_tenLoai))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                sp.TenLoaiDV = E_tenLoai;
                UpdateModel(sp);
                db.SubmitChanges();
                return RedirectToAction("LoaiDv", "LoaiDv");
            }
            return this.Edit(id);
        }
    }
}