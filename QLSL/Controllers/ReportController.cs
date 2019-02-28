using QLSL.DAL;
using QLSL.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace QLSL.Controllers
{
 
    public class ReportController : Controller
    {
        private QLSLContext db = new QLSLContext();

        // GET: Report
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ErrorDetails()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetErrors()
        {
            try
            {
                var TLEs = (from TLE in db.TLNodeHitoryStatuses
                    where TLE.Processed == false
                    orderby TLE.DateOccur
                    select new ErrorExist
                    {
                        DateOccur = TLE.DateOccur,
                        Subject = TLE.TLNode.Name,
                        Error = TLE.TLNodeStatus.Name,
                        Detail = TLE.Details.ToString()
                    }
                    ).ToList();

                var VMSEs = (from VMSE in db.VMSHistoryStatuses
                            where VMSE.Processed == false
                             orderby VMSE.DateOccur
                             select new ErrorExist
                            {
                                DateOccur = VMSE.DateOccur,
                                Subject = VMSE.VMS.Name,
                                Error = VMSE.VMSStatus.Name,
                                Detail = VMSE.Details.ToString()
                             }
                    ).ToList();
                var CCTVEs = (from CCTV in db.CCTVStatuses
                             where CCTV.Processed == false
                              orderby CCTV.DateOccur
                              select new ErrorExist
                             {
                                 DateOccur = CCTV.DateOccur,
                                 Subject = CCTV.CCTV.Name,
                                 Error = CCTV.CCTVError.Name,
                                 Detail = CCTV.Details.ToString()
                              }
                   ).ToList();
                var WIMEs = (from WIM in db.PrimaryWStatus
                             where WIM.Processed == false
                             orderby WIM.DateOccur
                             select new ErrorExist
                              {
                                  DateOccur = WIM.DateOccur,
                                  Subject = WIM.PrimaryW.Name,
                                  Error = WIM.PrimaryWError.Name,
                                  Detail = WIM.Details.ToString()
                             }
                  ).ToList();
                return Json(new
                {
                    TLEs,VMSEs,CCTVEs,WIMEs
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [HttpGet]
        public ActionResult MonthlyReport(int? Year)
        {
            if (Year == null)
            {
                Year = DateTime.Now.Year;
            }

            var MRTF = new MonthlyErrorReport();
            MRTF.Label = "Đèn tín hiệu";


            var MRVMS = new MonthlyErrorReport();
            MRVMS.Label = "VMS";



            var MRWIM = new MonthlyErrorReport();
            MRWIM.Label = "Trạm cân";


            var MRCCTV = new MonthlyErrorReport();
            MRCCTV.Label = "Camera";


            for (var i = 1; i <= 12; i++)
            {
                int error;
                error = (from tfe in db.TLNodeHitoryStatuses
                         where tfe.DateOccur.Month == i && tfe.DateOccur.Year == Year
                         select tfe
                              ).Count();
                MRTF.TotalError.Add(error);

                error = (from vmse in db.VMSHistoryStatuses
                         where vmse.DateOccur.Month == i && vmse.DateOccur.Year == Year
                         select vmse
                             ).Count();
                MRVMS.TotalError.Add(error);

                error = (from wime in db.PrimaryWStatus
                         where wime.DateOccur.Month == i && wime.DateOccur.Year == Year
                         select wime
                             ).Count();
                MRWIM.TotalError.Add(error);

                error = (from cctve in db.CCTVStatuses
                         where cctve.DateOccur.Month == i && cctve.DateOccur.Year == Year
                         select cctve
                             ).Count();
                MRCCTV.TotalError.Add(error);
            }
            return Json(new { MRTF, MRVMS, MRCCTV, MRWIM }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ErrorReport()
        {
            var MR = new ErrorReport();
            MR.TFError = (from tfe in db.TLNodeHitoryStatuses
                          where tfe.Processed == false
                          select tfe
                          ).Count();
            MR.VMSError = (from vmse in db.VMSHistoryStatuses
                           where vmse.Processed == false
                           select vmse
                         ).Count();
            MR.WIMError = (from wime in db.PrimaryWStatus
                           where wime.Processed == false
                           select wime
                         ).Count();
            MR.CCTVError = (from cctve in db.CCTVStatuses
                            where cctve.Processed == false
                            select cctve
                         ).Count();
            return Json(MR, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public ActionResult RateReport(int? Year)
        {
            if (Year == null)
            {
                Year = DateTime.Now.Year;
            }
            var ProcessedErrTotal = (from p in db.TLNodeHitoryStatuses
                         where p.DateUpdate.Year == Year && p.Processed == true
                         select p).Count()
                         +
                         (from p in db.VMSHistoryStatuses
                          where p.DateUpdate.Year == Year && p.Processed == true 
                          select p).Count()
                          +
                         (from p in db.PrimaryWStatus
                          where p.DateUpdate.Year == Year && p.Processed == true 
                          select p).Count()
                          +
                         (from p in db.CCTVStatuses
                          where p.DateUpdate.Year == Year && p.Processed == true
                          select p).Count();

            var NonProcessErr = (from p in db.TLNodeHitoryStatuses
                         where p.Processed == false || (p.DateOccur.Year == Year && p.Processed == true && p.DateUpdate.Year > Year)
                         select p).Count()
                        +
                        (from p in db.VMSHistoryStatuses
                         where p.Processed == false || (p.DateOccur.Year == Year && p.Processed == true && p.DateUpdate.Year > Year)
                         select p).Count()
                         +
                        (from p in db.PrimaryWStatus
                         where p.Processed == false || (p.DateOccur.Year == Year && p.Processed == true && p.DateUpdate.Year > Year)
                         select p).Count()
                         +
                        (from p in db.CCTVStatuses
                         where p.Processed == false || (p.DateOccur.Year == Year && p.Processed == true && p.DateUpdate.Year > Year)
                         select p).Count();

            return Json(new { NonProcessErr, ProcessedErrTotal }, JsonRequestBehavior.AllowGet);
        }
    }
}