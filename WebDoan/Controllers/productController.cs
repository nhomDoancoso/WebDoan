using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDoan.Models;

namespace WebDoan.Controllers
{
    public class productController : Controller
    {
        // GET: SanPham
        myDataContextDataContext db = new myDataContextDataContext();

        public ActionResult Index()
        {
            //var lstSp = from ss in db.SANPHAMs select ss;
            var lstSp = db.SANPHAMs.Where(ss => ss.SoLuong > 0).ToList();
            return View(lstSp);
        }

        public ActionResult Detail(int id)
        {
            var D_SP = db.SANPHAMs.Where(m => m.MaSP == id).First();
            return View(D_SP);
        }
    }
}