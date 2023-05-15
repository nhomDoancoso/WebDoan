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
            return View(lst);   
        }
        public ActionResult Confirm(int id)
        {
            var dbContext = new myDataContextDataContext();
            var phieudat = dbContext.PHIEUDATs.Where(p => p.MaPD == id).FirstOrDefault();

            if (phieudat != null)
            {
                phieudat.TrangThaiPhieuDat = !phieudat.TrangThaiPhieuDat; // toggle the TrangThaiPhieuDat property value
                dbContext.SubmitChanges(); // save the changes to the database
            }

            // redirect to the same view that displays the item details
            return RedirectToAction("Index", new { id = id });
          
        }
        //[HttpPost]
        //public ActionResult AddFavorite(int id)
        //{
        //    string result = "fail";
        //    int userID = int.Parse(Session["UserID"].ToString());
        //    Favorite favorite = context.Favorites.FirstOrDefault(f => f.UserID == userID && f.DishID == id);
        //    if (favorite == null)
        //    {

        //        Favorite newFavor = new Favorite
        //        {
        //            DishID = id,
        //            UserID = userID,
        //        };
        //        context.Favorites.Add(newFavor);
        //        context.SaveChanges();
        //        result = "fav";
        //    }
        //    else
        //    {
        //        context.Favorites.Remove(favorite);
        //        context.SaveChanges();
        //        result = "unfav";

        //    }
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}
    }
}