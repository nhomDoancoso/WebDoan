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
    public class NhanVienController : Controller
    {
        // GET: Admin/NhanVien
        myDataContextDB db = new myDataContextDB();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Chat(FormCollection collection)
        {
            var tendangnhap = collection["UserName"];
            var matkhau = collection["Password"];
            var email = collection["Email"];

            NHANVIEN nv = db.NHANVIEN.FirstOrDefault(x => x.UserName == tendangnhap && x.Password == matkhau);
            if (nv != null)
            {
                ViewBag.ThongBao = "Chúc mừng đăng nhập thà nh công";
                Session["TenNv"] = nv.TenNV;
                Session["TaiKhoan"] = nv.MaNV;
                Session["User"] = nv.UserName;
                Session["Image"] = nv.HinhNV;

            }
            return View();
        }

        public ActionResult LstNhanVien(int? page, string SearchString)
        {
            int pageSize = 8;
            int pageNum = page ?? 1;
            var SearchAll = db.NHANVIEN.OrderBy(s => s.TenNV);
            var SearchSp = db.NHANVIEN.OrderBy(m => m.TenNV).Where(sp => sp.TenNV.ToUpper().Contains(SearchString.ToUpper()));
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
            ViewBag.MaCV = new SelectList(db.CHUCVU, "MaCV", "TenCV");
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection, NHANVIEN nv)
        {
            var hoten = collection["TenNV"];
            var dienthoai = collection["SDT"];
            var diachi = collection["DiaChi"];
            var email = collection["Email"];
            var User = collection["UserName"];
            var Pass = collection["Password"];
            var MatKhauXacNhan = collection["MatKhauXacNhan"];
            var chucvu = int.Parse(collection["MaCV"]);
            string temp = dienthoai;
            //    char check = temp[0];
            Regex regexMail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match matchMail = regexMail.Match(email);
            Regex regexPhone = new Regex(@"^(84|0[3|5|7|8|9])+([0-9]{8})\b");
            Match matchPhone = regexPhone.Match(dienthoai);
            Regex regexPass = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*?[0-9])(?=.*?[@!#%])[A-Za-z0-9!#%]{8,32}$");
            Match matchPassword = regexPass.Match(Pass);
            var checkUserl = db.NHANVIEN.FirstOrDefault(x => x.UserName == User);
            if(string.IsNullOrEmpty(dienthoai))
            {
                ViewData["!sdt"] = "Số điện thoại không được trống";
                return this.Create();
            }

            if (string.IsNullOrEmpty(diachi))
            {
                ViewData["!diachi"] = "địa chỉ không được trống";
                return this.Create();
            }

            if (string.IsNullOrEmpty(Pass))
            {
                ViewData["!pass"] = "mật khẩu không được trống";
                return this.Create();
            } 

            if (string.IsNullOrEmpty(email))
            {
                ViewData["!email"] = "email không được trống";
                return this.Create();
            }

            if (string.IsNullOrEmpty(User))
            {
                ViewData["!user"] = "user không được trống";
                return this.Create();
            }

            if (string.IsNullOrEmpty(hoten))
            {
                ViewData["Name"] = "Họ tên không được để trống";
                return this.Create();
            }
            if (checkUserl != null)
            {
                ViewData["UserExist"] = "user đăng nhập đã tồn tại";
                return this.Create();
            }
            if (!matchPhone.Success)
            {
                ViewData["NumWrong"] = "số điện thoải phải đúng định dạng";
                return this.Create();
            }
            if (!matchMail.Success)
            {
                ViewData["EmailWrong"] = "Email phải đúng định dạng";
                return this.Create();
            }
            if (!matchPassword.Success)
            {
                ViewData["PassWordWrong"] = "phải có chữ viết hoa, viết thường, ký tự đặc biệt, phải có số, nhập đủ 8 đến 32 ký tự";
                return this.Create();
            }
            if (String.IsNullOrEmpty(MatKhauXacNhan))
            {
                ViewData["NhapXNMK"] = "Phải nhập mật khẩu xác nhận!";
            }
            else
            {
                if (!Pass.Equals(MatKhauXacNhan))
                {
                    ViewData["MatKhauGiongNhau"] = "Mật khẩu và mật khẩu xác nhận phải giống nhau";
                }
                else
                {
                    nv.TenNV = hoten;
                    nv.SDT = dienthoai;
                    nv.DiaChi = diachi;
                    nv.Email = email;
                    nv.SDT = dienthoai;
                    nv.MaCV = chucvu;

                    db.NHANVIEN.Add(nv);
                    db.SaveChanges();

                    return RedirectToAction("LstNhanVien");
                }
            }
            return this.Create();
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

        public ActionResult Detail(int id)
        {
            var listNv = db.NHANVIEN.Where(m => m.MaNV == id).First();
            return View(listNv);
        }

        public ActionResult Edit(int id)
        {
            ViewBag.MaCV = new SelectList(db.CHUCVU, "MaCV", "TenCV");
            var listNv = db.NHANVIEN.First(m => m.MaNV == id);
            return View(listNv);
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var ENv = db.NHANVIEN.First(m => m.MaNV == id);
            var E_hinhNV = collection["HinhNV"];
            var E_tenNV = collection["TenNV"];
            var E_SDT = collection["SDT"];
            var E_DiaChi = collection["DiaChi"];
            var E_Email = collection["Email"];
            var User = collection["UserName"];
            var Pass = collection["Password"];
            ENv.MaNV = id;
            if (string.IsNullOrEmpty(E_tenNV))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                ENv.TenNV = E_tenNV;
                ENv.SDT = E_SDT;
                ENv.DiaChi = E_DiaChi;
                ENv.Email = E_Email;
                ENv.UserName = User;
                ENv.Password = Pass;
                UpdateModel(ENv);
                db.SaveChanges();
                return RedirectToAction("LstNhanVien");
            }
            return this.Edit(id);
        }
        public ActionResult Delete(int id)
        {
            var listNv = db.NHANVIEN.First(m => m.MaNV == id);
            return View(listNv);
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var listNv = db.NHANVIEN.Where(m => m.MaNV == id).First();
            db.NHANVIEN.Remove(listNv);
            db.SaveChanges();
            return RedirectToAction("LstNhanVien");
        }
        public ActionResult Logout()
        {
            Session["User"] = null;
            return RedirectToAction("LstNhanVien", "NhanVien");
        }

    }
}