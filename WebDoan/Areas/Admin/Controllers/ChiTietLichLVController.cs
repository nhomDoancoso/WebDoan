using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDoan.Models;

namespace WebDoan.Areas.Admin.Controllers
{
    public class ChiTietLichLVController : Controller
    {
        // GET: Admin/ChiTietLichLV
        myDataContextDataContext db = new myDataContextDataContext();
        public ActionResult Index(FormCollection collection, CHUCVU cHUCVU)
        {
            var macv = collection["MaCV"];
            var lstDetailTime = from ss in db.CTLLVs select ss;
            var chucvu = db.CHUCVUs.FirstOrDefault(x => x.MaCV.ToString() == macv);
            if(cHUCVU != null)
            {
                ViewBag.Chucu = cHUCVU.MaCV;
            }
            return View(lstDetailTime);
        }

        public ActionResult Create()
        {
            ViewBag.MaTime = new SelectList(db.Liches, "MaTime", "Time");
            ViewBag.MaNV = new SelectList(db.NHANVIENs, "MaNV", "TenNV");
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection, CTLLV cttlv)
        {
            var matime = collection["MaTime"];
            var manv = collection["MaNV"];
            var checkTime = db.CTLLVs.FirstOrDefault(x => x.MaTime.ToString() == matime && x.MaNV.ToString() == manv);
            if (checkTime != null)
            {
                ViewData["UserExist"] = "mã loại dịch vụ đã tồn tại";
                return this.Create();
            }
            db.CTLLVs.InsertOnSubmit(cttlv);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            var D_DV = db.CTLLVs.First(m => m.MaTime == id);
            return View(D_DV);
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var D_DV = db.CTLLVs.Where(m => m.MaTime == id).First();
            db.CTLLVs.DeleteOnSubmit(D_DV);
            db.SubmitChanges();
            return RedirectToAction("Index", "ChiTietLichLV");
        }

        public ActionResult Edit(int id)
        {
            try
            {
                ViewBag.MaTime = new SelectList(db.Liches, "MaTime", "Time");
                ViewBag.MaNV = new SelectList(db.NHANVIENs, "MaNV", "TenNV");
                var E_Sp = db.Liches.First(m => m.MaTime == id);
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

            var sp = db.Liches.First(m => m.MaTime == id);

            var E_tendv =Convert.ToInt32(collection["MaTime"]);
            sp.MaTime = id;
           
            if (string.IsNullOrEmpty(E_tendv.ToString()))
            {
                ViewData["Error"] = "Don't empty!";
                return this.Create();
            }
            else
            {
                sp.MaTime = E_tendv;
                UpdateModel(sp);
                db.SubmitChanges();
                return RedirectToAction("Index", "DichVu");
            }
            return this.Edit(id);
        }
    }
}