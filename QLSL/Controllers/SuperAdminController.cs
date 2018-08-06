using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace QLSL.Controllers
{
    [Authorize(Roles = "SuperAdmins")]
    public class SuperAdminController : Controller
    {
        // GET: SuperAdmin

        public ActionResult Index()
        {
            return View();
        }
    }
}