using System;
using System.ComponentModel.DataAnnotations;


namespace QLSL.ViewModels
{
    public class TrafficLightNodeReport
    {
        public string NodeName { get; set; }
        public string LabelMarker { get; set; }
        public decimal Lat { get; set; }
        public decimal Lng { get; set; }

        public string SignalPlanCurrent { get; set; }
        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = false)]
        public DateTime? TimeUpdateSignalPlan { get; set; }

        public int? StatusID { get; set; }
        public string NodeStatus { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = false)]
        public DateTime? TimeUpdateStatus { get; set; }

    }
}