using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDoan.Models;

namespace WebDoan.Areas.Admin.Controllers
{
    public class LoginAdminController : Controller
    {
        // GET: Admin/LoginAdmin
        myDataContextDB db = new myDataContextDB();
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection collection, NHANVIEN nhanvien)
        {
            var tendangnhap = collection["UserName"];
            var matkhau = collection["Password"];
            var email = collection["Email"];

            NHANVIEN nv = db.NHANVIEN.FirstOrDefault(x => x.UserName == tendangnhap && x.Password == matkhau);
            
            if (nv != null)
            {
                ViewBag.ThongBao = "Chúc mừng đăng nhập thà nh công";
                Session["User"] = nv.TenNV;
                Session["CHucVU"] = nv.CHUCVU.MaCV;
                Session["TaiKhoan"] = nv.MaNV;
                Session["User"] = nv.UserName;
                Session["Account"] = nv.MaCV;
                Session["FullTaiKhoan"] = nv;
                Session["Image"] = nv.HinhNV;

            }
            else if (nv == null)
            {
                ViewData["ErrorAccount"] = "sai mật khẩu hoặc Tên đăng nhập không tồn tại vui lòng nhập lại";
                return this.DangNhap();
            }
            else
            {
                ViewData["ErrorPass"] = "Mật khẩu không đúng";
                return this.DangNhap();
            }
            return RedirectToAction("Index", "NhanVien");
        }

        public ActionResult Logout()
        {
            Session["User"] = null;
            return RedirectToAction("DangNhap", "LoginAdmin");
        }
    }
}