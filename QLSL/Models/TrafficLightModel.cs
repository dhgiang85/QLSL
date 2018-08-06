using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QLSL.Controllers;

namespace QLSL.Models
{
    [Table("TLSignalPlan")]
    public class TLSignalPlan : EventTimeBase
    {
        public int TLSignalPlanID { get; set; }

        [Required]
        [Display(Name = "Thời lượng hiện tại")]
        public string SignalPlanCurrent { get; set; }

        [Required]
        [Display(Name = "Thời lượng thay đổi")]
        public string SignalPlanChanged { get; set; }

        //Nhân viên thay đổi
        [Required]
        [Display(Name = "Nhân viên")]
        [StringLength(50, ErrorMessage = "Nhân viên must be under 50 characters.")]
        public string OperatorName { get; set; }

        [Display(Name = "Ghi chú")]
        public string Note { get; set; }

        //Nguyên nhân thay đổi
        [Required]
        public int ReasonChangeSPID { get; set; }
        public virtual ReasonChangeSP ReasonChangeSP { get; set; }

        public bool Unforced { get; set; }
        public bool Changed{ get; set; }
        //Chốt đèn tín hiệu
        [Required]
        public int TLNodeID { get; set; }
        public virtual TLNode TLNode { get; set; }

    }

    [Table("ReasonChangeSP")]
    public class ReasonChangeSP
    {
        public int ReasonChangeSPID { get; set; }

        [Display(Name = "Nguyên nhân")]
        public String Name { get; set; }

        public virtual ICollection<TLSignalPlan> TSignalPlans { get; set; }

    }

    public class TLNodeHitoryStatus : EventTimeBase
    {
        public int TLNodeHitoryStatusID { get; set; }

        //Nhân viên thay đổi
        [Display(Name = "Liên hệ")]
        [StringLength(200, ErrorMessage = "Người liên hệ must be under 50 characters.")]
        public string ContactName { get; set; }

        [Required]
        [Display(Name = "Nhân viên")]
        [StringLength(50, ErrorMessage = "Nhân viên must be under 50 characters.")]
        public string OperatorName { get; set; }

        [Display(Name = "Chi tiết")]
        public string Details { get; set; }

        public bool Processed { get; set; }
        //Lỗi
        public int TLNodeID { get; set; }
        public virtual TLNode TLNode { get; set; }

        public int TLNodeStatusID { get; set; }
        public virtual TLNodeStatus TLNodeStatus { get; set; }

    }
    public class TLNodeStatus
    {
        public int TLNodeStatusID { get; set; }

        [Display(Name = "Nguyên nhân")]
        public String Name { get; set; }

        public virtual ICollection<TLNodeHitoryStatus> TLNodeHitoryStatuses { get; set; }

    }

    public class TLNode
    {
        public int TLNodeID { get; set; }

        [Display(Name = "Chốt đèn")]
        public String Name { get; set; }

        [Display(Name = "IP")]
        [StringLength(15)]
        [RegularExpression(
            "^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$",
            ErrorMessage = "Invalid IP format")]
        public string IP { get; set; }



        [Display(Name = "Disable")]
        public bool Disable { get; set; }

        [Display(Name = "Map")]
        public bool Map { get; set; }

        [Display(Name = "Ghi chú")]
        [StringLength(200, ErrorMessage = "Ghi chú must be under 200 characters.")]
        public string Note { get; set; }

        public string LabelMarker { get; set; }
        public decimal Lat { get; set; }
        public decimal Lng { get; set; }

        public virtual ICollection<TLSignalPlan> TlSignalPlans { get; set; }
        public virtual ICollection<TLNodeHitoryStatus> TLNodeHitoryStatuses { get; set; }
    }
}