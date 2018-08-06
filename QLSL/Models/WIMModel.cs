using QLSL.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLSL.Models
{
    public class PrimaryW
    {
        public int PrimaryWID { get; set; }

        [Required]
        [Display(Name = "Tên Trạm sơ cấp")]
        [StringLength(256, MinimumLength = 2)]
        public string Name { get; set; }

        [Display(Name = "Ghi chú")]
        [StringLength(200, ErrorMessage = "Ghi chú must be under 200 characters.")]
        public string Note { get; set; }

        [Display(Name = "Disable")]
        public bool Disable { get; set; }

        public decimal? Lat { get; set; }
        public decimal? Lng { get; set; }


        public virtual ICollection<PrimaryWStatus> PrimaryWStatus { get; set; }
    }

    public class PrimaryWStatus : EventTimeBase
    {
        public int PrimaryWStatusID { get; set; }

        [Display(Name = "Liên hệ")]
        [StringLength(200, ErrorMessage = "Người liên hệ must be under 50 characters.")]
        public string ContactName { get; set; }


        [Display(Name = "Nhân viên")]
        [StringLength(50, ErrorMessage = "Nhân viên must be under 50 characters.")]
        public string OperatorName { get; set; }

        [Display(Name = "Chi tiết")]
        public string Details { get; set; }

        public bool Processed { get; set; }

        public int PrimaryWID { get; set; }
        public virtual PrimaryW PrimaryW { get; set; }

        public int PrimaryWErrorID { get; set; }
        public virtual PrimaryWError PrimaryWError { get; set; }

    }

    public class PrimaryWError
    {
        public int PrimaryWErrorID { get; set; }

        [StringLength(60)]
        public string Name { get; set; }

        public virtual ICollection<PrimaryWStatus> PrimaryWStatus { get; set; }
    }
}