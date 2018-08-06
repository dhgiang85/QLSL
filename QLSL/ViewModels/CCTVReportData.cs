

using System;
using System.ComponentModel.DataAnnotations;

namespace QLSL.ViewModels
{
    public class CCTVSReportData
    {
  
        public string CAMName { get; set; }

        public string ContactName { get; set; }

        public string Error { get; set; }
        public string Details { get; set; }


        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DateOccur { get; set; }

    }
}