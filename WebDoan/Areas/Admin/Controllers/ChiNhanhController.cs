using Microsoft.Ajax.Utilities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using WebDoan.Models;

namespace WebDoan.Areas.Admin.Controllers
{
    public class ChiNhanhController : Controller
    {
        myDataContextDataContext db = new myDataContextDataContext();

        // GET: Admin/ChiNhanh
        public ActionResult Index(int? page, string SearchString)
        {
            int pageSize = 8;
            int pageNum = page ?? 1;
            var search = db.CHINHANHs.OrderBy(s => s.HotLine);
            //var SearchAll = db.CHINHANHs.OrderBy(s => s.MaCN).Where(s => s.DiaChi.ToUpper().Contains(SearchString.ToUpper()));
             var SearchAll = db.CHINHANHs.OrderBy(s => s.MaCN);
         var SearchSp = db.CHINHANHs.OrderBy(m => m.DiaChi).Where(sp => sp.DiaChi.ToUpper().Contains(SearchString.ToUpper()));
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
           // ViewBag.MaLoaiDV = new SelectList(db.LOAIDICHVUs, "MaLoaiDV", "TenLoaiDV");
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection, CHINHANH chinhanh)
        {
            if (Session["TaiKhoanAdmin"] == null || Session["TaiKhoanAdmin"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "LoginAdmin");
            }
            int maTK = (int)Session["TaiKhoanAdmin"];
            var macn = collection["MaCN"];
            var DiaChi = collection["DiaChi"];
            var hotline = collection["HotLine"];
            Regex regexPhone = new Regex(@"(84|\+84|0)(3[2-9]|5[6|8|9]|7[0|6-9]|8[1-5|8|9]|9[0-4|6-9])[0-9]{7}");
            Match matchPhone = regexPhone.Match(hotline);
            var checkCombo = db.CHINHANHs.FirstOrDefault(x => x.MaCN.ToString() == macn);
            if (checkCombo != null)
            {
                ViewData["userExits"] = "mã cn đã tồn tại";
                return this.Create();
            }
            if (hotline.Length < 10 || hotline.Length > 10)
            {
                ViewData["lenghtNum"] = "số điện thoải phải 10 số";
                return this.Create();
            }
            if (!matchPhone.Success)
            {
                ViewData["NumWrong"] = "số điện thoải phải đúng định dạng";
                return this.Create();
            }
            if (string.IsNullOrEmpty(DiaChi) && string.IsNullOrEmpty(hotline) && string.IsNullOrEmpty(macn))
            {
                ViewData["ViewErr"] = "Không được để trống";
                return this.Create();
            }
            db.CHINHANHs.InsertOnSubmit(chinhanh);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Detail(int id)
        {
            var lstCombo = db.CHINHANHs.Where(m => m.MaCN == id).First();
            return View(lstCombo);
        }

        public ActionResult Edit(int id)
        {
            var lstCombo = db.CHINHANHs.First(m => m.MaCN == id);
            return View(lstCombo);
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection, CHINHANH chinhanh)
        {
            var E_ChiNhanh = db.CHINHANHs.First(m => m.MaCN == id);
            var E_DiaChi = collection["DiaChi"];
            var E_Holine = collection["HotLine"];
            E_ChiNhanh.MaCN = id;
            Regex regexPhone = new Regex(@"(84|\+84|0)(3[2-9]|5[6|8|9]|7[0|6-9]|8[1-5|8|9]|9[0-4|6-9])[0-9]{7}");
            Match matchPhone = regexPhone.Match(E_Holine);
            if(E_Holine.Length < 10 || E_Holine.Length > 10)
            {
                ViewData["lenghtNum"] = "số điện thoải phải 10 số";
                return this.Create();
            }
            if (!matchPhone.Success )
            {
                ViewData["NumWrong"] = "số điện thoải phải đúng định dạng";
                return this.Create();
            }
            if (string.IsNullOrEmpty(E_DiaChi) && string.IsNullOrEmpty(E_Holine))
            {
                ViewData["Error"] = "Don't empty!";
                return this.Create();
            }
            else
            {
                E_ChiNhanh.DiaChi = E_DiaChi;
                E_ChiNhanh.HotLine = E_Holine;
                UpdateModel(E_ChiNhanh);
                db.SubmitChanges();
                return RedirectToAction("Index");
            }
            return this.Edit(id);
        }

        public ActionResult Delete(int id)
        {
            var D_DV = db.CHINHANHs.First(m => m.MaCN == id);
            return View(D_DV);
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var D_DV = db.CHINHANHs.Where(m => m.MaCN == id).First();
            db.CHINHANHs.DeleteOnSubmit(D_DV);
            db.SubmitChanges();
            return RedirectToAction("Index", "ChiNhanh");
        }
    }
}