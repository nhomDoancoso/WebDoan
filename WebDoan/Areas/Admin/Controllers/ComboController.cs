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
    public class ComboController : Controller
    {
        // GET: Admin/Combo
        myDataContextDB db = new myDataContextDB();
        public ActionResult Index()
        {
            var lstCombo = from ss in db.COMBODICHVU select ss;
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
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection, COMBODICHVU combo)
        {
            var macb = collection["MaCB"];
            var checkCombo = db.COMBODICHVU.FirstOrDefault(x => x.MaCB.ToString() == macb);
            if (combo.MaCB.ToString().IsNullOrWhiteSpace())
            {
                ViewData["ViewErr"] = "Không được để trống";
                return this.Create();
            }

            if (combo.TenCB.ToString().IsNullOrWhiteSpace())
            {
                ViewData["ViewErr2"] = "Không được để trống";
                return this.Create();
            }

            if(combo.Gia.ToString().IsNullOrWhiteSpace() && combo.Gia <= 0)
            {
                ViewData["ViewErr3"] = "Không được để trống";
                ViewData["Vieweee"] = "Gía tiền không được âm";
                return this.Create();
            }
            db.COMBODICHVU.Add(combo);
            db.SaveChanges();
            return RedirectToAction("loaidv");
        }

        public ActionResult Detail(int id)
        {
            var lstCombo = db.COMBODICHVU.Where(m => m.MaCB == id).First();
            return View(lstCombo);
        }

        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COMBODICHVU combo  = db.COMBODICHVU.Find(id);
            if (combo == null)
            {
                return HttpNotFound();
            }
            return View(combo);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            COMBODICHVU combodv = db.COMBODICHVU.Find(id);
            db.COMBODICHVU.Remove(combodv);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
           // ViewBag.MaCV = new SelectList(db.CHUCVU, "MaCV", "TenCV");
            var lstCombo = db.COMBODICHVU.First(m => m.MaCB == id);
            return View(lstCombo);
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var E_Combo = db.COMBODICHVU.First(m => m.MaCB == id);
            var E_tencb = collection["TenCB"];
            var E_Gia = double.Parse(collection["Gia"]);
            var E_HinhAh = collection["HinhAnh"];
            E_Combo.MaCB = id;
            if (string.IsNullOrEmpty(E_tencb))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                E_Combo.TenCB = E_tencb;
                E_Combo.Gia = E_Gia;
                E_Combo.HinhAnh = E_HinhAh;
                UpdateModel(E_Combo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return this.Edit(id);
        }
    }
}