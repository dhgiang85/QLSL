using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QLSL.Controllers;

namespace QLSL.Models
{
    [Table("TunnelError")]
    public class TunnelError : EventTimeBase
    {
      
        public int TunnelErrorID { get; set; }

        [Display(Name = "Chi tiết lỗi")]
        [StringLength(255)]
        public string Details { get; set; }

        [Display(Name = "Ghi chú")]
        [StringLength(128, ErrorMessage = "Ghi chú must be under 128 characters.")]
        public string Note { get; set; }


        [Required]
        [Display(Name = "Người vận hành")]
        [StringLength(128, ErrorMessage = "Người liên hệ must be under 128 characters.")]
        public string OperatorName { get; set; }
        
        [Display(Name = "Người tiếp nhận")]
        [StringLength(128, ErrorMessage = "Người liên hệ must be under 128 characters.")]
        public string ContactName { get; set; }
                
        [Display(Name = "Người khắc phục")]
        [StringLength(128, ErrorMessage = "Người liên hệ must be under 128 characters.")]
        public string Maintenancer { get; set; }

        [Display(Name = "Chi tiết lỗi")]
        [StringLength(255)]
        public string Measures { get; set; }

        public bool Processed { get; set; }
    }
}