using System;
using System.Collections.Generic;
using System.Linq;
using PagedList;
using System.Web.Mvc;
using QLSL.DAL;
using QLSL.Models;

namespace QLSL.Controllers
{
    [Authorize]
    public class TunnelController : Controller
    {
        private UnitOfWork uOW = new UnitOfWork();

        public ActionResult UpdateTable(int? page, int? pageListSize)
        {
            IEnumerable<TunnelError> tns;

            string searchString = Convert.ToString(TempData["CurrentFilter"]);
            string sortOrder = Convert.ToString(TempData["CurrentSort"]);
            bool allEvent = Convert.ToBoolean(TempData["allEvent"]);

            TempData["CurrentFilter"] = searchString;
            TempData["CurrentSort"] = sortOrder;
            TempData["allEvent"] = allEvent;


            if (!String.IsNullOrEmpty(searchString))
            {
                tns = uOW.TunnelErrorRepository.Get(filter: s => s.Details.ToUpper().Contains(searchString.ToUpper()),
                    orderBy: s => s.OrderByDescending(x => x.DateCreate));
            }
            else
            {
                tns = uOW.TunnelErrorRepository.Get(orderBy: s => s.OrderByDescending(x => x.DateCreate));

            }
            if (allEvent == null || allEvent == false)
            {
                tns = tns.Where(x => !x.Processed);
            }
            switch (sortOrder)
            {
                case "Error":
                    tns = tns.OrderBy(s => s.Details);
                    break;
                case "Error_desc":
                    tns = tns.OrderByDescending(s => s.Details);
                    break;

                case "Date_asc":
                    tns = tns.OrderBy(s => s.DateCreate);
                    break;
                default:
                    tns = tns.OrderByDescending(s => s.DateOccur);
                    break;
            }
            int pageSize = (pageListSize ?? 20);
            int pageNumber = (page ?? 1);
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            return PartialView("_UpdateTable", tns.ToPagedList(pageNumber, pageSize));
        }


        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page,
            int? pageListSize, bool? allEvent)
        {

            IEnumerable<TunnelError> tns;
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
            ViewBag.AllEvent = allEvent;

            ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "Date_asc" : "";
            ViewBag.ErrorSortParm = sortOrder == "Error" ? "Error_desc" : "Error";

            TempData["CurrentFilter"] = searchString;
            TempData["CurrentSort"] = sortOrder;
            TempData["allEvent"] = allEvent;

            if (!String.IsNullOrEmpty(searchString))
            {
                tns = uOW.TunnelErrorRepository.Get(filter: s => s.Details.ToUpper().Contains(searchString.ToUpper()),
                    orderBy: s => s.OrderByDescending(x => x.DateCreate));
            }
            else
            {
                tns = uOW.TunnelErrorRepository.Get(orderBy: s => s.OrderByDescending(x => x.DateCreate));
            }
            if (allEvent == null || allEvent == false)
            {
                tns = tns.Where(x => !x.Processed);
            }
            switch (sortOrder)
            {
                case "Error":
                    tns = tns.OrderBy(s => s.Details);
                    break;
                case "Error_desc":
                    tns = tns.OrderByDescending(s => s.Details);
                    break;

                case "Date_asc":
                    tns = tns.OrderBy(s => s.DateCreate);
                    break;
                default:
                    tns = tns.OrderByDescending(s => s.DateOccur);
                    break;
            }
            int pageSize = (pageListSize ?? 20);
            int pageNumber = (page ?? 1);
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            return View(tns.ToPagedList(pageNumber, pageSize));
        }

        [Authorize(Roles = "Operator")]
        public ActionResult Create()
        { 
            return PartialView("_Create");
        }


        [HttpPost]
        [Authorize(Roles = "Operator")]
        public ActionResult Create([Bind(Include = "Details,Note,DateCreate,DateOccur,ContactName,OperatorName")] TunnelError tns)
        {

            if (ModelState.IsValid)
            {
                tns.DateUpdate = DateTime.Now;
                uOW.TunnelErrorRepository.Insert(tns);
                uOW.Save();
                return Json(new { success = true, message = "Created Successfully." });
            }
            
            return PartialView("_Create");
        }
        [Authorize(Roles = "Operator")]
        public ActionResult Edit(int id = 0)
        {

            TunnelError tn = uOW.TunnelErrorRepository.GetByID(id);
            if (tn == null)
            {
                return HttpNotFound();
            }

            return PartialView("_Edit", tn);
        }
        [HttpPost]
        [Authorize(Roles = "Operator")]
        public ActionResult Edit(TunnelError tn, string submit)
        {
            if (ModelState.IsValid)
            {

                if (submit == "Finish")
                {
                    tn.Processed = true;
                    tn.DateUpdate = DateTime.Now;
                }
                uOW.TunnelErrorRepository.Update(tn);
                uOW.Save();
                return Json(new { success = true, message = "Updated Successfully." });
            }

            return PartialView("_Edit", tn);
        }
        public ActionResult GetDetails(int? id)
        {
            try
            {
                TunnelError tn = uOW.TunnelErrorRepository.GetByID(id);

                var rslt = new
                {
              
                    Time = tn.DateCreate.ToString("dd-MM-yy HH:mm"),
             
                    Details = tn.Details

                };
                return Json(rslt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return null;
            }
        }
        [Authorize(Roles = "Admin,TeamLeader")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {

                var rslt = uOW.TunnelErrorRepository.GetByID(id);
                if (rslt != null)
                {
                    uOW.TunnelErrorRepository.Delete(rslt);
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