using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QLSL.Controllers;

namespace QLSL.Models
{
    public class CCTV
    {
        public int CCTVID { get; set; }

        [Required]
        [Display(Name = "Tên CCTV")]
        [StringLength(256, MinimumLength = 2)]
        public string Name { get; set; }

        [Display(Name = "Địa Chỉ")]
        public string Address { get; set; }

    
        [Display(Name = "IP")]
        [StringLength(15)]
        [RegularExpression(
            "^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$",
            ErrorMessage = "Invalid IP format")]
        public string IP { get; set; }

        [Display(Name = "Hãng sản xuất")]
        [StringLength(30, ErrorMessage = "Manufacturer must be under 30 characters.")]
        public string Manufacturer { get; set; }

        [StringLength(60, ErrorMessage = "Model must be under 60 characters.")]
        public string Model { get; set; }

        [Display(Name = "Năm lắp đặt")]
        [Range(1900, 2200)]
        public int YearInstall { get; set; }

        [Display(Name = "Disable")]
        public bool Disable { get; set; }

        [Display(Name = "Ghi chú")]
        [StringLength(200, ErrorMessage = "Ghi chú must be under 200 characters.")]
        public string Note { get; set; }

        [Display(Name = "Map")]
        public bool Map { get; set; }

        public decimal? Lat { get; set; }
        public decimal? Lng { get; set; }

        public virtual ICollection<Zone> Zones { get; set; }

        [Display(Name = "Mã đường truyền")]
        [StringLength(100)]
        public string MDT { get; set; }

        [Display(Name = "Mã tiền điện")]
        [StringLength(100)]
        public string MTD { get; set; }

        [ForeignKey("OwerCCTV")]
        public int? OwerCCTVID { get; set; }
        public OwerCCTV OwerCCTV { get; set; }


        [ForeignKey("LocatyCCTV")]
        public int? LocatyCCTVID { get; set; }
        public virtual OwerCCTV LocatyCCTV { get; set; }

      
        public int? CAMTypeID { get; set; }
        public virtual CAMType CAMType { get; set; }

        public virtual ICollection<CCTVStatus> CCTVStatuses { get; set; }
    }

    public class Zone
    {
        public int ZoneID { get; set; }

        [StringLength(60)]
        public string Name { get; set; }
        
        public virtual ICollection<CCTV> CCTVs { get; set; }
    }

    public class OwerCCTV
    {
        public int OwerCCTVID { get; set; }

        [StringLength(30)]
        public string Name { get; set; }

        [InverseProperty("OwerCCTV")]
        public virtual  ICollection<CCTV> Ower { get; set; }
        [InverseProperty("LocatyCCTV")]
        public virtual ICollection<CCTV>  Locaty { get; set; }
    }
  
    public class CAMType
    {
        public int CAMTypeID { get; set; }

        [StringLength(30)]
        public string Name { get; set; }
        public virtual ICollection<CCTV> CCTVs { get; set; }

    }
    public class CCTVStatus :EventTimeBase
    {
        public int CCTVStatusID { get; set; }

        [Display(Name = "Liên hệ")]
        [StringLength(200, ErrorMessage = "Người liên hệ must be under 50 characters.")]
        public string ContactName { get; set; }

   
        [Display(Name = "Nhân viên")]
        [StringLength(50, ErrorMessage = "Nhân viên must be under 50 characters.")]
        public string OperatorName { get; set; }

        [Display(Name = "Chi tiết")]
        public string Details { get; set; }

        public bool Processed { get; set; }
        
        public int CCTVID { get; set; }
        public virtual CCTV CCTV { get; set; }

        public int CCTVErrorID { get; set; }
        public virtual CCTVError CCTVError { get; set; }
        
    }

    public class CCTVError 
    {
        public int CCTVErrorID { get; set; }

        [StringLength(60)]
        public string Name { get; set; }

        public virtual ICollection<CCTVStatus> CCTVStatuses { get; set; }
    }
}