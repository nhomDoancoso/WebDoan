using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using WebDoan.Models;

namespace WebDoan.Areas.Admin.Controllers
{
    public class ComboController : Controller
    {
        // GET: Admin/Combo
        myDataContextDataContext db = new myDataContextDataContext();
        public ActionResult Index()
        {
            var lstCombo = from ss in db.COMBODICHVUs select ss;
            return View(lstCombo);
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
            ViewBag.TenDV = new SelectList(db.DICHVUs, "MaDV", "TenDV");
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection, COMBODICHVU combo)
        {
            var macb = collection["MaCB"];
            var tencb = collection["TenCB"];
            var gia = collection["Gia"];
            var checkCombo = db.COMBODICHVUs.FirstOrDefault(x => x.MaCB.ToString() == macb);
            if (checkCombo != null)
            {
                ViewData["userExits"] = "mã combo đã tồn tại";
                return this.Create();
            }
            if (string.IsNullOrEmpty(macb) && string.IsNullOrEmpty(gia)&& string.IsNullOrEmpty(tencb))
            {
                ViewData["ViewErr"] = "Không được để trống";
                return this.Create();
            }
            if (combo.Gia <= 0)
            {
                ViewData["Price"] = "không được để giá âm";
                return this.Create();
            }

            double GIA;
            if (!double.TryParse(gia, out GIA))
            {
                ViewData["Price"] = "Giá không hợp lệ!";
                ViewBag.TenDV = new SelectList(db.DICHVUs, "MaDV", "TenDV");
                return View(combo);
            }




            db.COMBODICHVUs.InsertOnSubmit(combo);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Detail(int id)
        {
            var lstCombo = db.COMBODICHVUs.Where(m => m.MaCB == id).First();
            return View(lstCombo);
        }

        public ActionResult Delete(int id)
        {
            var D_DV = db.COMBODICHVUs.First(m => m.MaCB == id);
            return View(D_DV);
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var D_DV = db.COMBODICHVUs.Where(m => m.MaCB == id).First();
            db.COMBODICHVUs.DeleteOnSubmit(D_DV);
            db.SubmitChanges();
            return RedirectToAction("Index", "Combo");
        }

        public ActionResult Edit(int id)
        {
            var lstCombo = db.COMBODICHVUs.First(m => m.MaCB == id);
            return View(lstCombo);
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection, COMBODICHVU combo)
        {
            var E_Combo = db.COMBODICHVUs.First(m => m.MaCB == id);
            var E_tencb = collection["TenCB"];
            var E_gia = collection["Gia"];
            var E_HinhAh = collection["HinhAnh"];
            E_Combo.MaCB = id;
            double GIA;
            if (!double.TryParse(E_gia, out GIA))
            {
                ViewData["Price"] = "Giá không hợp lệ!";
                ViewBag.TenDV = new SelectList(db.DICHVUs, "MaDV", "TenDV");
                return View(combo);
            }
            if (string.IsNullOrEmpty(E_tencb))
            {
                ViewData["Error"] = "Don't empty!";
                return this.Create();
            }
            else
            {
                E_Combo.TenCB = E_tencb;
                E_Combo.Gia =Double.Parse(E_gia);
                E_Combo.HinhAnh = E_HinhAh;
                UpdateModel(E_Combo);
                db.SubmitChanges();
                return RedirectToAction("Index");
            }
            return this.Edit(id);
        }
    }
}