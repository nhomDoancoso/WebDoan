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
            var lstSp = from ss in db.SANPHAMs select ss;
            return View(lstSp);
        }
    }
}