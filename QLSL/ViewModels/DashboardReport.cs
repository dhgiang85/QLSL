
using System.Collections.Generic;

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
}