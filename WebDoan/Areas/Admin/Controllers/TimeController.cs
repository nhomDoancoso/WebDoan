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
        myDataContextDataContext db = new myDataContextDataContext();
        public ActionResult Index(FormCollection collection, NHANVIEN nhanvien)
        {
            var tendangnhap = collection["UserName"];
            var matkhau = collection["Password"];
            var email = collection["Email"];
            NHANVIEN nv = db.NHANVIENs.FirstOrDefault(x => x.UserName == tendangnhap && x.Password == matkhau);
            if (nv != null)
            {
                Session["ChucVU"] = nv.CHUCVU.TenCV;
            }
            var lstTime = from ss in db.Liches select ss;
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
            var checkTime = db.Liches.FirstOrDefault(x => x.MaTime.ToString() == matime);
            if (checkTime != null)
            {
                ViewData["UserExist"] = "mã loại dịch vụ đã tồn tại";
                return this.Create();
            }
            db.Liches.InsertOnSubmit(loaitime);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var D_DV = db.Liches.First(m => m.MaTime == id);
            return View(D_DV);
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var D_DV = db.Liches.Where(m => m.MaTime == id).First();
            db.Liches.DeleteOnSubmit(D_DV);
            db.SubmitChanges();
            return RedirectToAction("Index", "Time");
        }

        public ActionResult Detail(int id)
        {
            var loaiTime = db.Liches.Where(m => m.MaTime == id).First();
            return View(loaiTime);
        }

        public ActionResult Edit(int id)
        {
            try
            {
                var E_time = db.Liches.First(m => m.MaTime == id);
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
            var sp = db.Liches.First(m => m.MaTime == id);

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
                db.SubmitChanges();
                return RedirectToAction("Index", "Time");
            }
            return this.Edit(id);
        }
    }
}