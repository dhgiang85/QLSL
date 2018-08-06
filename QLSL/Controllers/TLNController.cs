using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using QLSL.Models;
using QLSL.DAL;
using QLSL.ViewModels;


namespace QLSL.Controllers
{
    [Authorize]
    public class TLNController : Controller
    {
        private UnitOfWork uOW = new UnitOfWork();

        //
        // GET: /TLNode/
        public ActionResult Report()
        {
           var data = (from Node in uOW.TLNodeRepository.Get(orderBy: q => q.OrderBy(s => s.Name))
                   from SignalPlan in
                       uOW.TLSignalPlanRepository.Get(filter: x => x.TLNodeID == Node.TLNodeID,
                           orderBy: q => q.OrderByDescending(x => x.DateOccur)).DefaultIfEmpty()
                   from NodeStatus in
                       uOW.TLNodeHitoryStatusRepository.Get(filter: x => x.TLNodeID == Node.TLNodeID && x.Processed==false,
                           orderBy: q => q.OrderByDescending(x => x.DateOccur)).DefaultIfEmpty()
                   select new TrafficLightNodeReport 
                   {
                       NodeName = Node.Name,
                       LabelMarker = Node.LabelMarker,
                       Lat = Node.Lat,
                       Lng = Node.Lng,
                       SignalPlanCurrent = (SignalPlan != null ? (SignalPlan.Unforced ? "unforced" : SignalPlan.SignalPlanChanged) : "unforced"),
                       TimeUpdateSignalPlan = (SignalPlan != null ? (SignalPlan.Unforced ? SignalPlan.DateUpdate : SignalPlan.DateOccur) : (DateTime?)null),
                       StatusID = (NodeStatus!=null ? NodeStatus.TLNodeStatusID : 1),
                       NodeStatus = (NodeStatus != null ? NodeStatus.TLNodeStatus.Name : "Bình thường"),
                       TimeUpdateStatus = (NodeStatus != null ? NodeStatus.DateUpdate : (DateTime?)null)
                   }).ToList();

           var data2 = (from i in data
                      group i by i.NodeName into grp
                       select new TrafficLightNodeReport
                       {
                           NodeName = grp.Key,
                           LabelMarker = grp.First().LabelMarker,
                           Lat = grp.First().Lat,
                           Lng = grp.First().Lng,
                           SignalPlanCurrent = grp.First().SignalPlanCurrent,
                           TimeUpdateSignalPlan = grp.First().TimeUpdateSignalPlan,
                           StatusID = grp.First().StatusID,
                           NodeStatus = grp.First().NodeStatus,
                           TimeUpdateStatus = grp.First().TimeUpdateStatus
                       }).ToList();
            return View(data2);
        }
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, int? pageListSize, bool? allEvent)
        {

            IEnumerable<TLNodeHitoryStatus> tlns;
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
            ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "Date_asc" : "";
            ViewBag.StatusParm = sortOrder == "StatusTLS" ? "StatusTLS_desc" : "StatusTLS";
            TempData["CurrentFilter"] = searchString;
            TempData["CurrentSort"] = sortOrder;
            TempData["allEvent"] = allEvent;
            if (!String.IsNullOrEmpty(searchString))
            {
                tlns = uOW.TLNodeHitoryStatusRepository.Get(filter: s => s.TLNode.Name.ToUpper().Contains(searchString.ToUpper())
                                                                           || s.TLNodeStatus.Name.ToUpper().Contains(searchString.ToUpper())
                                                                           || s.ContactName.ToUpper().Contains(searchString.ToUpper())
                                                                           || s.Details.ToUpper().Contains(searchString.ToUpper())
                                                                           || s.TLNode.LabelMarker.ToUpper().Contains(searchString.ToUpper()),
                    includeProperties: "TLNode,TLNodeStatus",
                    orderBy: s => s.OrderByDescending(x => x.DateOccur));
            }
            else
            {
                tlns = uOW.TLNodeHitoryStatusRepository.Get(orderBy: s => s.OrderByDescending(x => x.DateOccur),
                includeProperties: "TLNode,TLNodeStatus");
            }
            if (allEvent == null || allEvent == false) 
            {
                tlns = tlns.Where(x => !x.Processed);
            }
            
            switch (sortOrder)
            {
                case "StatusTLS":
                    tlns = tlns.OrderBy(s => s.TLNodeStatus.Name);
                    break;
                case "StatusTLS_desc":
                    tlns = tlns.OrderByDescending(s => s.TLNodeStatus.Name);
                    break;
                case "Note":
                    tlns = tlns.OrderBy(s => s.TLNode.Name);
                    break;
                case "Note_desc":
                    tlns = tlns.OrderByDescending(s => s.TLNode.Name);
                    break;
                case "Date_asc":
                    tlns = tlns.OrderBy(s => s.DateOccur);
                    break;
                default:
                    tlns = tlns.OrderByDescending(s => s.DateOccur);
                    break;
            }
            int pageSize = (pageListSize ?? 20);
            int pageNumber = (page ?? 1);
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            return View(tlns.ToPagedList(pageNumber, pageSize));
        }
        //public ActionResult UpdateTable(string sortOrder, string searchString, int? page)

        public ActionResult UpdateTable(int? page,int? pageListSize)
        {
            IEnumerable<TLNodeHitoryStatus> tlns;
            string searchString = Convert.ToString(TempData["CurrentFilter"]);
            string sortOrder = Convert.ToString(TempData["CurrentSort"]);
            bool allEvent = Convert.ToBoolean(TempData["allEvent"]);
            TempData["CurrentFilter"] = searchString;
            TempData["CurrentSort"] = sortOrder;
            TempData["allEvent"] = allEvent;
            if (!String.IsNullOrEmpty(searchString))
            {
                tlns = uOW.TLNodeHitoryStatusRepository.Get(filter: s => s.TLNode.Name.ToUpper().Contains(searchString.ToUpper())
                                                                          || s.TLNodeStatus.Name.ToUpper().Contains(searchString.ToUpper())
                                                                          || s.ContactName.ToUpper().Contains(searchString.ToUpper())
                                                                          || s.Details.ToUpper().Contains(searchString.ToUpper())
                                                                          || s.TLNode.LabelMarker.ToUpper().Contains(searchString.ToUpper()),
                   includeProperties: "TLNode,TLNodeStatus",
                   orderBy: s => s.OrderByDescending(x => x.DateOccur));
            }
            else
            {
                tlns = uOW.TLNodeHitoryStatusRepository.Get(orderBy: s => s.OrderByDescending(x => x.DateOccur),
                includeProperties: "TLNode,TLNodeStatus");
            }
            if (allEvent == null || allEvent == false)
            {
                tlns = tlns.Where(x => !x.Processed);
            }
            switch (sortOrder)
            {
                case "StatusTLS":
                    tlns = tlns.OrderBy(s => s.TLNodeStatus.Name);
                    break;
                case "StatusTLS_desc":
                    tlns = tlns.OrderByDescending(s => s.TLNodeStatus.Name);
                    break;
                case "Note":
                    tlns = tlns.OrderBy(s => s.TLNode.Name);
                    break;
                case "Note_desc":
                    tlns = tlns.OrderByDescending(s => s.TLNode.Name);
                    break;
                case "Date_asc":
                    tlns = tlns.OrderBy(s => s.DateOccur);
                    break;
                default:
                    tlns = tlns.OrderByDescending(s => s.DateOccur);
                    break;
            }
            int pageSize = (pageListSize ?? 20);
            int pageNumber = (page ?? 1);
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            return PartialView("_UpdateTableTLN", tlns.ToPagedList(pageNumber, pageSize));
        }



        //
        // GET: /TLNode/Details/5



        //
        // GET: /TLNode/Create
        [Authorize(Roles = "Operator")]
        public ActionResult Create()
        {
            ViewBag.TLNodeID = new SelectList(uOW.TLNodeRepository.Get(filter: q => q.Disable == false, orderBy:q=>q.OrderBy(x=>x.Name)), "TLNodeID", "Name");
            ViewBag.TLNodeStatusID = new SelectList(uOW.TLNodeStatusRepository.Get(filter:x=>x.TLNodeStatusID!=1), "TLNodeStatusID", "Name");
            return PartialView("_CreateTLNS");
        }

        //
        // POST: /TLNode/Create
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Operator")]
        public ActionResult Create([Bind(Include = "DateOccur,TLNodeID,TLNodeStatusID,OperatorName,ContactName,Details")]
            TLNodeHitoryStatus tlNodeHitories)
        {
            if (ModelState.IsValid)
            {
                tlNodeHitories.DateCreate = DateTime.Now;
                tlNodeHitories.DateUpdate = DateTime.Now;

                uOW.TLNodeHitoryStatusRepository.Insert(tlNodeHitories);
                uOW.Save();
                return Json(new { success = true, message = "Created Successfully." });
            }

            ViewBag.TLNodeID = new SelectList(uOW.TLNodeRepository.Get(filter: q => q.Disable == false, orderBy: q => q.OrderBy(x => x.Name)), "TLNodeID", "Name", tlNodeHitories.TLNodeID);
            ViewBag.TLNodeStatusID = new SelectList(uOW.TLNodeStatusRepository.Get(filter: x => x.TLNodeStatusID != 1), "TLNodeStatusID", "Name", tlNodeHitories.TLNodeStatusID);
            return PartialView("_CreateTLNS", tlNodeHitories);
        }

        //
        // GET: /TLNode/Edit/5
        [Authorize(Roles = "Operator")]
        public ActionResult Edit(int id = 0)
        {
            
            TLNodeHitoryStatus tlNodeHitoryStatus = uOW.TLNodeHitoryStatusRepository.GetByID(id);
            if (tlNodeHitoryStatus == null)
            {
                return HttpNotFound();
            }
            //ViewBag.TLNodeID = new SelectList(uOW.TLNodeRepository.Get(orderBy: q => q.OrderBy(x => x.Name)), "TLNodeID", "Name", tlNodeHitoryStatus.TLNodeID);
            ViewBag.TLNodeStatusID = new SelectList(uOW.TLNodeStatusRepository.Get(filter: x => x.TLNodeStatusID != 1), "TLNodeStatusID", "Name", tlNodeHitoryStatus.TLNodeStatusID);
            ViewBag.TLNodeID = uOW.TLNodeRepository.GetByID(tlNodeHitoryStatus.TLNodeID).Name;
            //ViewBag.TLNodeStatusID = uOW.TLNodeStatusRepository.GetByID(tlNodeHitoryStatus.TLNodeStatusID).Name;

            return PartialView("_EditTLNS", tlNodeHitoryStatus);
        }

        //
        // POST: /TLNode/Edit/5
        
        [HttpPost]
        [Authorize(Roles = "Operator")]
        public ActionResult Edit(TLNodeHitoryStatus tlnhs, string submit)
        {
            if (ModelState.IsValid)
            {
                
                if (submit == "Finish")
                {
                    tlnhs.Processed = true;
                    tlnhs.DateUpdate = DateTime.Now;
                }
                uOW.TLNodeHitoryStatusRepository.Update(tlnhs);
                uOW.Save();
                return Json(new { success = true, message = "Updated Successfully." });
            }
            //ViewBag.TLNodeID = new SelectList(uOW.TLNodeRepository.Get(orderBy: q => q.OrderBy(x => x.Name)), "TLNodeID", "Name", tlNodeHitoryStatus.TLNodeID);
            ViewBag.TLNodeStatusID = new SelectList(uOW.TLNodeStatusRepository.Get(filter: x => x.TLNodeStatusID != 1), "TLNodeStatusID", "Name", tlnhs.TLNodeStatusID);
            ViewBag.TLNodeID = uOW.TLNodeRepository.GetByID(tlnhs.TLNodeID).Name;
            //ViewBag.TLNodeStatusID = uOW.TLNodeStatusRepository.GetByID(tlnhs.TLNodeStatusID).Name;
            return PartialView("_EditTLNS", tlnhs);
        }

      
        //[HttpPost]
        //public ActionResult Edit(
        //     [Bind(Include = "TLNodeHitoryStatusID, ContactName, OperatorName, Note, Processed," +
        //                     "TLNodeID,TLNodeStatusID, DateUpdate, DateCreate, DateOccur")]
        //    TLNodeHitoryStatus tlNodeHitoryStatus)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        tlNodeHitoryStatus.DateUpdate = DateTime.Now;
        //        if (!string.IsNullOrEmpty(Request["Finish"]))
        //        {
        //            tlNodeHitoryStatus.Processed = true;
        //        }
        //        uOW.TLNodeHitoryStatusRepository.Update(tlNodeHitoryStatus);
        //        uOW.Save();
        //        return Json(new { success = true, message = "Updated Successfully." });
        //    }
        //    //ViewBag.TLNodeID = new SelectList(uOW.TLNodeRepository.Get(orderBy: q => q.OrderBy(x => x.Name)), "TLNodeID", "Name", tlNodeHitoryStatus.TLNodeID);
        //    //ViewBag.TLNodeStatusID = new SelectList(uOW.TLNodeStatusRepository.Get(filter: x => x.TLNodeStatusID != 1), "TLNodeStatusID", "Name", tlNodeHitoryStatus.TLNodeStatusID);
        //    return PartialView("_EditTLNS", tlNodeHitoryStatus);
        //}

        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection formCollection, string submit)
        //{

        //    TLNodeHitoryStatus tlnhs = uOW.TLNodeHitoryStatusRepository.GetByID(id);
        //    string str = Request.Form["submit"];
        //    if (TryUpdateModel(tlnhs, "",
        //                new string[] { "TLNodeHitoryStatusID", "ContactName", "OperatorName",
        //                    "Note", "Processed","TLNodeID","TLNodeStatusID", "DateUpdate", "DateCreate", "DateOccur"}))
        //    {
        //        try
        //        {
        //            tlnhs.DateUpdate = DateTime.Now;
        //            if (submit == "Finish")
        //            {
        //                tlnhs.Processed = true;
        //            }
        //            uOW.TLNodeHitoryStatusRepository.Update(tlnhs);
        //            uOW.Save();
        //        }
        //        catch (DataException /* dex */)
        //        {
        //            //Log the error (uncomment dex variable name after DataException and add a line here to write a log. 
        //            ModelState.AddModelError("",
        //                "Unable to save changes. Try again, and if the problem persists see your system administrator.");
        //        }
        //        return Json(new { success = true, message = "Updated Successfully." });
        //    }
        //    //ViewBag.TLNodeID = new SelectList(uOW.TLNodeRepository.Get(orderBy: q => q.OrderBy(x => x.Name)), "TLNodeID", "Name", tlNodeHitoryStatus.TLNodeID);
        //    //ViewBag.TLNodeStatusID = new SelectList(uOW.TLNodeStatusRepository.Get(filter: x => x.TLNodeStatusID != 1), "TLNodeStatusID", "Name", tlNodeHitoryStatus.TLNodeStatusID);
        //    return PartialView("_EditTLNS", tlnhs);
        //}
        



        // GET: /TLNode/Delete/5
        [Authorize(Roles = "Admin,TeamLeader")]
        public ActionResult DeleteConfirmed(int tlnsID)
        {
            try
            {
                
                var rslt =  uOW.TLNodeHitoryStatusRepository.GetByID(tlnsID);
                if (rslt != null)
                {
                    uOW.TLNodeHitoryStatusRepository.Delete(rslt);
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
        public ActionResult TLNSDetails(int? tlnsID)
        {
            try
            {
                TLNodeHitoryStatus TLNS = uOW.TLNodeHitoryStatusRepository.GetByID(tlnsID);
                
                var rslt = new
                {
                    NoteTLNS = TLNS.TLNode.Name,
                    Time = TLNS.DateOccur.ToString("dd-MM-yy hh:mm tt"),
                    Status = TLNS.TLNodeStatus.Name

                };
                return Json(rslt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
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