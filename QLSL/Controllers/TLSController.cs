using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using QLSL.Models;
using QLSL.DAL;


namespace QLSL.Controllers
{
    [Authorize]
    public class TLSController : Controller
    {
        private UnitOfWork uOW = new UnitOfWork();

        //
        // GET: /TLSignalPlan/

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, int? pageListSize, bool? allEvent)
        {
            IEnumerable<TLSignalPlan> trafficlightsignals;
                
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
            ViewBag.CurrentPlanSortParm = sortOrder == "CurrentPlan" ? "CurrentPlan_desc" : "CurrentPlan";
            ViewBag.ChangedPlanSortParm = sortOrder == "ChangedPlan" ? "ChangedPlan_desc" : "ChangedPlan";
            ViewBag.ReasonSortParm = sortOrder == "ReasonTLS" ? "Reason_desc" : "ReasonTLS";
            TempData["CurrentFilter"] = searchString;
            TempData["CurrentSort"] = sortOrder;
            TempData["allEvent"] = allEvent;
            if (!String.IsNullOrEmpty(searchString))
            {
                trafficlightsignals = uOW.TLSignalPlanRepository.Get(
                    filter: s => s.SignalPlanCurrent.ToUpper().Contains(searchString.ToUpper())
                                 || s.SignalPlanChanged.ToUpper().Contains(searchString.ToUpper())
                                 || s.ReasonChangeSP.Name.ToUpper().Contains(searchString.ToUpper())
                                 || s.OperatorName.ToUpper().Contains(searchString.ToUpper())
                                 || s.TLNode.Name.ToUpper().Contains(searchString.ToUpper())
                                 || s.Note.ToUpper().Contains(searchString.ToUpper())
                                 || s.TLNode.LabelMarker.ToUpper().Contains(searchString.ToUpper()),
                    orderBy: q => q.OrderByDescending(x => x.DateOccur), includeProperties: "TLNode,ReasonChangeSP");
            }
            else
            {
               trafficlightsignals =  uOW.TLSignalPlanRepository.Get(orderBy: q => q.OrderByDescending(x => x.DateOccur), includeProperties: "TLNode,ReasonChangeSP");
            }
            if (allEvent == null || allEvent == false)
            {
                trafficlightsignals = trafficlightsignals.Where(x => !x.Changed && !x.Unforced);
            }
            switch (sortOrder)
            {
                case "ReasonTLS":
                    trafficlightsignals = trafficlightsignals.OrderBy(s => s.ReasonChangeSP.Name);
                    break;
                case "Reason_desc":
                    trafficlightsignals = trafficlightsignals.OrderByDescending(s => s.ReasonChangeSP.Name);
                    break;

                case "ChangedPlan":
                    trafficlightsignals = trafficlightsignals.OrderBy(s => s.SignalPlanChanged);
                    break;
                case "ChangedPlan_desc":
                    trafficlightsignals = trafficlightsignals.OrderByDescending(s => s.SignalPlanChanged);
                    break;

                case "CurrentPlan":
                    trafficlightsignals = trafficlightsignals.OrderBy(s => s.SignalPlanCurrent);
                    break;
                case "CurrentPlan_desc":
                    trafficlightsignals = trafficlightsignals.OrderByDescending(s => s.SignalPlanCurrent);
                    break;

                case "Note":
                    trafficlightsignals = trafficlightsignals.OrderBy(s => s.TLNode.Name);
                    break;
                case "Note_desc":
                    trafficlightsignals = trafficlightsignals.OrderByDescending(s => s.TLNode.Name);
                    break;

                case "Date_asc":
                    trafficlightsignals = trafficlightsignals.OrderBy(s => s.DateOccur);
                    break;
                default:
                    trafficlightsignals = trafficlightsignals.OrderByDescending(s => s.DateOccur);
                    break;
            }

            int pageSize = (pageListSize ?? 20);
            int pageNumber = (page ?? 1);
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            return View(trafficlightsignals.ToPagedList(pageNumber,pageSize));
        }


        public ActionResult UpdateTable( int? page,int? pageListSize)
        {
            IEnumerable<TLSignalPlan> trafficlightsignals;
            string searchString = Convert.ToString(TempData["CurrentFilter"]);
            string sortOrder = Convert.ToString(TempData["CurrentSort"]);
            bool allEvent = Convert.ToBoolean(TempData["allEvent"]);
            TempData["CurrentFilter"] = searchString;
            TempData["CurrentSort"] = sortOrder;
            TempData["allEvent"] = allEvent;
            if (!String.IsNullOrEmpty(searchString))
            {
                trafficlightsignals = uOW.TLSignalPlanRepository.Get(
                    filter: s => s.SignalPlanCurrent.ToUpper().Contains(searchString.ToUpper())
                                 || s.SignalPlanChanged.ToUpper().Contains(searchString.ToUpper())
                                 || s.ReasonChangeSP.Name.ToUpper().Contains(searchString.ToUpper())
                                 || s.OperatorName.ToUpper().Contains(searchString.ToUpper())
                                 || s.TLNode.Name.ToUpper().Contains(searchString.ToUpper())
                                 || s.Note.ToUpper().Contains(searchString.ToUpper())
                                 || s.TLNode.LabelMarker.ToUpper().Contains(searchString.ToUpper()),
                    orderBy: q => q.OrderByDescending(x => x.DateOccur), includeProperties: "TLNode,ReasonChangeSP");
            }
            else
            {
                trafficlightsignals = uOW.TLSignalPlanRepository.Get(orderBy: q => q.OrderByDescending(x => x.DateOccur), includeProperties: "TLNode,ReasonChangeSP");
            }
            if (allEvent == null || allEvent == false)
            {
                trafficlightsignals = trafficlightsignals.Where(x => !x.Changed && !x.Unforced);
            }
            switch (sortOrder)
            {
                case "ReasonTLS":
                    trafficlightsignals = trafficlightsignals.OrderBy(s => s.ReasonChangeSP.Name);
                    break;
                case "Reason_desc":
                    trafficlightsignals = trafficlightsignals.OrderByDescending(s => s.ReasonChangeSP.Name);
                    break;

                case "ChangedPlan":
                    trafficlightsignals = trafficlightsignals.OrderBy(s => s.SignalPlanChanged);
                    break;
                case "ChangedPlan_desc":
                    trafficlightsignals = trafficlightsignals.OrderByDescending(s => s.SignalPlanChanged);
                    break;

                case "CurrentPlan":
                    trafficlightsignals = trafficlightsignals.OrderBy(s => s.SignalPlanCurrent);
                    break;
                case "CurrentPlan_desc":
                    trafficlightsignals = trafficlightsignals.OrderByDescending(s => s.SignalPlanCurrent);
                    break;

                case "Note":
                    trafficlightsignals = trafficlightsignals.OrderBy(s => s.TLNode.Name);
                    break;
                case "Note_desc":
                    trafficlightsignals = trafficlightsignals.OrderByDescending(s => s.TLNode.Name);
                    break;

                case "Date_asc":
                    trafficlightsignals = trafficlightsignals.OrderBy(s => s.DateOccur);
                    break;
                default:
                    trafficlightsignals = trafficlightsignals.OrderByDescending(s => s.DateOccur);
                    break;
            }

            int pageSize = (pageListSize ?? 20);
            int pageNumber = (page ?? 1);
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            return PartialView("_UpdateTableTLS", trafficlightsignals.ToPagedList(pageNumber, pageSize));
        }
        [Authorize(Roles = "Operator")]
        public ActionResult Create()
        {
            ViewBag.TLNodeID = new SelectList(uOW.TLNodeRepository.Get(filter:q=>q.Disable==false, orderBy:q=>q.OrderBy(s=>s.Name)), "TLNodeID", "Name");
            ViewBag.ReasonChangeSPID = new SelectList(uOW.ReasonChangeSPRepository.dbSet, "ReasonChangeSPID", "Name");
            var rslt =
                uOW.TLSignalPlanRepository.Get(filter: x => x.TLNodeID == 8,
                    orderBy: q => q.OrderByDescending(x => x.DateOccur)).FirstOrDefault();
            ViewBag.SignalPlan = (rslt != null) ? rslt.SignalPlanChanged : "";
            return PartialView("_CreateTLS");
        }

        //
        // POST: /TLSignalPlan/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Operator")]
        public ActionResult Create([Bind(Include = "DateOccur,TLNodeID," +
                    "SignalPlanCurrent,SignalPlanChanged,ReasonChangeSPID,Operatorname,Note")]TLSignalPlan trafficlightsignal)
        {
            
            if (ModelState.IsValid)
            {
                var tlns =
                    uOW.TLSignalPlanRepository.Get(filter: x => x.DateOccur > trafficlightsignal.DateOccur && x.TLNodeID==trafficlightsignal.TLNodeID)
                        .FirstOrDefault();
                if (tlns != null)
                {
                    ModelState.AddModelError(string.Empty,"Thời gian tạo pha không hợp lý");
                }
            }
            if (ModelState.IsValid)
            {
                var tlns =
                    uOW.TLSignalPlanRepository.Get(filter: x => x.DateOccur < trafficlightsignal.DateOccur && !x.Changed && !x.Unforced && x.TLNodeID == trafficlightsignal.TLNodeID
                                        , orderBy: q => q.OrderByDescending(x => x.DateOccur))
                        .FirstOrDefault();
                if (tlns != null)
                {
                    tlns.Changed = true;
                    tlns.DateUpdate = trafficlightsignal.DateOccur;
                    uOW.TLSignalPlanRepository.Update(tlns);
                }
                trafficlightsignal.DateCreate = DateTime.Now;
                trafficlightsignal.DateUpdate = DateTime.Now;
                uOW.TLSignalPlanRepository.Insert(trafficlightsignal);
                uOW.Save();
                return Json(new { success = true, message = "Created Successfully." });
            }
            var rslt =
                uOW.TLSignalPlanRepository.Get(filter: x => x.TLNodeID == trafficlightsignal.TLNodeID,
                    orderBy: q => q.OrderByDescending(x => x.DateOccur)).FirstOrDefault();
            ViewBag.SignalPlan = (rslt != null) ? rslt.SignalPlanChanged : "";
            ViewBag.TLNodeID = new SelectList(uOW.TLNodeRepository.Get(filter: q => q.Disable == false, orderBy: q => q.OrderBy(s => s.Name)), "TLNodeID", "Name", trafficlightsignal.TLNodeID);
            ViewBag.ReasonChangeSPID = new SelectList(uOW.ReasonChangeSPRepository.dbSet, "ReasonChangeSPID", "Name", trafficlightsignal.ReasonChangeSPID);
            return PartialView("_CreateTLS",trafficlightsignal);
        }


        // GET: /TLSignalPlan/Edit/5
        [Authorize(Roles = "Operator")]
        public ActionResult Edit(int id = 0)
        {
            TLSignalPlan trafficlightsignal = uOW.TLSignalPlanRepository.Get(filter: x => x.TLSignalPlanID == id, includeProperties: "TLNode,ReasonChangeSP").FirstOrDefault();
            if (trafficlightsignal == null)
            {
                return HttpNotFound();
            }
            //ViewBag.TLNodeID = new SelectList(uOW.TLNodeRepository.Get(orderBy: q => q.OrderBy(s => s.Name)), "TLNodeID", "Name", trafficlightsignal.TLNodeID);
            ViewBag.ReasonChangeSPID = new SelectList(uOW.ReasonChangeSPRepository.dbSet, "ReasonChangeSPID", "Name", trafficlightsignal.ReasonChangeSPID);
            ViewBag.TLNodeID = uOW.TLNodeRepository.GetByID(trafficlightsignal.TLNodeID).Name;
            //ViewBag.ReasonChangeSPID = uOW.ReasonChangeSPRepository.GetByID(trafficlightsignal.ReasonChangeSPID).Name;

            return PartialView("_EditTLS",trafficlightsignal);
        }

        //
        // POST: /TLSignalPlan/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Operator")]
        public ActionResult Edit(TLSignalPlan trafficlightsignal, string submit)
        {
            if (ModelState.IsValid)
            {
                if (submit == "Unforced")
                {
                    trafficlightsignal.Unforced = true;
                    trafficlightsignal.DateUpdate = DateTime.Now;
                }
                uOW.TLSignalPlanRepository.Update(trafficlightsignal);
                uOW.Save();
                return Json(new { success = true, message = "Updated Successfully." });
            }
            //ViewBag.TLNodeID = new SelectList(uOW.TLNodeRepository.Get(orderBy: q => q.OrderBy(s => s.Name)), "TLNodeID", "Name", trafficlightsignal.TLNodeID);
            ViewBag.ReasonChangeSPID = new SelectList(uOW.ReasonChangeSPRepository.dbSet, "ReasonChangeSPID", "Name", trafficlightsignal.ReasonChangeSPID);
            ViewBag.TLNodeID = uOW.TLNodeRepository.GetByID(trafficlightsignal.TLNodeID).Name;
            //ViewBag.ReasonChangeSPID = uOW.ReasonChangeSPRepository.GetByID(trafficlightsignal.ReasonChangeSPID).Name;
            return PartialView("_EditTLS", trafficlightsignal);
        }
      
        //
        // POST: /TLSignalPlan/Delete/5

        [Authorize(Roles = "Admin,TeamLeader")]
        public ActionResult DeleteConfirmed(int? tlsID)
        {
            try
            {
                var rslt = uOW.TLSignalPlanRepository.GetByID(tlsID);
                if (rslt != null)
                {
                    uOW.TLSignalPlanRepository.Delete(rslt);
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

        public ActionResult TLSDetails(int? tlsID)
        {
            try
            {
                TLSignalPlan tlSignalPlan = uOW.TLSignalPlanRepository.GetByID(tlsID); 
                var rslt = new 
                {
                    NoteTLS = tlSignalPlan.TLNode.Name,
                    Time = tlSignalPlan.DateOccur.ToString("dd-MM-yy hh:mm tt"),
                    CurrentPlan = tlSignalPlan.SignalPlanChanged

                };
                return Json(rslt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public JsonResult CurrentPlan(string noteID)
        {
            int id = Convert.ToInt32(noteID);
            var rslt =
                uOW.TLSignalPlanRepository.Get(filter: x => x.TLNodeID == id,
                    orderBy: q => q.OrderByDescending(x => x.DateOccur)).FirstOrDefault();
            string str = (rslt != null) ? rslt.SignalPlanChanged : "";

            return Json(str, JsonRequestBehavior.AllowGet);
        }
        protected override void Dispose(bool disposing)
        {
            uOW.Dispose();
            base.Dispose(disposing);
        }        
    }
}