using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            //var lst = from ss in db.PHIEUDATs where ss;
            return View(lst);   
        }
        public ActionResult Confirm(int id)
        {

            var dbContext = new myDataContextDataContext();
            var phieudat = dbContext.PHIEUDATs.Where(p => p.MaPD == id).FirstOrDefault();

            if (phieudat != null)
            {
                phieudat.TrangThaiPhieuDat = !phieudat.TrangThaiPhieuDat;
                dbContext.SubmitChanges(); 
            }

            return RedirectToAction("Index", "ClientBooking", new { id = id });
          
        }
    }
}