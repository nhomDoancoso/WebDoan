using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebDoan.Models;

namespace WebDoan.Areas.Admin.Controllers
{
    public class APIController : ApiController
    {
        myDataContextDataContext db = new myDataContextDataContext();

        [HttpPost]
        public IHttpActionResult UpdateBooleanValue(int id)
        {
            var recordToUpdate = db.PHIEUDATs.Single(t => t.MaPD == id);
            recordToUpdate.TrangThaiPhieuDat = true; // Đặt giá trị của trường boolean thành true
            db.SubmitChanges();
            return Ok();
        }

    }
}
