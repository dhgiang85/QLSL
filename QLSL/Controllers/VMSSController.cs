
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using QLSL.DAL;
using QLSL.Models;

namespace QLSL.Controllers
{
    [Authorize]
    public class VMSSController : Controller
    {
        //
        // GET: /VMSS/
        private UnitOfWork uOW = new UnitOfWork();

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page,
            int? pageListSize, bool? allEvent)
        {
            IEnumerable<VMSHistoryStatus> vmshs;
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
            ViewBag.NoteSortParm = sortOrder == "Note" ? "Note_desc" : "Note";
            ViewBag.VMSSortParm = sortOrder == "VMS" ? "VMS_desc" : "VMS";
            ViewBag.DetailSortParm = sortOrder == "Detail" ? "Detail_desc" : "Detail";
            ViewBag.ContactSortParm = sortOrder == "Contact" ? "Contact_desc" : "Contact";
            ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "Date_asc" : "";
            ViewBag.TypeSortParm = sortOrder == "Type" ? "Type_desc" : "Type";
            TempData["CurrentFilter"] = searchString;
            TempData["CurrentSort"] = sortOrder;
            TempData["allEvent"] = allEvent;
            if (!String.IsNullOrEmpty(searchString))
            {
                vmshs = uOW.VMSHistoryStatusRepository.Get(
                    filter: s => s.Note.ToUpper().Contains(searchString.ToUpper())
                                 || s.VMS.Name.ToUpper().Contains(searchString.ToUpper())
                                 || s.VMSStatus.Name.ToUpper().Contains(searchString.ToUpper())
                                 || s.ContactName.ToUpper().Contains(searchString.ToUpper())
                                 || s.Details.ToUpper().Contains(searchString.ToUpper()),
                    orderBy: s => s.OrderByDescending(x => x.DateOccur),
                    includeProperties: "VMS,VMSStatus");
            }
            else
            {
                vmshs = uOW.VMSHistoryStatusRepository.Get(orderBy: s => s.OrderByDescending(x => x.DateOccur),
                    includeProperties: "VMS,VMSStatus");
            }
            if (allEvent == null || allEvent == false)
            {
                vmshs = vmshs.Where(x => !x.Processed);
            }
            switch (sortOrder)
            {
                case "VMS":
                    vmshs = vmshs.OrderBy(s => s.VMS.Name);
                    break;
                case "VMS_desc":
                    vmshs = vmshs.OrderByDescending(s => s.VMS.Name);
                    break;
                case "Type":
                    vmshs = vmshs.OrderBy(s => s.VMSStatus.Name);
                    break;
                case "Type_desc":
                    vmshs = vmshs.OrderByDescending(s => s.VMSStatus.Name);
                    break;
                case "Detail":
                    vmshs = vmshs.OrderBy(s => s.Details);
                    break;
                case "Detail_desc":
                    vmshs = vmshs.OrderByDescending(s => s.Details);
                    break;
                case "Contact":
                    vmshs = vmshs.OrderBy(s => s.ContactName);
                    break;
                case "Contact_desc":
                    vmshs = vmshs.OrderByDescending(s => s.ContactName);
                    break;
                case "Note":
                    vmshs = vmshs.OrderBy(s => s.Note);
                    break;
                case "Note_desc":
                    vmshs = vmshs.OrderByDescending(s => s.Note);
                    break;
                case "Date_asc":
                    vmshs = vmshs.OrderBy(s => s.DateOccur);
                    break;
                default:
                    vmshs = vmshs.OrderByDescending(s => s.DateOccur);
                    break;
            }
            int pageSize = (pageListSize ?? 20);
            int pageNumber = (page ?? 1);
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            return View(vmshs.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult UpdateTable(int? page, int? pageListSize)
        {
            IEnumerable<VMSHistoryStatus> vmshs;

            string searchString = Convert.ToString(TempData["CurrentFilter"]);
            string sortOrder = Convert.ToString(TempData["CurrentSort"]);
            bool allEvent = Convert.ToBoolean(TempData["allEvent"]);
            TempData["CurrentFilter"] = searchString;
            TempData["CurrentSort"] = sortOrder;
            TempData["allEvent"] = allEvent;

            if (!String.IsNullOrEmpty(searchString))
            {
                vmshs = uOW.VMSHistoryStatusRepository.Get(
                    filter: s => s.Note.ToUpper().Contains(searchString.ToUpper())
                                 || s.VMS.Name.ToUpper().Contains(searchString.ToUpper())
                                 || s.VMSStatus.Name.ToUpper().Contains(searchString.ToUpper())
                                 || s.ContactName.ToUpper().Contains(searchString.ToUpper())
                                 || s.Details.ToUpper().Contains(searchString.ToUpper()),
                    orderBy: s => s.OrderByDescending(x => x.DateOccur),
                    includeProperties: "VMS,VMSStatus");
            }
            else
            {
                vmshs = uOW.VMSHistoryStatusRepository.Get(orderBy: s => s.OrderByDescending(x => x.DateOccur),
                    includeProperties: "VMS,VMSStatus");
            }
            if (allEvent == null || allEvent == false)
            {
                vmshs = vmshs.Where(x => !x.Processed);
            }

            switch (sortOrder)
            {
                case "VMS":
                    vmshs = vmshs.OrderBy(s => s.VMS.Name);
                    break;
                case "VMS_desc":
                    vmshs = vmshs.OrderByDescending(s => s.VMS.Name);
                    break;
                case "Type":
                    vmshs = vmshs.OrderBy(s => s.VMSStatus.Name);
                    break;
                case "Type_desc":
                    vmshs = vmshs.OrderByDescending(s => s.VMSStatus.Name);
                    break;
                case "Detail":
                    vmshs = vmshs.OrderBy(s => s.Details);
                    break;
                case "Detail_desc":
                    vmshs = vmshs.OrderByDescending(s => s.Details);
                    break;
                case "Contact":
                    vmshs = vmshs.OrderBy(s => s.ContactName);
                    break;
                case "Contact_desc":
                    vmshs = vmshs.OrderByDescending(s => s.ContactName);
                    break;
                case "Note":
                    vmshs = vmshs.OrderBy(s => s.Note);
                    break;
                case "Note_desc":
                    vmshs = vmshs.OrderByDescending(s => s.Note);
                    break;
                case "Date_asc":
                    vmshs = vmshs.OrderBy(s => s.DateOccur);
                    break;
                default:
                    vmshs = vmshs.OrderByDescending(s => s.DateOccur);
                    break;
            }
            int pageSize = (pageListSize ?? 20);
            int pageNumber = (page ?? 1);
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            return PartialView("_UpdateTable", vmshs.ToPagedList(pageNumber, pageSize));
        }

        [Authorize(Roles = "Operator")]
        public ActionResult Create()
        {
            ViewBag.VMSID =
                new SelectList(
                    uOW.VMSRepository.Get(filter: q => q.Disable == false, orderBy: q => q.OrderBy(x => x.Name)),
                    "VMSID", "Name");
            ViewBag.VMSStatusID =
                new SelectList(
                    uOW.VMSStatusRepository.Get(filter: x => x.VMSStatusID != 1,
                        orderBy: q => q.OrderByDescending(x => x.Name)), "VMSStatusID", "Name");
            return PartialView("_Create");
        }

        [HttpPost]
        [Authorize(Roles = "Operator")]
        public ActionResult Create(
            [Bind(Include = "DateOccur,VMSID,VMSStatusID,Details,ContactName,OperatorName,Note")] VMSHistoryStatus vmshs)
        {
            if (ModelState.IsValid)
            {
                vmshs.DateCreate = DateTime.Now;
                vmshs.DateUpdate = DateTime.Now;
                uOW.VMSHistoryStatusRepository.Insert(vmshs);
                uOW.Save();
                return Json(new {success = true, message = "Created Successfully."});
            }


            ViewBag.VMSID =
                new SelectList(
                    uOW.VMSRepository.Get(filter: q => q.Disable == false, orderBy: q => q.OrderBy(x => x.Name)),
                    "VMSID", "Name", vmshs.VMSID);
            ViewBag.VMSStatusID =
                new SelectList(
                    uOW.VMSStatusRepository.Get(filter: x => x.VMSStatusID != 1,
                        orderBy: q => q.OrderByDescending(x => x.Name)), "VMSStatusID", "Name", vmshs.VMSStatusID);
            return PartialView("_Create", vmshs);
        }

        [Authorize(Roles = "Operator")]
        public ActionResult Edit(int id = 0)
        {
            VMSHistoryStatus vmshs = uOW.VMSHistoryStatusRepository.GetByID(id);
            if (vmshs == null)
            {
                return HttpNotFound();
            }
            //ViewBag.VMSID = new SelectList(uOW.VMSRepository.Get(orderBy: q => q.OrderBy(x => x.Name)), "VMSID", "Name", vmshs.VMSID);
            ViewBag.VMSStatusID = new SelectList(uOW.VMSStatusRepository.Get(orderBy: q => q.OrderBy(x => x.Name)),
                "VMSStatusID", "Name", vmshs.VMSStatusID);
            ViewBag.VMSID = uOW.VMSRepository.GetByID(vmshs.VMSID).Name;
            //ViewBag.VMSStatusID = uOW.VMSStatusRepository.GetByID(vmshs.VMSStatusID).Name;
            return PartialView("_Edit", vmshs);
        }

        //
        // POST: /TLSignalPlan/Edit/5

        [HttpPost]
        [Authorize(Roles = "Operator")]
        public ActionResult Edit(VMSHistoryStatus vmshs, string submit)
        {
            if (ModelState.IsValid)
            {
                if (submit == "Finish")
                {
                    vmshs.Processed = true;
                    vmshs.DateUpdate = DateTime.Now;
                }
                uOW.VMSHistoryStatusRepository.Update(vmshs);
                uOW.Save();
                return Json(new {success = true, message = "Updated Successfully."});
            }
            //ViewBag.VMSID = new SelectList(uOW.VMSRepository.Get(orderBy: q => q.OrderBy(x => x.Name)), "VMSID", "Name", vmshs.VMSID);
            ViewBag.VMSStatusID = new SelectList(uOW.VMSStatusRepository.Get(orderBy: q => q.OrderBy(x => x.Name)),
                "VMSStatusID", "Name", vmshs.VMSStatusID);
            ViewBag.VMSID = uOW.VMSRepository.GetByID(vmshs.VMSID).Name;
            //ViewBag.VMSStatusID = uOW.VMSStatusRepository.GetByID(vmshs.VMSStatusID).Name;
            return PartialView("_Edit", vmshs);
        }

        public ActionResult GetDetails(int? id)
        {
            try
            {
                VMSHistoryStatus vmshs = uOW.VMSHistoryStatusRepository.GetByID(id);

                var rslt = new
                {
                    Name = vmshs.VMS.Name,
                    Time = vmshs.DateOccur.ToString("dd-MM-yy HH:mm"),
                    Type = vmshs.VMSStatus.Name,
                    Details = vmshs.Details

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

                var rslt = uOW.VMSHistoryStatusRepository.GetByID(id);
                if (rslt != null)
                {
                    uOW.VMSHistoryStatusRepository.Delete(rslt);
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

       
    

    protected override void Dispose(bool disposing)
        {
            uOW.Dispose();
            base.Dispose(disposing);
        }
    }
}
