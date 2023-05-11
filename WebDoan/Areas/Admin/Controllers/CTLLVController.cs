using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDoan.Models;

namespace WebDoan.Areas.Admin.Controllers
{
    public class CTLLVController : Controller
    {
        myDataContextDataContext db = new myDataContextDataContext();

        // GET: Admin/CTLLV
        public ActionResult Index()
        {
            var lst = from ss in db.CTLLVs select ss;
            return View(lst);
        }

        public ActionResult Create()
        {
            ViewBag.MaNV = new SelectList(db.NHANVIENs, "MaNV", "TenNV");
            ViewBag.MaLich = new SelectList(db.Liches, "MaLich", "GioLamViec");
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection, CTLLV cTLLV)
        {
            var maLich = collection["MaLich"];
            var manv = collection["MaNV"];
            var checkCombo = db.CTLLVs.FirstOrDefault(x => x.MaLich.ToString() == maLich);
            db.CTLLVs.InsertOnSubmit(cTLLV);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            var D_DV = db.CTLLVs.First(m => m.MaLich == id);
            return View(D_DV);
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var D_DV = db.CTLLVs.Where(m => m.MaLich == id).First();
            db.CTLLVs.DeleteOnSubmit(D_DV);
            db.SubmitChanges();
            return RedirectToAction("Index", "CTLLV");
        }
    }
}