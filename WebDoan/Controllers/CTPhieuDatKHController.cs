using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDoan.Models;

namespace WebDoan.Controllers
{
    public class CTPhieuDatKHController : Controller
    {
        // GET: CTPhieuDatKH
        myDataContextDataContext db = new myDataContextDataContext();
        public ActionResult Index()
        {
            var listPhieuDat = db.PHIEUDATs.Where(p => p.NHANVIEN.MaNV == (int)Session["TaiKhoanKH"]).ToList();
            return View();
        }
    }
}