using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using QLSL.DAL;
using QLSL.Models;
using QLSL.ViewModels;

namespace QLSL.Controllers
{
    [Authorize]
    public class VMSEController : Controller
    {
        //
        // GET: /VMSE/
        private UnitOfWork uOW = new UnitOfWork();

        public ActionResult Index()
        {
            return View();
        }

        #region Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create(DateTime? start,DateTime? end)
        {
            if (start == null || end == null)
            {
                DateTime date = DateTime.Now;
                start =  new DateTime(date.Year, date.Month, date.Day);
                end = new DateTime(date.Year, date.Month, date.Day+1);
            }

            VMSEvent vmse = new VMSEvent();

            vmse.DateCreate = start.Value;
            vmse.DateOccur = end.Value;
            return PartialView("_Create", vmse);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "DateCreate,DateOccur,Subject,Description,IsFullDay,ThemeColor,AttachFile")]
            VMSEvent vmse)
        {
            try
            {
                if (ModelState.IsValid )
                {
                    if (vmse.DateOccur < vmse.DateCreate) /*&!vmse.IsFullDay*/
                    {
                        ModelState.AddModelError(string.Empty, "invalid end time");
                    }
                }
                if (ModelState.IsValid)
                {
                    vmse.DateUpdate = DateTime.Now;

                    //if (vmse.IsFullDay)
                    //{
                    //    vmse.DateOccur = new DateTime(vmse.DateCreate.Year, vmse.DateCreate.Month,
                    //        vmse.DateCreate.Day + 1);
                    //}

                    if (vmse.DateOccur < vmse.DateCreate)
                    {
                        ModelState.AddModelError(string.Empty, "invalid end time");
                    }
                    

                    AttachFile fileDetail = new AttachFile();
                    if (Request.Files.Count > 0)
                    {
                        var file = Request.Files[0];
                        var fileName = Path.GetFileName(file.FileName);
                        if (Path.GetExtension(fileName) == ".pdf")
                        {
                            fileDetail.FileName = fileName;
                            fileDetail.Extension = Path.GetExtension(fileName);
                            fileDetail.ID = Guid.NewGuid();
                            var path = Path.Combine(Server.MapPath("~/Resource/Upload/"),
                                fileDetail.ID + fileDetail.Extension);
                            file.SaveAs(path);
                            vmse.AttachFile = fileDetail;
                        }

                    }

                    uOW.VMSEventRepository.Insert(vmse);
                    uOW.Save();
                    return Json(new {success = true, message = "Created Successfully."});
                }
                return PartialView("_Create", vmse);
            }
            catch (Exception ex)
            {
                return new JsonResult
                {
                    Data = new { ErrorMessage = ex.Message, Success = false },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.DenyGet
                };

            }
        }
        #endregion

        public ActionResult getFilePDF(int eventID)
        {
            var v = uOW.VMSEventRepository.Get(filter:x=>x.VMSEventID==eventID,includeProperties:"AttachFile").Single();
            //ViewBag.Path = Path.Combine(Server.MapPath("~/Resource/Upload"),
            //                v.AttachFile.ID + v.AttachFile.Extension);
          var path = "~/Resource/Upload/"+v.AttachFile.ID + v.AttachFile.Extension;

          return File(path, "application/pdf");
            //return PartialView("_pdfView");

            
        }

        public JsonResult GetEvents()
        {
            var VMSEs = new List<VMSEventCallendar>();
            var allEvent = uOW.VMSEventRepository.Get(filter: x => x.IsAlways == false);
            foreach (var vmse in allEvent)
            {
                VMSEs.Add(new VMSEventCallendar()
                {
                    Subject = vmse.Subject,
                    VMSEventID = vmse.VMSEventID,
                    DateCreate = vmse.DateCreate,
                    DateOccur = vmse.DateOccur,
                    Description = vmse.Description,
                    //IsFullDay = false,
                    Uploaded = vmse.Uploaded,
                    Unloaded = vmse.Unloaded,
                    ThemeColor = vmse.ThemeColor,
                    FileName =(vmse.AttachFile!=null? vmse.AttachFile.FileName:null),
                });
            } 
            return new JsonResult {Data = VMSEs, JsonRequestBehavior = JsonRequestBehavior.AllowGet};
        }

        public ActionResult PListEvent()
        {
            var vmses = uOW.VMSEventRepository.Get(
                filter: x => x.IsAlways == false,
                orderBy: x => x.OrderByDescending(d => d.DateCreate));
            var vmseUpload = (from vmse in vmses
                where vmse.Uploaded == false && vmse.DateCreate <= DateTime.Now
                select new VMSEventCallendar
                {
                    Subject = vmse.Subject,
                    VMSEventID = vmse.VMSEventID,
                    DateCreate = vmse.DateCreate,
                    DateOccur = vmse.DateOccur,
                    Description = vmse.Description,
                }
                ).ToList();
            var vmseUploading = (from vmse in vmses
                              where vmse.Unloaded == false && vmse.Uploaded == true 
                              && vmse.DateCreate <= DateTime.Now 
                              && vmse.DateOccur >= DateTime.Now
                                 select new VMSEventCallendar
                              {
                                  Subject = vmse.Subject,
                                  VMSEventID = vmse.VMSEventID,
                                  DateCreate = vmse.DateCreate,
                                  DateOccur = vmse.DateOccur,
                                  Description = vmse.Description,
                              }
                ).ToList();
            var vmseUnload = (from vmse in vmses
                where vmse.Unloaded == false && vmse.Uploaded == true && vmse.DateOccur < DateTime.Now
                select new VMSEventCallendar
                {
                    Subject = vmse.Subject,
                    VMSEventID = vmse.VMSEventID,
                    DateCreate = vmse.DateCreate,
                    DateOccur = vmse.DateOccur,
                    Description = vmse.Description,
                }
                ).ToList();
            ViewBag.VmseUpload = vmseUpload;
            ViewBag.VmseUnload = vmseUnload;
            ViewBag.VmseUploading = vmseUploading;
            return PartialView("PListEvent");
        }
        public string ThemeColor()
        {
            return "Dark";
        }

        [HttpPost]
        [Authorize(Roles = "Admin,TeamLeader")]
        public JsonResult DeleteEvent(int eventID)
        {
            var status = false;
            
                var v = uOW.VMSEventRepository.Get(filter:x=>x.VMSEventID==eventID,includeProperties:"AttachFile").Single();
                if (v != null)
                {
                    if (v.AttachFile != null)
                    {
                        String path = Path.Combine(Server.MapPath("~/Resource/Upload"),
                            v.AttachFile.ID + v.AttachFile.Extension);
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                        v.AttachFile = null;
                    }

                    uOW.VMSEventRepository.Delete(v);
                    uOW.Save();
                    status = true;
                }
            
            return new JsonResult { Data = new { status = status } };
        }
        public JsonResult Upload(int eventID)
        {
            var status = false;
            var v = uOW.VMSEventRepository.Get(filter: x => x.VMSEventID == eventID).FirstOrDefault();
            v.Uploaded = true;
            uOW.VMSEventRepository.Update(v);
            uOW.Save();
            status = true;
            return new JsonResult { Data = new { status = status } };
        }
        public JsonResult Unload(int eventID)
        {
            var status = false;
           
            var v = uOW.VMSEventRepository.Get(filter: x => x.VMSEventID == eventID).FirstOrDefault();
            if (v.Uploaded)
            {
                v.Unloaded = true;
                v.DateUpdate = DateTime.Now;
                uOW.VMSEventRepository.Update(v);
                uOW.Save();
                status = true;
            }

            return new JsonResult { Data = new { status = status } };
        }
        #region Edit
        public ActionResult Edit(int id = 0)
        {
            VMSEvent vmse = uOW.VMSEventRepository.GetByID(id);
            if (vmse == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Edit", vmse);
        }
        [HttpPost]
        public ActionResult Edit(VMSEvent vmse)
        {
            if (ModelState.IsValid)
            {
                if (vmse.DateOccur < vmse.DateCreate) /*&!vmse.IsFullDay*/
                {
                    ModelState.AddModelError(string.Empty, "invalid end time");
                }
            }
            if (ModelState.IsValid)
            {
                vmse.DateUpdate = DateTime.Now;
                //if (vmse.IsFullDay)
                //{
                //    vmse.DateOccur = new DateTime(vmse.DateCreate.Year, vmse.DateCreate.Month, vmse.DateCreate.Day+1);
                //}
                uOW.VMSEventRepository.Update(vmse);
                uOW.Save();
                return Json(new { success = true, message = "Updated Successfully." });
            }
            
            return PartialView("_Edit", vmse);
        }
    #endregion
        protected override void Dispose(bool disposing)
        {
            uOW.Dispose();
            base.Dispose(disposing);
        }

    }
}
