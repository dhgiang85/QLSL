using QLSL.DAL;
using QLSL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLSL.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private QLSLContext db = new QLSLContext();

        // GET: Report
        public ActionResult Index()
        {
            return View();
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