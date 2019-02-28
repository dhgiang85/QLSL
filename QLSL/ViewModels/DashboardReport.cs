
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QLSL.ViewModels
{
    public class ErrorReport
    {
        public int TFError { get; set; }
        public int CCTVError { get; set; }
        public int VMSError { get; set; }
        public int WIMError { get; set; }
    }

    public class MonthlyErrorReport
    {
        public MonthlyErrorReport()
        {
            TotalError = new List<int>();
        }
        public string Label { get; set; }
        public List<int> TotalError { get; set; }
    }
    public class ErrorRateReport
    {
        public int TTError { get; set; }
        public int TTProcessed { get; set; }
    }

    public class ErrorExist
    {
        [DisplayFormat(DataFormatString = "{0:dd/MM HH:mm}")]
        public DateTime DateOccur { get; set; }

        public string Subject { get; set; }

        public string Detail { get; set; }
        public string Error { get; set; }
    }
}