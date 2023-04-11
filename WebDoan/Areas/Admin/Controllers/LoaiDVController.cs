using Microsoft.Ajax.Utilities;
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
        myDataContextDB db = new myDataContextDB();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoaiDv()
        {
            var lstLoaiDV = from ss in db.LOAIDICHVU select ss;
            return View(lstLoaiDV);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection, LOAIDICHVU loaidv)
        {
            var madv = collection["MaLoaiDV"];
            var checkLoaiDV = db.LOAIDICHVU.FirstOrDefault(x => x.MaLoaiDV == madv);
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
            db.LOAIDICHVU.Add(loaidv);
            db.SaveChanges();
            return RedirectToAction("LoaiDv");
        }
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOAIDICHVU lOAIDICHVU = db.LOAIDICHVU.Find(id);
            if (lOAIDICHVU == null)
            {
                return HttpNotFound();
            }
            return View(lOAIDICHVU);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            LOAIDICHVU loaidv = db.LOAIDICHVU.Find(id);
            db.LOAIDICHVU.Remove(loaidv);
            db.SaveChanges();
            return RedirectToAction("LoaiDv");
        }

        public ActionResult Detail(string id)
        {
            var loaidv = db.LOAIDICHVU.Where(m => m.MaLoaiDV == id).First();
            return View(loaidv);
        }

        public ActionResult Edit(string id)
        {
            try
            {
                var E_Sp = db.LOAIDICHVU.First(m => m.MaLoaiDV == id);
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

            var sp = db.LOAIDICHVU.First(m => m.MaLoaiDV == id);

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
                db.SaveChanges();
                return RedirectToAction("LoaiDv", "LoaiDv");
            }
            return this.Edit(id);
        }
    }
}