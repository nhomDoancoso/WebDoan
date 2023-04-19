using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebDoan.Models;

namespace WebDoan.Areas.Admin.Controllers
{
    public class TimeController : Controller
    {
        // GET: Admin/Time
        myDataContextDB db = new myDataContextDB();
        public ActionResult Index()
        {
            var lstTime = from ss in db.Lich select ss;
            return View(lstTime);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection, Lich loaitime)
        {
            var matime = collection["MaTime"];
            var checkTime = db.Lich.FirstOrDefault(x => x.MaTime.ToString() == matime);
            if (checkTime != null)
            {
                ViewData["UserExist"] = "mã loại dịch vụ đã tồn tại";
                return this.Create();
            }
            db.Lich.Add(loaitime);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lich lich = db.Lich.Find(id);
            if (lich == null)
            {
                return HttpNotFound();
            }
            return View(lich);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Lich lich = db.Lich.Find(id);
            db.Lich.Remove(lich);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Detail(int id)
        {
            var loaiTime = db.Lich.Where(m => m.MaTime == id).First();
            return View(loaiTime);
        }

        public ActionResult Edit(int id)
        {
            try
            {
                var E_time = db.Lich.First(m => m.MaTime == id);
                return View(E_time);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var sp = db.Lich.First(m => m.MaTime == id);

            var E_tenLoai = TimeSpan.Parse(collection["Time"]);
            sp.MaTime = id;
            if (string.IsNullOrEmpty(E_tenLoai.ToString()))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                sp.Time = E_tenLoai;
                UpdateModel(sp);
                db.SaveChanges();
                return RedirectToAction("Index", "Time");
            }
            return this.Edit(id);
        }
    }
}