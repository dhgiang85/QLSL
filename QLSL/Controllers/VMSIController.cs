using System;
using System.Collections.Generic;
using System.Data;
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
    public class VMSIController : Controller
    {
        //
        // GET: /VMSI/
        private UnitOfWork uOW = new UnitOfWork();

        public ActionResult Report(string dateStart, string dateEnd)
        {
            DateTime DateStart;
            DateTime DateEnd;
            if (String.IsNullOrEmpty(dateStart) || (String.IsNullOrEmpty(dateStart)))
            {
                DateTime date = DateTime.Now;
                DateStart = new DateTime(date.Year, date.Month, 1);
                DateEnd = date;
            }
            else
            {
                DateStart = DateTime.ParseExact(dateStart, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);
                DateEnd = DateTime.ParseExact(dateEnd, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);
            }

            ViewBag.dateStart = string.Format("{0:dd-MM-yyyy HH:mm}", DateStart);
            ViewBag.dateEnd = string.Format("{0:dd-MM-yyyy HH:mm}", DateEnd);
            var infs = uOW.InformationRepository.Get(
                filter: x => x.DateOccur >= DateStart && x.DateOccur <= DateEnd,
                includeProperties: "VMSs,InformationSource",
                orderBy:x=>x.OrderByDescending(d=>d.DateOccur));

            var data = (from information in infs
                        group information by information.InformationSource.Name into grp 
                        select new VMSReportData
                        {
                            Name = grp.Key,
                            Count = grp.Count()
                        }
                ).ToList();

            var data2 = new VMSReportData
                         {
                                Name= "VMS",
                                //Count=infs.Select(x => x.VMSs.Count).Sum()
                                Count = uOW.InformationVMSRepository
                                        .Get(filter: x => x.Information.DateOccur >= DateStart && x.Information.DateOccur <= DateEnd && x.Success)
                                        .Select(x=>x.VMSID).Count()
                                
                        };
            data.Insert(0,data2);
            
            ViewBag.Infs = infs;
            return View(data);
        }

        public ActionResult UpdateTable( int? page, int? pageListSize)
        {
            IEnumerable<Information> infs;

            string searchString = Convert.ToString(TempData["CurrentFilter"]);
            string sortOrder = Convert.ToString(TempData["CurrentSort"]);
            TempData["CurrentFilter"] = searchString;
            TempData["CurrentSort"] = sortOrder;
            if (!String.IsNullOrEmpty(searchString))
            {
                infs = uOW.InformationRepository.Get(
                    filter: s => s.Content.ToUpper().Contains(searchString.ToUpper())
                              || s.InformationSource.Name.ToUpper().Contains(searchString.ToUpper())
                              || s.VMSs.Select(x => x.VMS.Name).Contains(searchString.ToUpper()),
                    orderBy: s => s.OrderByDescending(x => x.DateOccur),
                    includeProperties: "VMSs,InformationSource");
            }
            else
            {
                infs = uOW.InformationRepository.Get(orderBy: s => s.OrderByDescending(x => x.DateOccur),
                    includeProperties: "VMSs,InformationSource");
            }
            switch (sortOrder)
            {
                case "Content":
                    infs = infs.OrderBy(s => s.Content);
                    break;
                case "Content_desc":
                    infs = infs.OrderByDescending(s => s.Content);
                    break;
                case "Source":
                    infs = infs.OrderBy(s => s.InformationSource.Name);
                    break;
                case "Source_desc":
                    infs = infs.OrderByDescending(s => s.InformationSource.Name);
                    break;
                case "Date_asc":
                    infs = infs.OrderBy(s => s.DateOccur);
                    break;
                default:
                    infs = infs.OrderByDescending(s => s.DateOccur);
                    break;
            }
            int pageSize = (pageListSize ?? 20);
            int pageNumber = (page ?? 1);
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            return PartialView("_UpdateTable", infs.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, int? pageListSize)
        {
            IEnumerable<Information> infs;
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
            //ViewBag.NoteSortParm = sortOrder == "Note" ? "Note_desc" : "Note";

            ViewBag.ContentSortParm = sortOrder == "Content" ? "Content_desc" : "Content";
            ViewBag.SourceSortParm = sortOrder == "Source" ? "Source_desc" : "Source";
            ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "Date_asc" : "";
            TempData["CurrentFilter"] = searchString;
            TempData["CurrentSort"] = sortOrder;
            if (!String.IsNullOrEmpty(searchString))
            {
                infs = uOW.InformationRepository.Get(
                    filter: s => s.Content.ToUpper().Contains(searchString.ToUpper())
                              || s.InformationSource.Name.ToUpper().Contains(searchString.ToUpper())
                              || s.VMSs.Select(x => x.VMS.Name.ToUpper()).Contains(searchString.ToUpper()),
                    orderBy: s => s.OrderByDescending(x => x.DateOccur),
                    includeProperties: "VMSs,InformationSource");
            }
            else
            {
                infs = uOW.InformationRepository.Get(orderBy: s => s.OrderByDescending(x => x.DateOccur),
                    includeProperties: "VMSs,InformationSource");
            }
            switch (sortOrder)
            {
                case "Content":
                    infs = infs.OrderBy(s => s.Content);
                    break;
                case "Content_desc":
                    infs = infs.OrderByDescending(s => s.Content);
                    break;
                case "Source":
                    infs = infs.OrderBy(s => s.InformationSource.Name);
                    break;
                case "Source_desc":
                    infs = infs.OrderByDescending(s => s.InformationSource.Name);
                    break;
                case "Date_asc":
                    infs = infs.OrderBy(s => s.DateOccur);
                    break;
                default:
                    infs = infs.OrderByDescending(s => s.DateOccur);
                    break;
            }
            int pageSize = (pageListSize ?? 20);
            int pageNumber = (page ?? 1);
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            return View(infs.ToPagedList(pageNumber, pageSize));
        }
        [Authorize(Roles = "Operator")]
        public ActionResult Create()
        {
            ViewBag.InformationSourceID = new SelectList(uOW.InformationSourceRepository.Get(orderBy: q => q.OrderByDescending(x => x.Name)), "InformationSourceID", "Name");
            var inf = new Information();
            PopulateAssigedVMSData(inf);
            return PartialView("_Create");
        }
        [HttpPost]
        [Authorize(Roles = "Operator")]
        public ActionResult Create([Bind(Include = "DateOccur,InformationSourceID,Content,OperatorName,Note")] Information inf,
            string[] selectedVMSs, string noVMS, string noCAM)
        {
            if (ModelState.IsValid)
            {
                inf.DateCreate = DateTime.Now;
                inf.DateUpdate = DateTime.Now;
                uOW.InformationRepository.Insert(inf);
                if (selectedVMSs != null)
                {
                    foreach (var vms in selectedVMSs)
                    {
                        var avms = uOW.VMSRepository.GetByID(int.Parse(vms));
                        var addInftoUpdate = new InformationVMS
                        {
                            Information = inf,
                            VMS = avms,
                            Success = (lastStatusVMS(avms.VMSID) <= 2 ? true : false)
                        };
                        uOW.InformationVMSRepository.Insert(addInftoUpdate);
                    }
                }
                else 
                {
                    if (noVMS != null && noCAM == null)
                    {
                        inf.Note = String.IsNullOrEmpty(inf.Note) ? "Không có bảng" : "Không có bảng\r\n" + inf.Note;
                    }
                    else if (noCAM != null && noVMS == null)
                    {
                        inf.Note = String.IsNullOrEmpty(inf.Note) ? "Không có CAM" : "Không có CAM\r\n" + inf.Note;
                    }
                    else if (noCAM == null && noVMS == null)
                    {
                        inf.Note = String.IsNullOrEmpty(inf.Note) ? "Không có CAM & bảng" : inf.Note;
                    }
                    else if (noCAM != null && noVMS != null)
                    {
                        inf.Note = String.IsNullOrEmpty(inf.Note) ? "Không có CAM & bảng" : "Không có CAM & bảng\r\n" + inf.Note;
                    }
                }
                
                uOW.Save();
                return Json(new {success = true, message = "Created Successfully."});                
            }
            ViewBag.InformationSourceID =
                new SelectList(uOW.InformationSourceRepository.Get(orderBy: q => q.OrderByDescending(x => x.Name)),
                    "InformationSourceID", "Name",inf.InformationSourceID);
            PopulateAssigedVMSData(inf);
            return PartialView("_Create");
        }
        [Authorize(Roles = "Operator")]
        public ActionResult Edit(int id = 0)
        {
            Information inf = uOW.InformationRepository.Get(filter: x => x.InformationID == id,
                                includeProperties: "VMSs,InformationSource").Single();
            if (inf == null)
            {
                return HttpNotFound();
            }
            ViewBag.InformationSourceID =
                new SelectList(uOW.InformationSourceRepository.Get(orderBy: q => q.OrderByDescending(x => x.Name)),
                    "InformationSourceID", "Name", inf.InformationSourceID);
            //ViewBag.InformationSourceID =
            //    uOW.InformationSourceRepository.GetByID(inf.InformationSourceID).Name;
            //PopulateAssigedVMSData(inf);
            return PartialView("_Edit", inf);
        }

        [HttpPost]
        [Authorize(Roles = "Operator")]
        public ActionResult Edit(int id, FormCollection formCollection, string[] selectedVMSs)
        {
            Information inf = uOW.InformationRepository.Get(filter: x => x.InformationID == id,
                                includeProperties: "VMSs,InformationSource").Single();
            if (TryUpdateModel(inf, "",
                        new string[] { "DateOccur", "InformationSourceID", "Content", "Note", "OperatorName" }))
            {
                try
                {
                    inf.DateUpdate = DateTime.Now;
                    //UpdateInformationVMS(selectedVMSs, inf);
                    uOW.InformationRepository.Update(inf);
                    uOW.Save();
                    return Json(new {success = true, message = "Updated Successfully."});
                }
                catch (DataException /* dex */)
                {
                    //Log the error (uncomment dex variable name after DataException and add a line here to write a log. 
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }
            ViewBag.InformationSourceID =
               new SelectList(uOW.InformationSourceRepository.Get(orderBy: q => q.OrderByDescending(x => x.Name)),
                   "InformationSourceID", "Name", inf.InformationSourceID);
            //ViewBag.InformationSourceID =
            //    uOW.InformationSourceRepository.GetByID(inf.InformationSourceID).Name;
            //PopulateAssigedVMSData(inf);
            return PartialView("_Edit", inf);
            
        }

        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection formCollection, string[] selectedVMSs)
        //{
        //    Information inf = uOW.InformationRepository.Get(filter: x => x.InformationID == id,
        //                        includeProperties: "VMSs,InformationSource").Single();
        //    if (TryUpdateModel(inf, "",
        //                new string[] { "DateOccur", "InformationSourceID", "Content", "Note" }))
        //    {
        //        try
        //        {
        //            inf.DateUpdate = DateTime.Now;
        //            UpdateInformationVMS(selectedVMSs, inf);
        //            uOW.InformationRepository.Update(inf);
        //            uOW.Save();
        //            return Json(new { success = true, message = "Updated Successfully." });
        //        }
        //        catch (DataException /* dex */)
        //        {
        //            //Log the error (uncomment dex variable name after DataException and add a line here to write a log. 
        //            ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
        //        }
        //    }
        //    ViewBag.InformationSourceID =
        //       new SelectList(uOW.InformationSourceRepository.Get(orderBy: q => q.OrderBy(x => x.Name)),
        //           "InformationSourceID", "Name", inf.InformationSourceID);
        //    PopulateAssigedVMSData(inf);
        //    return PartialView("_Edit", inf);

        //}
        public ActionResult GetDetails(int? id)
        {
            try
            {
                Information inf = uOW.InformationRepository.Get(filter: x => x.InformationID == id,
                                includeProperties: "VMSs,InformationSource").Single();
                var aVMS = new HashSet<string>(inf.VMSs.Select(x => x.VMS.Name));
                aVMS = aVMS.Count > 0 ? aVMS : new HashSet<string> { inf.Note };
                var rslt = new
                {
                    Content = inf.Content,
                    Time = inf.DateOccur.ToString("dd-MM-yy HH:mm"),
                    VMSs = aVMS
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

                var rslt = uOW.InformationRepository.Get(filter: x => x.InformationID == id,
                                includeProperties: "VMSs,InformationSource").Single();
                if (rslt != null)
                {
                    uOW.InformationRepository.Delete(rslt);
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

        

        private int lastStatusVMS(int ID)
        {
            var vmshs =
                uOW.VMSHistoryStatusRepository.Get(filter: x => x.VMSID == ID && x.Processed==false,
                    orderBy: s => s.OrderByDescending(d => d.DateOccur)).FirstOrDefault();
            return vmshs == null ? 1: vmshs.VMSStatusID;
        }

        private void PopulateAssigedVMSData(Information inf)
        {
            var allVMS = uOW.VMSRepository.Get(filter: q => q.Disable == false);
            var informationVMS = new HashSet<int>(uOW.InformationVMSRepository.Get(filter:x=>x.Information.InformationID ==inf.InformationID).Select(c=>c.VMSID));
            var viewModel = new List<AssigedVMSData>();
            foreach (var vms in allVMS)
            {
                viewModel.Add(new AssigedVMSData()
                {
                    VMSID = vms.VMSID,
                    Address = vms.Address,
                    Name = vms.Name,
                    Success = lastStatusVMS(vms.VMSID) ==1  ? true:false,
                    Assiged = informationVMS.Contains(vms.VMSID)
                });
            }
            ViewBag.VMSs = viewModel;
        }

        private void UpdateInformationVMS(string[] selectedVMSs, Information infToUpdate)
        {
            if (selectedVMSs == null)
            {
                infToUpdate.VMSs = null;
                return;
            }
            var selectedVMSID = new HashSet<string>(selectedVMSs);
            var informationVMS = new HashSet<int>(infToUpdate.VMSs.Select(x=>x.VMSID));
            foreach (var vms in uOW.VMSRepository.dbSet)
            {
                if (selectedVMSID.Contains(vms.VMSID.ToString()))
                {
                    if (!informationVMS.Contains(vms.VMSID))
                    {
                        var addInftoUpdate = new InformationVMS{Information=infToUpdate,VMS=vms,Success =(lastStatusVMS(vms.VMSID)<=2? true:false) };
                        uOW.InformationVMSRepository.Insert(addInftoUpdate);

                    }
                }
                else
                {
                    if (informationVMS.Contains(vms.VMSID))
                    {
                        var addInftoUpdate = uOW.InformationVMSRepository.
                            Get(filter: x => x.InformationID == infToUpdate.InformationID && x.VMSID == vms.VMSID)
                            .Single();
                        uOW.InformationVMSRepository.Delete(addInftoUpdate);
                    }
                }
            }
        }
        protected override void Dispose(bool disposing)
        {
            uOW.Dispose();
            base.Dispose(disposing);
        }
    }
}
