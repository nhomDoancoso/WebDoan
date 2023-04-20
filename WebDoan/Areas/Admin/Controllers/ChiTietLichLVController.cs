using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDoan.Models;

namespace WebDoan.Areas.Admin.Controllers
{
    public class ChiTietLichLVController : Controller
    {
        // GET: Admin/ChiTietLichLV
        myDataContextDataContext db = new myDataContextDataContext();
        public ActionResult Index()
        {
            return View();
        }
    }
}