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
    public class TLController : Controller
    {

        private UnitOfWork uOW = new UnitOfWork();

        public ActionResult UpdateTable(int? page, int? pageListSize)
        {
            IEnumerable<TLNode> tls;
            //var cctvs = uOW.CCTVRepository.dbSet;

            string searchString = Convert.ToString(TempData["CurrentFilter"]);
            string sortOrder = Convert.ToString(TempData["CurrentSort"]);
            bool allEQ = Convert.ToBoolean(TempData["allEQ"]);
            TempData["CurrentFilter"] = searchString;
            TempData["CurrentSort"] = sortOrder;
            TempData["allEQ"] = allEQ;
            if (!String.IsNullOrEmpty(searchString))
            {
                tls = uOW.TLNodeRepository.Get(
                    filter: s => s.Name.ToUpper().Contains(searchString.ToUpper())
                                 || s.IP.ToUpper().Contains(searchString.ToUpper())
                                 || s.LabelMarker.ToUpper().Contains(searchString.ToUpper()),
                    orderBy: s => s.OrderBy(x => x.Name)
                    );
            }
            else
            {
                tls = uOW.TLNodeRepository.Get(orderBy: s => s.OrderBy(x => x.Name));
            }

            if (allEQ == null || allEQ == false)
            {
                tls = tls.Where(x => !x.Disable);
            }

            switch (sortOrder)
            {
                case "Marker":
                    tls = tls.OrderBy(s => s.LabelMarker);
                    break;
                case "Marker_desc":
                    tls = tls.OrderByDescending(s => s.LabelMarker);
                    break;
                case "Name_desc":
                    tls = tls.OrderByDescending(s => s.Name);
                    break;
                default:
                    tls = tls.OrderBy(s => s.Name);
                    break;
            }


            int pageSize = (pageListSize ?? 20);
            int pageNumber = (page ?? 1);
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            return PartialView("_UpdateTable", tls.ToPagedList(pageNumber, pageSize));
        }


        // GET: TL
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, int? pageListSize, bool? allEQ)
        {
            IEnumerable<TLNode> tls;
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
            ViewBag.MarkerSortParm = sortOrder == "Marker" ? "Marker_desc" : "Marker";
            TempData["CurrentSort"] = sortOrder;
            TempData["allEQ"] = allEQ;

            if (!String.IsNullOrEmpty(searchString))
            {
                tls = uOW.TLNodeRepository.Get(
                    filter: s => s.Name.ToUpper().Contains(searchString.ToUpper())
                                 || s.IP.ToUpper().Contains(searchString.ToUpper())
                                 || s.LabelMarker.ToUpper().Contains(searchString.ToUpper()),
                    orderBy: s => s.OrderBy(x => x.Name)
                    );
            }
            else
            {
                tls = uOW.TLNodeRepository.Get(orderBy: s => s.OrderBy(x => x.Name));
            }

            if (allEQ == null || allEQ == false)
            {
                tls = tls.Where(x => !x.Disable);
            }

            switch (sortOrder)
            {
                case "Marker":
                    tls = tls.OrderBy(s => s.LabelMarker);
                    break;
                case "Marker_desc":
                    tls = tls.OrderByDescending(s => s.LabelMarker);
                    break;
                case "Name_desc":
                    tls = tls.OrderByDescending(s => s.Name);
                    break;
                default:
                    tls = tls.OrderBy(s => s.Name);
                    break;
            }

            int pageSize = (pageListSize ?? 20);
            int pageNumber = (page ?? 1);
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            return View(tls.ToPagedList(pageNumber, pageSize));
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {

            return PartialView("_Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Name,Note,LabelMarker,IP,Disable,Map,Lat,Lng")]TLNode tLNode)
        {
     
            if (ModelState.IsValid)
            {

                uOW.TLNodeRepository.Insert(tLNode);
                uOW.Save();
                return Json(new { success = true, message = "Created Successfully." });
            }
      
            
            return PartialView("_Create", tLNode);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id = 0)
        {

            TLNode tLNode = uOW.TLNodeRepository.Get(filter: x => x.TLNodeID == id).FirstOrDefault();
            if (tLNode == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Edit", tLNode);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TLNode tLNode)
        {

            if (ModelState.IsValid)
            {
                uOW.TLNodeRepository.Update(tLNode);
                uOW.Save();
                return Json(new { success = true, message = "Updated Successfully." });
            }
            return PartialView("_Edit", tLNode);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult GetDetails(int? id)
        {
            try
            {
                TLNode tLNode = uOW.TLNodeRepository.GetByID(id);

                var rslt = new
                {
                    Name = tLNode.Name,
                    IP = tLNode.IP,
                    Marker = tLNode.LabelMarker

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

                var rslt = uOW.TLNodeRepository.GetByID(id);
                if (rslt != null)
                {
                    uOW.TLNodeRepository.Delete(rslt);
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