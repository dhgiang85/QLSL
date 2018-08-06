using QLSL.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLSL.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private UnitOfWork uOW = new UnitOfWork();

        // GET: Report
        public ActionResult Index()
        {
            return View();
        }
    }
}