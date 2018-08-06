using System;
using System.ComponentModel.DataAnnotations;

namespace QLSL.ViewModels
{
    public class VMSReportData
    {
        public string Name { get; set; }
        public int Count { get; set; }
    }

    public class VMSEventCallendar
    {

        public int VMSEventID { get; set; }

        //public bool IsFullDay { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM HH:mm}")]
        public DateTime DateCreate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM HH:mm}")]
        public DateTime DateOccur { get; set; }
        public string Subject { get; set; }

        public bool Uploaded { get; set; }
        public bool Unloaded { get; set; }

        public string Description { get; set; }
        public string ThemeColor { get; set; }
        public string FileName { get; set; }
    }
}