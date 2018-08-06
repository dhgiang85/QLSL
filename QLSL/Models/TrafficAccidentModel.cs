using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using QLSL.Controllers;

namespace QLSL.Models
{
    public class TAccident : EventTimeBase
    {
        public int TAccidentID { get; set; }

        [Required]
        [Display(Name = "Liên hệ")]
        [StringLength(200, ErrorMessage = "Người liên hệ must be under 200 characters.")]
        public string ContactName { get; set; }

        
        [Display(Name = "Nhân viên")]
        [StringLength(50, ErrorMessage = "Nhân viên must be under 50 characters.")]
        public string OperatorName { get; set; }

        [Required]
        [Display(Name = "Chi tiết")]
        public string Details { get; set; }

        [Display(Name = "Ghi chú")]
        public string Note { get; set; }

        public int AccidentTypeID { get; set; }
        public virtual AccidentType AccidentType { get; set; }
    }

    public class AccidentType
    {
        public int AccidentTypeID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<TAccident> TAccidents { get; set; }
    }
}