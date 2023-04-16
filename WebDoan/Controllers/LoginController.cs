﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using WebDoan.Models;

namespace WebDoan.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        myDataContextDB db = new myDataContextDB();
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult DangNhap()
        {
            // var matkhau = MaHoaMd5.GetHashMD5(collection["Password"]);
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {
            var tendangnhap = collection["UserName"];
            var matkhau = collection["Password"];
            var email = collection["Email"];

            KHACHHANG khachang = db.KHACHHANG.FirstOrDefault(x => x.UserName == tendangnhap && x.Password == matkhau);

            if (khachang != null)
            {
                ViewBag.ThongBao = "Chúc mừng đăng nhập thà nh công";
                Session["User"] = khachang.TenKH;
                Session["User"] = khachang.UserName;
                Session["TaiKhoanKH"] = khachang.MaKH;
                Session["FullTaiKhoan"] = khachang;

            }
            else if (khachang == null)
            {
                ViewData["ErrorAccount"] = "sai mật khẩu hoặc Tên đăng nhập không tồn tại vui lòng nhập lại";
                return this.DangNhap();
            }
            else
            {
                ViewData["ErrorPass"] = "Mật khẩu không đúng";
                return this.DangNhap();
            }
            return RedirectToAction("Index", "Footer");
        }

        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(FormCollection collection, KHACHHANG kh)
        {
            var hoten = collection["TenKH"];
            var dienthoai = collection["SDT"].ToString();
            var email = collection["Email"];
            var diachi = collection["DiaChi"];
            var tendangnhap = collection["UserName"];
            var matkhau = collection["Password"];
            var MatKhauXacNhan = collection["MatKhauXacNhan"];
            string temp = dienthoai;
            Regex regexMail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match matchMail = regexMail.Match(email);
            Regex regexPhone = new Regex(@"^(84|0[3|5|7|8|9])+([0-9]{8})\b");
            Match matchPhone = regexPhone.Match(dienthoai);
            Regex regexPass = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*?[0-9])(?=.*?[@!#%])[A-Za-z0-9!#%]{8,32}$");
            Match matchPassword = regexPass.Match(matkhau);
            var checkEmail = db.KHACHHANG.FirstOrDefault(x => x.Email == email);
           
            if (string.IsNullOrEmpty(hoten))
            {
                ViewData["Name"] = "Họ tên không được để trống";
                return this.DangKy();
            }
            if(string.IsNullOrEmpty(dienthoai))
            {
                ViewData["dienthoai"] = "So dt không được để trống";
                return this.DangKy();
            }
            if (string.IsNullOrEmpty(tendangnhap))
            {
                ViewData["tendangnhap"] = "ten dang nhap không được để trống";
                return this.DangKy();
            }
            if (string.IsNullOrEmpty(matkhau))
            {
                ViewData["matkhau"] = "mat khau không được để trống";
                return this.DangKy();
            }
            if (string.IsNullOrEmpty(email))
            {
                ViewData["email"] = "email không được để trống";
                return this.DangKy();
            }
            if (checkEmail != null)
            {
                ViewData["UserExist"] = "email đã tồn tại, vui long nha email khac";
                return this.DangKy();
            }
            if (!matchPhone.Success)
            {
                ViewData["NumWrong"] = "Num phải đúng định dạng";
                return this.DangKy();
            }
            if (!matchMail.Success)
            {
                ViewData["EmailWrong"] = "Email phải đúng định dạng";
                return this.DangKy();
            }
            if (!matchPassword.Success)
            {
                ViewData["PassWrong"] = "phải có chữ viết hoa, viết thường, ký tự đặc biệt, phải có số, nhập đủ 8 đến 32 ký tự";
                return this.DangKy();
            }
            if (String.IsNullOrEmpty(MatKhauXacNhan))
            {
                ViewData["NhapMKXN"] = "Phải nhập mật khẩu xác nhận";
            }
            else
            {
                if (!matkhau.Equals(MatKhauXacNhan))
                {
                    ViewData["MatKhauGiongNhau"] = "Mật khẩu và mật khẩu xác nhận khong giong nhau";
                }
                else
                {
                    kh.TenKH = hoten;
                    kh.SDT = dienthoai;
                    kh.Email = email;
                    kh.UserName = tendangnhap;
                    kh.Password = matkhau;
                    db.KHACHHANG.Add(kh);
                    db.SaveChanges();
                    return RedirectToAction("DangNhap", "Login");
                }
            }
            return RedirectToAction("DangNhap", "Login");
        }

        public ActionResult Logout()
        {
            Session["User"] = null;
            return RedirectToAction("DangNhap", "Login");
        }
    }
}