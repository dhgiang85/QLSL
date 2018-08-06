using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using QLSL.DAL;
using QLSL.Models;

namespace QLSL.Controllers
{
    [Authorize]
    public class VMSController : Controller
    {
        private UnitOfWork uOW = new UnitOfWork();

        public ActionResult UpdateTable(int? page, int? pageListSize)
        {
            IEnumerable<VMS> vMSs;
            //var cctvs = uOW.CCTVRepository.dbSet;

            string searchString = Convert.ToString(TempData["CurrentFilter"]);
            string sortOrder = Convert.ToString(TempData["CurrentSort"]);
            bool allEQ = Convert.ToBoolean(TempData["allEQ"]);
            TempData["CurrentFilter"] = searchString;
            TempData["CurrentSort"] = sortOrder;
            TempData["allEQ"] = allEQ;
            if (!String.IsNullOrEmpty(searchString))
            {
                vMSs = uOW.VMSRepository.Get(
                    filter: s => s.Name.ToUpper().Contains(searchString.ToUpper())
                                 || s.IP.ToUpper().Contains(searchString.ToUpper())
                                 || s.Address.ToUpper().Contains(searchString.ToUpper()),
                    orderBy: s => s.OrderBy(x => x.Name)
                    );
            }
            else
            {
                vMSs = uOW.VMSRepository.Get(orderBy: s => s.OrderBy(x => x.Name));
            }

            if (allEQ == null || allEQ == false)
            {
                vMSs = vMSs.Where(x => !x.Disable);
            }

            switch (sortOrder)
            {

                case "Name_desc":
                    vMSs = vMSs.OrderByDescending(s => s.Name);
                    break;
                default:
                    vMSs = vMSs.OrderBy(s => s.Name);
                    break;
            }

            int pageSize = (pageListSize ?? 20);
            int pageNumber = (page ?? 1);
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            return PartialView("_UpdateTable", vMSs.ToPagedList(pageNumber, pageSize));
        }

        // GET: VMS

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page,int? pageListSize, bool? allEQ)
        {
            IEnumerable<VMS> vMSs;
            if (searchString != null)
            {
                page = 1;
                searchString = searchString.Trim();
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.AllEQ = allEQ;

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";
 
            TempData["CurrentSort"] = sortOrder;
            TempData["allEQ"] = allEQ;
            if (!String.IsNullOrEmpty(searchString))
            {
                vMSs = uOW.VMSRepository.Get(
                    filter: s => s.Name.ToUpper().Contains(searchString.ToUpper())
                                 || s.IP.ToUpper().Contains(searchString.ToUpper())
                                 || s.Address.ToUpper().Contains(searchString.ToUpper()),
                    orderBy: s => s.OrderBy(x => x.Name)
                    );
            }
            else
            {
                vMSs = uOW.VMSRepository.Get(orderBy: s => s.OrderBy(x => x.Name));
            }

            if (allEQ == null || allEQ == false)
            {
                vMSs = vMSs.Where(x => !x.Disable);
            }

            switch (sortOrder)
            {
                
                case "Name_desc":
                    vMSs = vMSs.OrderByDescending(s => s.Name);
                    break;
                default:
                    vMSs = vMSs.OrderBy(s => s.Name);
                    break;
            }

            int pageSize = (pageListSize ?? 20);
            int pageNumber = (page ?? 1);
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            return View(vMSs.ToPagedList(pageNumber, pageSize));
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return PartialView("_Create");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Name,Note,Address,IP,Disable,Map,Lat,Lng")]VMS vMs)
        {

            if (ModelState.IsValid)
            {

                uOW.VMSRepository.Insert(vMs);
                uOW.Save();
                return Json(new { success = true, message = "Created Successfully." });
            }


            return PartialView("_Create", vMs);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id = 0)
        {

            VMS vms = uOW.VMSRepository.Get(filter: x => x.VMSID == id).FirstOrDefault();
            if (vms == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Edit", vms);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(VMS vms)
        {

            if (ModelState.IsValid)
            {
                uOW.VMSRepository.Update(vms);
                uOW.Save();
                return Json(new { success = true, message = "Updated Successfully." });
            }
            return PartialView("_Edit", vms);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult GetDetails(int? id)
        {
            try
            {
                VMS vms = uOW.VMSRepository.GetByID(id);

                var rslt = new
                {
                    Name = vms.Name,
                    IP = vms.IP,
                    Address = vms.Address

                };
                return Json(rslt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return null;
            }
        }
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {

                var rslt = uOW.VMSRepository.GetByID(id);
                if (rslt != null)
                {
                    uOW.VMSRepository.Delete(rslt);
                    uOW.Save();
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return HttpNotFound();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}