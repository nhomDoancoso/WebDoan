﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDoan.Models;

namespace WebDoan.Controllers
{
    public class FooterController : Controller
    {
        // GET: Footer
        myDataContextDataContext db = new myDataContextDataContext();
        public ActionResult Index()
        {
            var lstDv = from ss in db.DICHVUs select ss;
            return View(lstDv);
        }
    }
}