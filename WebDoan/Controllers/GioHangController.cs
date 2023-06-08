using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Migrations;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using WebDoan.Models;

namespace WebDoan.Controllers
{
    public class GioHangController : Controller
    {
        myDataContextDataContext data = new myDataContextDataContext();
        public List<DonDatHang> Laygiohang()
        {
            //if (Session["TaiKhoanKH"] == null || Session["TaiKhoanKH"].ToString() == "")
            //{
            //    return RedirectToAction("DangNhap", "Login");
            //}
            int maKH = (int)Session["TaiKhoanKH"];
            List<DonDatHang> lstGiohang = Session["GioHang"] as List<DonDatHang>;
            var loadGH = data.DonDatHangs.Where(x => x.MaKH == maKH).ToList();
            lstGiohang = loadGH;
            if (lstGiohang == null)
            {
                lstGiohang = new List<DonDatHang>();
                Session["GioHang"] = lstGiohang;
            }
            else
            {
                foreach (DonDatHang gh in loadGH)
                {
                    gh.SANPHAM = (SANPHAM)data.SANPHAMs.FirstOrDefault(x => x.MaSP == gh.MaSP);
                    gh.KHACHHANG = (KHACHHANG)data.KHACHHANGs.FirstOrDefault(x => x.MaKH == gh.MaKH);
                    if (lstGiohang == null)
                    {
                        lstGiohang = new List<DonDatHang>();
                        lstGiohang.Add(gh);
                    }
                    else
                    {
                        if (!lstGiohang.Contains(gh))
                            lstGiohang.Add(gh);
                        else Session["GioHang"] = lstGiohang;
                    }
                }
            }
            return lstGiohang;

        }
        public ActionResult ThemGioHang(int id, string strURL)
        {
            if (Session["TaiKhoanKH"] == null || Session["TaiKhoanKH"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "Login");
            }
            int maTK = (int)Session["TaiKhoanKH"];
            List<DonDatHang> lstGiohang = Laygiohang();
            DonDatHang sanpham = lstGiohang.Find(s => s.MaSP == id);
            DonDatHang temp = new DonDatHang(id, maTK);
            if (sanpham == null)
            {
                sanpham = new DonDatHang(id);
                sanpham.MaKH = maTK;
                lstGiohang.Add(sanpham);
                temp.SoLuong = sanpham.SoLuong;
                data.DonDatHangs.InsertOnSubmit(temp);
                data.SubmitChanges();
                return Redirect(strURL);
            }
            else
            {
                var donDatHang = data.DonDatHangs.FirstOrDefault(x => x.MaKH == maTK);
                sanpham.SoLuong += 1;
                lstGiohang.Add(sanpham);
                donDatHang.SoLuong = sanpham.SoLuong;
                data.SubmitChanges();
                return Redirect(strURL);
            }
        }
        public int? TongSoLuong()
        {

            int? tsl = 0;
            List<DonDatHang> lstGiohang = Session["GioHang"] as List<DonDatHang>;
            if (lstGiohang != null)
            {
                tsl = lstGiohang.Sum(n => n.SoLuong);
            }
            return tsl;
        }
        public int TongSoLuongSanPham()
        {
            int tsl = 0;
            List<DonDatHang> lstGiohang = Session["GioHang"] as List<DonDatHang>;
            if (lstGiohang != null)
            {
                tsl = lstGiohang.Count;
            }
            return tsl;
        }

        private double? TongTien()
        {
            double? tt = 0;
            List<DonDatHang> lstGiohang = Session["GioHang"] as List<DonDatHang>;

            if (lstGiohang != null)
            {
                tt = lstGiohang.Sum(n => n.ThanhTien);
            }
            return tt;
        }

        public ActionResult GioHang()
        {
            List<DonDatHang> lstGiohang = Laygiohang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            ViewBag.TongSoLuongSanPham = TongSoLuongSanPham();
            return View(lstGiohang);
        }

        public ActionResult GioHangPartial()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            ViewBag.TongSoLuongSanPham = TongSoLuongSanPham();
            return PartialView();
        }

        public ActionResult XoaGioHang(int id)
        {
            List<DonDatHang> lstGiohang = Laygiohang();
            DonDatHang sanpham = lstGiohang.SingleOrDefault(n => n.MaSP == id);
            if (sanpham != null)
            {
                lstGiohang.RemoveAll(n => n.MaSP == id);
                data.DonDatHangs.DeleteOnSubmit(sanpham);
                data.SubmitChanges();
                return RedirectToAction("GioHang");
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult CapnhatGiohang(int id, FormCollection collection)
        {
            List<DonDatHang> lstGiohang = Laygiohang();
            DonDatHang gioHang = lstGiohang.SingleOrDefault(s => s.MaSP == id);
            SANPHAM sanPham = data.SANPHAMs.SingleOrDefault(sp => sp.MaSP == id);
            if (gioHang != null)
            {
                int sl = int.Parse(collection["txtSolg"].ToString());
                gioHang.SoLuong = sl;
                data.DonDatHangs.FirstOrDefault(s => s.MaSP == id);
                data.SubmitChanges();
                return RedirectToAction("GioHang");
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult XoaTatCaGiohang()
        {
            List<DonDatHang> lstGiohang = Laygiohang();
            lstGiohang.Clear();
            return RedirectToAction("GioHang");
        }
        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["TaiKhoanKH"] == null || Session["TaiKhoanKH"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "Login");
            }
            if (Session["Giohang"] == null)
            {
                return RedirectToAction("Index", "product");
            }
            List<DonDatHang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return View(lstGiohang);
        }

        public ActionResult DatHang(FormCollection collection)
        {

            HOADON dh = new HOADON();
            KHACHHANG kh = (KHACHHANG)Session["FullTaiKhoan"];
            SANPHAM s = new SANPHAM();
            List<DonDatHang> lstGiohang = Session["GioHang"] as List<DonDatHang>;
            dh.MaKH = kh.MaKH;
            dh.NgayLap = DateTime.Now;
            data.HOADONs.InsertOnSubmit(dh);
            data.SubmitChanges();
            foreach (var item in lstGiohang)
            {
                var temp = data.DonDatHangs.FirstOrDefault(x => x.MaKH == item.MaKH && x.MaSP == item.MaSP);
                data.DonDatHangs.DeleteOnSubmit(temp);
                CTHOADON ctdh = new CTHOADON();
                s = data.SANPHAMs.FirstOrDefault(x => x.MaSP == item.MaSP);
                ctdh.MaSP = item.MaSP;
                ctdh.MaHD = dh.MaHD;
                ctdh.SoLuongSanPham = item.SoLuong;
                //trừ số lượng
                s.SoLuong -= item.SoLuong;
                data.CTHOADONs.InsertOnSubmit(ctdh);
                data.SANPHAMs.FirstOrDefault(x => x.MaSP == item.MaSP);
                data.SubmitChanges();
            }
            data.SubmitChanges();
            Session["GioHang"] = null;
            return RedirectToAction("XacnhanDonhang", "GioHang");
        }


        public ActionResult XacnhanDonhang()
        {
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}