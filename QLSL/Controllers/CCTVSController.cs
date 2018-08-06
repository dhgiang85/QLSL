
using System;

using System.Collections.Generic;

using System.Linq;
using System.Web.Mvc;
using PagedList;
using QLSL.DAL;
using QLSL.Models;
using QLSL.ViewModels;
using System.Globalization;

namespace QLSL.Controllers
{
    [Authorize]
    public class CCTVSController : Controller
    {
        //
        // GET: /CCTVS/

        private UnitOfWork uOW = new UnitOfWork();

        public ActionResult UpdateTable(int? page, int? pageListSize)
        {

            IEnumerable<CCTVStatus> cctvss;

            string searchString = Convert.ToString(TempData["CurrentFilter"]);
            string sortOrder = Convert.ToString(TempData["CurrentSort"]);
            bool allEvent = Convert.ToBoolean(TempData["allEvent"]);

            TempData["CurrentFilter"] = searchString;
            TempData["CurrentSort"] = sortOrder;
            TempData["allEvent"] = allEvent;
        
            
            if (!String.IsNullOrEmpty(searchString))
            {
                cctvss = uOW.CCTVStatusRepository.Get(filter: s => s.CCTV.Name.ToUpper().Contains(searchString.ToUpper())
                                                                           || s.CCTVError.Name.ToUpper().Contains(searchString.ToUpper())
                                                                           || s.ContactName.ToUpper().Contains(searchString.ToUpper())
                                                                           || s.Details.ToUpper().Contains(searchString.ToUpper()),
                    includeProperties: "CCTV,CCTVError",
                    orderBy: s => s.OrderByDescending(x => x.DateOccur));
            }
            else
            {
                cctvss = uOW.CCTVStatusRepository.Get(orderBy: s => s.OrderByDescending(x => x.DateOccur),
                includeProperties: "CCTV,CCTVError");
            }
            if (allEvent == null || allEvent == false)
            {
                cctvss = cctvss.Where(x => !x.Processed);
            }
            switch (sortOrder)
            {
                case "Error":
                    cctvss = cctvss.OrderBy(s => s.CCTVError.Name);
                    break;
                case "Error_desc":
                    cctvss = cctvss.OrderByDescending(s => s.CCTVError.Name);
                    break;

                case "Date_asc":
                    cctvss = cctvss.OrderBy(s => s.DateOccur);
                    break;
                default:
                    cctvss = cctvss.OrderByDescending(s => s.DateOccur);
                    break;
            }
            int pageSize = (pageListSize ?? 20);
            int pageNumber = (page ?? 1);
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            return PartialView("_UpdateTable", cctvss.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, int? pageListSize, bool? allEvent)
        {

            IEnumerable<CCTVStatus> cctvss;
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
                cctvss = uOW.CCTVStatusRepository.Get(filter: s => s.CCTV.Name.ToUpper().Contains(searchString.ToUpper())
                                                                           || s.CCTVError.Name.ToUpper().Contains(searchString.ToUpper())
                                                                           || s.ContactName.ToUpper().Contains(searchString.ToUpper())
                                                                           || s.Details.ToUpper().Contains(searchString.ToUpper()),
                    includeProperties: "CCTV,CCTVError",
                    orderBy: s => s.OrderByDescending(x => x.DateOccur));
            }
            else
            {
                cctvss = uOW.CCTVStatusRepository.Get(orderBy: s => s.OrderByDescending(x => x.DateOccur),
                includeProperties: "CCTV,CCTVError");
            }
            if (allEvent == null || allEvent == false)
            {
                cctvss = cctvss.Where(x => !x.Processed);
            }
            switch (sortOrder)
            {
                case "Error":
                    cctvss = cctvss.OrderBy(s => s.CCTVError.Name);
                    break;
                case "Error_desc":
                    cctvss = cctvss.OrderByDescending(s => s.CCTVError.Name);
                    break;
                
                case "Date_asc":
                    cctvss = cctvss.OrderBy(s => s.DateOccur);
                    break;
                default:
                    cctvss = cctvss.OrderByDescending(s => s.DateOccur);
                    break;
            }
            int pageSize = (pageListSize ?? 20);
            int pageNumber = (page ?? 1);
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            return View(cctvss.ToPagedList(pageNumber, pageSize));
        }

        [Authorize(Roles = "Operator")]
        public ActionResult Create()
        {
            ViewBag.CCTVID = new SelectList(uOW.CCTVRepository.Get(filter: q => q.Disable == false), "CCTVID", "Name");
            ViewBag.CCTVErrorID = new SelectList(uOW.CCTVErrorRepository.dbSet, "CCTVErrorID", "Name");
            return PartialView("_Create");
        }

        
        [HttpPost]
        [Authorize(Roles = "Operator")]
        public ActionResult Create([Bind(Include = "CCTVErrorID,Details,DateOccur,ContactName,OperatorName")] CCTVStatus cctvs,
            string[] CCTVID)
        {

            if (CCTVID == null)
            {
                ModelState.AddModelError("", "Chọn CAM Báo lỗi");
                //return Json(new { success = false, message = "Create Unsuccessfully." });    
            }
            if (ModelState.IsValid)
            {
                foreach (var cctvID in CCTVID)
                {
                    var id = int.Parse(cctvID);
                    var cctvsToADD = new CCTVStatus();
                    cctvsToADD.CCTVID = id;
                    cctvsToADD.CCTVErrorID = cctvs.CCTVErrorID;
                    cctvsToADD.Details = cctvs.Details;
                    cctvsToADD.DateOccur = cctvs.DateOccur;
                    cctvsToADD.OperatorName = cctvs.OperatorName;
                    cctvsToADD.ContactName = cctvs.ContactName;
                    cctvsToADD.DateCreate = DateTime.Now;
                    cctvsToADD.DateUpdate = DateTime.Now;
                    uOW.CCTVStatusRepository.Insert(cctvsToADD);
                }
                
                uOW.Save();
                return Json(new { success = true, message = "Created Successfully." });    
            }
            ViewBag.CCTVID = new SelectList(uOW.CCTVRepository.Get(filter: q => q.Disable == false), "CCTVID", "Name", cctvs.CCTVID);
            ViewBag.CCTVErrorID = new SelectList(uOW.CCTVErrorRepository.dbSet, "CCTVErrorID", "Name",cctvs.CCTVErrorID);
            return PartialView("_Create");
        }
        public ActionResult GetDetails(int? id)
        {
            try
            {
                CCTVStatus cctvs = uOW.CCTVStatusRepository.Get(filter: x => x.CCTVStatusID == id,
                                includeProperties: "CCTV,CCTVError").FirstOrDefault();

                var rslt = new
                {
                    Name = cctvs.CCTV.Name,
                    Time = cctvs.DateOccur.ToString("dd-MM-yy HH:mm"),
                    Error = cctvs.CCTVError.Name
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

                CCTVStatus rslt = uOW.CCTVStatusRepository.Get(filter: x => x.CCTVStatusID == id,
                                includeProperties: "CCTV,CCTVError").Single();
                if (rslt != null)
                {
                    uOW.CCTVStatusRepository.Delete(rslt);
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
        [Authorize(Roles = "Operator")]
        public ActionResult Edit(int id = 0)
        {

            CCTVStatus cctvs = uOW.CCTVStatusRepository.GetByID(id);
            if (cctvs == null)
            {
                return HttpNotFound();
            }
            ViewBag.CCTVID = uOW.CCTVRepository.GetByID(cctvs.CCTVID).Name;
            //ViewBag.CCTVErrorID = uOW.CCTVErrorRepository.GetByID(cctvs.CCTVErrorID).Name;
            //ViewBag.CCTVID = new SelectList(uOW.CCTVRepository.dbSet, "CCTVID", "Name", cctvs.CCTVID);
            ViewBag.CCTVErrorID = new SelectList(uOW.CCTVErrorRepository.dbSet, "CCTVErrorID", "Name", cctvs.CCTVErrorID);
            return PartialView("_Edit", cctvs);
        }

        //
        // POST: /TLNode/Edit/5

       
        [HttpPost]
        [Authorize(Roles = "Operator")]
        public ActionResult Edit(CCTVStatus cctvs, string submit)
        {
            if (ModelState.IsValid)
            {

                if (submit == "Finish")
                {
                    cctvs.Processed = true;
                    cctvs.DateUpdate = DateTime.Now;
                }
                uOW.CCTVStatusRepository.Update(cctvs);
                uOW.Save();
                return Json(new { success = true, message = "Updated Successfully." });
            }
            ViewBag.CCTVID = uOW.CCTVRepository.GetByID(cctvs.CCTVID).Name;
            //ViewBag.CCTVErrorID = uOW.CCTVErrorRepository.GetByID(cctvs.CCTVID).Name;
            //ViewBag.CCTVID = new SelectList(uOW.CCTVRepository.dbSet, "CCTVID", "Name", cctvs.CCTVID);
            ViewBag.CCTVErrorID = new SelectList(uOW.CCTVErrorRepository.dbSet, "CCTVErrorID", "Name", cctvs.CCTVErrorID);
            return PartialView("_Edit", cctvs);
        }

        public ActionResult Report()
        {
            DateTime DateStart;
            DateTime DateEnd;
            
                DateTime date = DateTime.Now;
                DateStart = new DateTime(date.Year, date.Month, 1);
                DateEnd = date;
         

            ViewBag.dateStart = string.Format("{0:dd-MM-yyyy HH:mm}", DateStart);
            ViewBag.dateEnd = string.Format("{0:dd-MM-yyyy HH:mm}", DateEnd);
            ViewBag.CCTVErrorID = new SelectList(uOW.CCTVErrorRepository.dbSet, "Name", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult GetReport(string dateStart, string dateEnd)
        {
            var For = Request.Form;
            DateTime DateStart = DateTime.ParseExact(dateStart, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);
            DateTime DateEnd = DateTime.ParseExact(dateEnd, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);
            var Start = Request.Form.GetValues("start").FirstOrDefault();
            var Length = Request.Form.GetValues("length").FirstOrDefault();

            var Draw = Request.Form.GetValues("draw").FirstOrDefault();


            var CAMName = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();
            var Error = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();
            var ContactName = Request.Form.GetValues("columns[2][search][value]").FirstOrDefault();
            var Details = Request.Form.GetValues("columns[3][search][value]").FirstOrDefault();

            var Items = uOW.CCTVStatusRepository
                .Get(filter: s =>
                    s.CCTV.Name.ToUpper().Contains(CAMName)
                    && s.ContactName.ToUpper().Contains(ContactName)
                    && (Details != "" ? s.Details.ToUpper().Contains(Details) : true)
                    && s.CCTVError.Name.ToUpper().Contains(Error)
                    && s.DateOccur >= DateStart && s.DateOccur<= DateEnd
                    )
                .Select(z => new CCTVSReportData
                {
                    CAMName = z.CCTV.Name,
                    ContactName = z.ContactName,
                    Error = z.CCTVError.Name,
                    Details = z.Processed ? (z.Details + "<br/><i class='pull-right small text-muted'>" + z.DateUpdate.ToString("dd-MM-yyyy HH:mm") + ":Đã xử lý</i> <br/>") : z.Details,
                    DateOccur = z.DateOccur,
                });

            var sortOrder = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][data]").FirstOrDefault();
            var SortDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            int PageSize = Length != null ? Convert.ToInt32(Length) : 20;
            int Skip = Start != null ? Convert.ToInt32(Start) : 0;
           
            switch (sortOrder)
            {
                case "CAMName":
                    Items = SortDir =="asc" ? Items.OrderBy(s => s.CAMName): Items.OrderByDescending(s => s.CAMName);
                    break;
                case "ContactName":
                    Items = SortDir == "asc" ? Items.OrderBy(s => s.ContactName) : Items.OrderByDescending(s => s.ContactName);
                    break;
                case "Error":
                    Items = SortDir == "asc" ? Items.OrderBy(s => s.Error) : Items.OrderByDescending(s => s.Error);
                    break;
                case "Details":
                    Items = SortDir == "asc" ? Items.OrderBy(s => s.Details) : Items.OrderByDescending(s => s.Details);
                    break;
      
                case "DateOccur":
                    Items = SortDir == "asc" ? Items.OrderBy(s => s.DateOccur) : Items.OrderByDescending(s => s.DateOccur);
                    break;
            }
            int TotalRecords = 0;   
            TotalRecords = Items.ToList().Count();
            var NewItems = Items.Skip(Skip).Take(PageSize == -1 ? TotalRecords : PageSize).ToList();


            return Json(new { draw = Draw, recordsFiltered = TotalRecords, recordsTotal = TotalRecords, data = NewItems }, JsonRequestBehavior.AllowGet);
        
        }
        protected override void Dispose(bool disposing)
        {
            uOW.Dispose();
            base.Dispose(disposing);
        }

    }
}
