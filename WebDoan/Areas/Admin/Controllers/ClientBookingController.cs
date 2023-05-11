using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using WebDoan.Models;

namespace WebDoan.Areas.Admin.Controllers
{
    public class ClientBookingController : Controller
    {
        // GET: Admin/ClientBooking
        myDataContextDataContext db = new myDataContextDataContext();

        public ActionResult Index()
        {
            var lst = from ss in db.PHIEUDATs select ss;
            return View(lst);
        }


    }
}