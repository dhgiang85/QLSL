using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using QLSL.DAL;
using QLSL.Models;
using QLSL.ViewModels;

namespace QLSL.Controllers
{
    [Authorize]
    public class TAccidentController : Controller
    {
        private UnitOfWork uOW = new UnitOfWork();
        //
        // GET: /TAccident/
        public ActionResult UpdateTable(int? page, int? pageListSize)
        {
            IEnumerable<TAccident> tas;

            string searchString = Convert.ToString(TempData["CurrentFilter"]);
            string sortOrder = Convert.ToString(TempData["CurrentSort"]);
            TempData["CurrentFilter"] = searchString;
            TempData["CurrentSort"] = sortOrder;

            if (!String.IsNullOrEmpty(searchString))
            {
                tas = uOW.TAccidentRepository.Get(
                    filter: s => s.Note.ToUpper().Contains(searchString.ToUpper())
                        || s.AccidentType.Name.ToUpper().Contains(searchString.ToUpper())
                        || s.ContactName.ToUpper().Contains(searchString.ToUpper())
                        || s.Details.ToUpper().Contains(searchString.ToUpper()),
                    orderBy: s => s.OrderByDescending(x => x.DateOccur),
                    includeProperties: "AccidentType");

            }
            else
            {
                tas = uOW.TAccidentRepository.Get(orderBy: s => s.OrderByDescending(x => x.DateOccur),
                includeProperties: "AccidentType");
            }
            switch (sortOrder)
            {
                case "Type":
                    tas = tas.OrderBy(s => s.AccidentType.Name);
                    break;
                case "Type_desc":
                    tas = tas.OrderByDescending(s => s.AccidentType.Name);
                    break;
                case "Detail":
                    tas = tas.OrderBy(s => s.Details);
                    break;
                case "Detail_desc":
                    tas = tas.OrderByDescending(s => s.Details);
                    break;
                case "Contact":
                    tas = tas.OrderBy(s => s.ContactName);
                    break;
                case "Contact_desc":
                    tas = tas.OrderByDescending(s => s.ContactName);
                    break;
                case "Note":
                    tas = tas.OrderBy(s => s.Note);
                    break;
                case "Note_desc":
                    tas = tas.OrderByDescending(s => s.Note);
                    break;
                case "Date_asc":
                    tas = tas.OrderBy(s => s.DateOccur);
                    break;
                default:
                    tas = tas.OrderByDescending(s => s.DateOccur);
                    break;
            }
            int pageSize = (pageListSize ?? 20);
            int pageNumber = (page ?? 1);
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            return PartialView("_UpdateTable", tas.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page,int? pageListSize)
        {
            
            IEnumerable<TAccident> tas;
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
            ViewBag.NoteSortParm = sortOrder == "Note" ? "Note_desc" : "Note";
            ViewBag.DetailSortParm = sortOrder == "Detail" ? "Detail_desc" : "Detail";
            ViewBag.ContactSortParm = sortOrder == "Contact" ? "Contact_desc" : "Contact";
            ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "Date_asc" : "";
            ViewBag.TypeSortParm = sortOrder == "Type" ? "Type_desc" : "Type";
            TempData["CurrentFilter"] = searchString;
            TempData["CurrentSort"] = sortOrder;
            
            if (!String.IsNullOrEmpty(searchString))
            {
               
                tas = uOW.TAccidentRepository.Get(
                    filter:s => s.Note.ToUpper().Contains(searchString.ToUpper())
                        || s.AccidentType.Name.ToUpper().Contains(searchString.ToUpper())
                        || s.ContactName.ToUpper().Contains(searchString.ToUpper())        
                        || s.Details.ToUpper().Contains(searchString.ToUpper()),
                        
                    orderBy: s => s.OrderByDescending(x => x.DateOccur),
                    includeProperties: "AccidentType");
                
            }
            else
            {
                tas = uOW.TAccidentRepository.Get(orderBy: s => s.OrderByDescending(x => x.DateOccur),
                includeProperties: "AccidentType");
            }
            switch (sortOrder)
            {
                case "Type":
                    tas = tas.OrderBy(s => s.AccidentType.Name);
                    break;
                case "Type_desc":
                    tas = tas.OrderByDescending(s => s.AccidentType.Name);
                    break;
                case "Detail":
                    tas = tas.OrderBy(s => s.Details);
                    break;
                case "Detail_desc":
                    tas = tas.OrderByDescending(s => s.Details);
                    break;
                case "Contact":
                    tas = tas.OrderBy(s => s.ContactName);
                    break;
                case "Contact_desc":
                    tas = tas.OrderByDescending(s => s.ContactName);
                    break;
                case "Note":
                    tas = tas.OrderBy(s => s.Note);
                    break;
                case "Note_desc":
                    tas = tas.OrderByDescending(s => s.Note);
                    break;
                case "Date_asc":
                    tas = tas.OrderBy(s => s.DateOccur);
                    break;
                default:
                    tas = tas.OrderByDescending(s => s.DateOccur);
                    break;
            }
            int pageSize = (pageListSize ?? 20);
            int pageNumber = (page ?? 1);
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            return View(tas.ToPagedList(pageNumber, pageSize));
        }

        [Authorize(Roles = "Operator")]
        public ActionResult Create()
        {
            ViewBag.AccidentTypeID = new SelectList(uOW.AccidentTypeRepository.Get(orderBy: q => q.OrderBy(x => x.Name)), "AccidentTypeID", "Name");
            return PartialView("_Create");
        }

        //
        // POST: /TLNode/Create

        
        [HttpPost]
        [Authorize(Roles = "Operator")]
        public ActionResult Create([Bind(Include = "DateOccur,AccidentTypeID,Details,ContactName,OperatorName, Note")]
            TAccident ta)
        {
            if (ModelState.IsValid)
            {
                ta.DateCreate = DateTime.Now;
                ta.DateUpdate = DateTime.Now;
                
                uOW.TAccidentRepository.Insert(ta);
                uOW.Save();
                return Json(new { success = true, message = "Created Successfully." });
            }

            ViewBag.AccidentTypeID = new SelectList(uOW.AccidentTypeRepository.Get(orderBy: q => q.OrderBy(x => x.Name)), "AccidentTypeID", "Name", ta.AccidentTypeID);
            return PartialView("_Create", ta);
        }

        public ActionResult GetDetails(int? id)
        {
            try
            {
                TAccident ta = uOW.TAccidentRepository.GetByID(id);

                var rslt = new
                {
                    Time = ta.DateOccur.ToString("dd-MM-yy hh:mm tt"),
                    Type = ta.AccidentType.Name,
                    Details = ta.Details

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

                var rslt = uOW.TAccidentRepository.GetByID(id);
                if (rslt != null)
                {
                    uOW.TAccidentRepository.Delete(rslt);
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
            TAccident ta = uOW.TAccidentRepository.GetByID(id);
            if (ta == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccidentTypeID = new SelectList(uOW.AccidentTypeRepository.Get(orderBy: q => q.OrderBy(x => x.Name)), "AccidentTypeID", "Name", ta.AccidentTypeID);
            //ViewBag.AccidentTypeID = uOW.AccidentTypeRepository.GetByID(ta.AccidentTypeID).Name;

            return PartialView("_Edit", ta);
        }

        //
        // POST: /TLSignalPlan/Edit/5
        
        [HttpPost]
        [Authorize(Roles = "Operator")]
        public ActionResult Edit(TAccident ta)
        {
            if (ModelState.IsValid)
            {
                ta.DateUpdate = DateTime.Now;
                uOW.TAccidentRepository.Update(ta);
                uOW.Save();
                return Json(new { success = true, message = "Updated Successfully." });
            }
            ViewBag.AccidentTypeID = new SelectList(uOW.AccidentTypeRepository.Get(orderBy: q => q.OrderBy(x => x.Name)), "AccidentTypeID", "Name", ta.AccidentTypeID);
            //ViewBag.AccidentTypeID = uOW.AccidentTypeRepository.GetByID(ta.AccidentTypeID).Name;

            return PartialView("_Edit", ta);
        }

        public ActionResult Report(string dateStart, string dateEnd)
        {
            DateTime DateStart;
            DateTime DateEnd;
            DateTime DateStartM;
            DateTime DateEndM;
            if (String.IsNullOrEmpty(dateStart) || (String.IsNullOrEmpty(dateStart)))
            {
                DateTime date = DateTime.Now;
                DateStart = new DateTime(date.Year, date.Month, 1);
                DateEnd = date;
            }
            else
            {
                DateStart = DateTime.ParseExact(dateStart, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);
                DateEnd= DateTime.ParseExact(dateEnd, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);
            }
            DateStartM =  DateStart.AddMonths(-1);
            DateEndM = DateStart.AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
            ViewBag.dateStart = string.Format("{0:dd-MM-yyyy HH:mm}", DateStart);
            ViewBag.dateEnd = string.Format("{0:dd-MM-yyyy HH:mm}", DateEnd);
            var data = (from accidentType in uOW.AccidentTypeRepository.Get(orderBy: q => q.OrderBy(s => s.Name))
                        from taccident in
                            uOW.TAccidentRepository.Get(filter: x => x.AccidentTypeID == accidentType.AccidentTypeID 
                                        && x.DateOccur>=DateStart&& x.DateOccur<=DateEnd)
                                group accidentType by accidentType.Name into grp
                       select new AccidentReport
                        {
                            AccidentType = grp.Key,
                            sum = grp.Count()
                        }).ToList();
            var data2 = (from accidentType in uOW.AccidentTypeRepository.Get(orderBy: q => q.OrderBy(s => s.Name))
                        from taccident in
                            uOW.TAccidentRepository.Get(filter: x => x.AccidentTypeID == accidentType.AccidentTypeID
                                        && x.DateOccur >= DateStartM && x.DateOccur <= DateEndM)
                        group accidentType by accidentType.Name into grp
                        select new AccidentReport
                        {
                            AccidentType = grp.Key,
                            sum = grp.Count()
                        }).ToList();
            Accident2Report report = new Accident2Report();

            report.Report1 = data;
            report.Report2 = data2;
            ViewBag.tas =
                uOW.TAccidentRepository.Get(filter: x => x.DateOccur >= DateStart && x.DateOccur <= DateEnd);
            return View(report);
        }

        protected override void Dispose(bool disposing)
        {
            uOW.Dispose();
            base.Dispose(disposing);
        }
    }
}
