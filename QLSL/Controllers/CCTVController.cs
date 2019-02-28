using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using QLSL.DAL;
using QLSL.Models;
using QLSL.ViewModels;

namespace QLSL.Controllers
{
    [Authorize]
    public class CCTVController : Controller
    {
        //
        // GET: /CCTV/
        private UnitOfWork uOW = new UnitOfWork();


        public ActionResult UpdateTable(int? page, int? pageListSize)
        {
            IEnumerable<CCTV> cctvs;
            //var cctvs = uOW.CCTVRepository.dbSet;

            string searchString = Convert.ToString(TempData["CurrentFilter"]);
            string sortOrder = Convert.ToString(TempData["CurrentSort"]);
            bool allCCTV = Convert.ToBoolean(TempData["allCCTV"]);
            TempData["CurrentFilter"] = searchString;
            TempData["CurrentSort"] = sortOrder;
            TempData["allCCTV"] = allCCTV;


            if (!String.IsNullOrEmpty(searchString))
            {
                cctvs = uOW.CCTVRepository.Get(
                    filter: s => s.Note.ToUpper().Contains(searchString.ToUpper())
                        || s.Name.ToUpper().Contains(searchString.ToUpper())
                        || s.Address.ToUpper().Contains(searchString.ToUpper())
                        || s.IP.ToUpper().Contains(searchString.ToUpper())
                        || s.OwerCCTV.Name.ToUpper().Contains(searchString.ToUpper())
                        || s.LocatyCCTV.Name.ToUpper().Contains(searchString.ToUpper())
                        || s.Manufacturer.ToUpper().Contains(searchString.ToUpper())
                        || s.Model.ToUpper().Contains(searchString.ToUpper())
                        || s.CAMType.Name.ToUpper().Contains(searchString.ToUpper())
                        || s.Zones.Where(q => q.Name.ToUpper().Contains(searchString.ToUpper())).FirstOrDefault() != null,
                    orderBy: s => s.OrderByDescending(x => x.YearInstall),
                    includeProperties: "Zones,OwerCCTV,CAMType,LocatyCCTV"
                    );
            }
            else
            {
                cctvs = uOW.CCTVRepository.Get(orderBy: s => s.OrderByDescending(x => x.YearInstall), includeProperties: "Zones,OwerCCTV");
            }
            if (allCCTV == null || allCCTV == false)
            {
                cctvs = cctvs.Where(x => !x.Disable);
            }
            switch (sortOrder)
            {

                case "Model":
                    cctvs = cctvs.OrderBy(s => s.Model);
                    break;
                case "Model_desc":
                    cctvs = cctvs.OrderByDescending(s => s.Model);
                    break;
                case "Man":
                    cctvs = cctvs.OrderBy(s => s.Manufacturer);
                    break;
                case "Man_desc":
                    cctvs = cctvs.OrderByDescending(s => s.Manufacturer);
                    break;
                case "Year_asc":
                    cctvs = cctvs.OrderBy(s => s.YearInstall);
                    break;
                default:
                    cctvs = cctvs.OrderByDescending(s => s.YearInstall);
                    break;
            }
            int pageSize = (pageListSize ?? 20);
            int pageNumber = (page ?? 1);
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            //ViewBag.TotalRecord = cctvs.Count();
            return PartialView("_UpdateTable",cctvs.ToPagedList(pageNumber, pageSize));

        }

     
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page,int? pageListSize, bool? allCCTV)
        {
            IEnumerable<CCTV> cctvs;
            bool result = User.IsInRole("admin");
            //var cctvs = uOW.CCTVRepository.dbSet;
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
            ViewBag.AllCCTV = allCCTV;

            ViewBag.YearInstallSortParm = String.IsNullOrEmpty(sortOrder) ? "Year_asc" : ""; 
            ViewBag.ModelSortParm = sortOrder == "Model" ? "Model_desc" : "Model";
            ViewBag.ManSortParm = sortOrder == "Man" ? "Man_desc" : "Man";

            TempData["CurrentFilter"] = searchString;
            TempData["CurrentSort"] = sortOrder;
            TempData["allCCTV"] = allCCTV;
          
            if (!String.IsNullOrEmpty(searchString))
            {
                cctvs = uOW.CCTVRepository.Get(
                    filter: s => s.Note.ToUpper().Contains(searchString.ToUpper())
                        || s.Name.ToUpper().Contains(searchString.ToUpper())
                        || s.Address.ToUpper().Contains(searchString.ToUpper())
                        || s.IP.ToUpper().Contains(searchString.ToUpper())
                        || s.OwerCCTV.Name.ToUpper().Contains(searchString.ToUpper())
                        || s.LocatyCCTV.Name.ToUpper().Contains(searchString.ToUpper())
                        || s.Manufacturer.ToUpper().Contains(searchString.ToUpper())
                        || s.Model.ToUpper().Contains(searchString.ToUpper())
                        || s.CAMType.Name.ToUpper().Contains(searchString.ToUpper())
                        || s.Zones.Where(q => q.Name.ToUpper().Contains(searchString.ToUpper())).FirstOrDefault() != null,
                    orderBy: s => s.OrderByDescending(x => x.YearInstall),
                    includeProperties: "Zones,OwerCCTV,CAMType,LocatyCCTV"
                    );
            }
            else
            {
                cctvs = uOW.CCTVRepository.Get(orderBy: s => s.OrderByDescending(x => x.YearInstall), includeProperties: "Zones,OwerCCTV");
            }
            if (allCCTV == null || allCCTV == false)
            {
                cctvs = cctvs.Where(x => !x.Disable);
            }
            switch (sortOrder)
            {

                case "Model":
                    cctvs = cctvs.OrderBy(s => s.Model);
                    break;
                case "Model_desc":
                    cctvs = cctvs.OrderByDescending(s => s.Model);
                    break;
                case "Man":
                    cctvs = cctvs.OrderBy(s => s.Manufacturer);
                    break;
                case "Man_desc":
                    cctvs = cctvs.OrderByDescending(s => s.Manufacturer);
                    break;
                case "Year_asc":
                    cctvs = cctvs.OrderBy(s => s.YearInstall);
                    break;
                default:
                    cctvs = cctvs.OrderByDescending(s => s.YearInstall);
                    break;
            }
            int pageSize = (pageListSize ?? 20);
            int pageNumber = (page ?? 1);
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            //ViewBag.TotalRecord = cctvs.Count();
            return View(cctvs.ToPagedList(pageNumber, pageSize));
  
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            var cctv = new CCTV();
            cctv.Zones = new List<Zone>();

            PopulateCCTVZoneData(cctv);
            ViewBag.OwerCCTVID = new SelectList(uOW.OwerCCTVRepository.dbSet, "OwerCCTVID", "Name");
            ViewBag.CAMTypeID = new SelectList(uOW.CAMTypeRepository.dbSet, "CAMTypeID", "Name");
            return PartialView("_Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Name,Address,IP,Manufacturer,Model,YearInstall,Disable,Note,Map,OwerCCTVID,LocatyCCTVID,CAMTypeID,Lat,Lng")] CCTV cctv,
                    string[] selectedZones)
        {
            if (selectedZones != null)
            {
                cctv.Zones = new List<Zone>();
                foreach (var zone in selectedZones)
                {
                    var zoneToAdd = uOW.ZoneRepository.GetByID(int.Parse(zone));
                    cctv.Zones.Add(zoneToAdd);
                }
            } 
            if (ModelState.IsValid)
            {
         
                uOW.CCTVRepository.Insert(cctv);
                uOW.Save();
                return Json(new { success = true, message = "Created Successfully." });
            }
            PopulateCCTVZoneData(cctv);
            ViewBag.OwerCCTVID = new SelectList(uOW.OwerCCTVRepository.dbSet, "OwerCCTVID", "Name", cctv.OwerCCTVID);
            ViewBag.CAMTypeID = new SelectList(uOW.CAMTypeRepository.dbSet, "CAMTypeID", "Name", cctv.CAMTypeID);
            return PartialView("_Create", cctv);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id = 0)
        {

            CCTV cctv = uOW.CCTVRepository.Get(filter: x => x.CCTVID == id,
                includeProperties: "Zones,OwerCCTV,CAMType,LocatyCCTV").FirstOrDefault();
            if (cctv == null)
            {
                return HttpNotFound();
            }
            
            PopulateCCTVZoneData(cctv);
            ViewBag.OwerCCTVID = new SelectList(uOW.OwerCCTVRepository.dbSet, "OwerCCTVID", "Name", cctv.OwerCCTVID);
            ViewBag.LocatyCCTVID = new SelectList(uOW.OwerCCTVRepository.dbSet, "OwerCCTVID", "Name", cctv.LocatyCCTVID);
            ViewBag.CAMTypeID = new SelectList(uOW.CAMTypeRepository.dbSet, "CAMTypeID", "Name", cctv.CAMTypeID);
            return PartialView("_Edit", cctv);
        }
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id, FormCollection formCollection, string[] selectedZones)
        {
            CCTV cctv = uOW.CCTVRepository.Get(filter: x => x.CCTVID == id,
                includeProperties: "Zones,OwerCCTV,CAMType,LocatyCCTV").FirstOrDefault();
            if (TryUpdateModel(cctv, "",
                        new string[] { "Name","Address","IP","Manufacturer","Model","YearInstall","Disable"
                            ,"Note","Map","Lat","Lng","OwerCCTVID","LocatyCCTVID", "CAMTypeID"  }))
            {
                try
                {

                    uOW.CCTVRepository.Update(cctv);
                    UpdateCCTVZone(selectedZones, cctv);
                    uOW.Save();
                    return Json(new {success = true, message = "Updated Successfully."});
                }
                catch (DataException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }
            PopulateCCTVZoneData(cctv);
            ViewBag.OwerCCTVID = new SelectList(uOW.OwerCCTVRepository.dbSet, "OwerCCTVID", "Name", cctv.OwerCCTVID);
            ViewBag.LocatyCCTVID = new SelectList(uOW.OwerCCTVRepository.dbSet, "OwerCCTVID", "Name", cctv.LocatyCCTVID);
            ViewBag.CAMTypeID = new SelectList(uOW.CAMTypeRepository.dbSet, "CAMTypeID", "Name", cctv.CAMTypeID);

            return PartialView("_Edit", cctv);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult EditMain(int id = 0)
        {

            CCTV cctv = uOW.CCTVRepository.Get(filter: x => x.CCTVID == id).FirstOrDefault();
            if (cctv == null)
            {
                return HttpNotFound();
            }

            return PartialView("_EditMain", cctv);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult EditMain(int id, FormCollection formCollection, string[] selectedZones)
        {
            CCTV cctv = uOW.CCTVRepository.Get(filter: x => x.CCTVID == id).FirstOrDefault();
            if (TryUpdateModel(cctv, "", new string[] {"MDT", "MTD"}))
            {
                try
                {

                    uOW.CCTVRepository.Update(cctv);
                    uOW.Save();
                    return Json(new { success = true, message = "Updated Successfully." });
                }
                catch (DataException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }
            
            return PartialView("_EditMain", cctv);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult GetDetails(int? id)
        {
            try
            {
                CCTV cctv = uOW.CCTVRepository.GetByID(id);

                var rslt = new
                {
                    Name = cctv.Name,
                    IP = cctv.IP,
                    Address = cctv.Address

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

                var rslt = uOW.CCTVRepository.GetByID(id);
                if (rslt != null)
                {
                    uOW.CCTVRepository.Delete(rslt);
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
        private void PopulateCCTVZoneData(CCTV cctv)
        {
            var allZone = uOW.ZoneRepository.dbSet;
            var cctvZone = new HashSet<int>();
            if (cctv.Zones != null)
            {
                cctvZone = new HashSet<int>(cctv.Zones.Select(c => c.ZoneID));
            }
            var viewModel = new List<AssigedZoneData>();
            foreach (var zone in allZone)
            {
                viewModel.Add(new AssigedZoneData()
                {
                    ZoneID = zone.ZoneID,
                    Name = zone.Name,
                    Assiged = cctvZone.Contains(zone.ZoneID)
                });
            }
            ViewBag.Zones = viewModel;
        }

        private void UpdateCCTVZone(string[] selectedZones, CCTV cctvToUpdate)
        {
            if (selectedZones == null)
            {
                cctvToUpdate.Zones = new List<Zone>();
                return;
            }
            var selectedZoneHS = new HashSet<string>(selectedZones);
            var cctvZones = new HashSet<int>
                (cctvToUpdate.Zones.Select(c => c.ZoneID));

            foreach (var zone in uOW.ZoneRepository.dbSet)
            {
                if (selectedZoneHS.Contains(zone.ZoneID.ToString()))
                {
                    if (!cctvZones.Contains(zone.ZoneID))
                    {
                        cctvToUpdate.Zones.Add(zone);
                    }
                }
                else
                {
                    if (cctvZones.Contains(zone.ZoneID))
                    {
                        cctvToUpdate.Zones.Remove(zone);
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
