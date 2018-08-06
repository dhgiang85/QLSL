using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using QLSL.Controllers;

namespace QLSL.Models
{
    public class VMS
    {
        public int VMSID { get; set; }

        [Display(Name = "Tên VMS")]
        public string Name { get; set; }

        [Display(Name = "Địa Chỉ")]
        public string Address { get; set; }

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

        public decimal Lat { get; set; }
        public decimal Lng { get; set; }

        public  ICollection<VMSHistoryStatus> VMSHistoryStatuses { get; set; }
        public virtual ICollection<InformationVMS> Informations { get; set; }
    }

    public class Information : EventTimeBase
    {
        public int InformationID { get; set; }

        public string Content { get; set; }

        [DataType(DataType.MultilineText)]
        public string Note { get; set; }

        [Display(Name = "Nhân viên")]
        [StringLength(50, ErrorMessage = "Nhân viên must be under 50 characters.")]
        public string OperatorName { get; set; }

        public int InformationSourceID { get; set; }

        [ForeignKey("InformationSourceID")]
        public virtual InformationSource InformationSource { get; set; }

        public virtual ICollection<InformationVMS> VMSs { get; set; }
    }


    public class InformationSource
    {
        public int InformationSourceID { get; set; }
        public string Name { get; set; }

        public ICollection<Information> Informations { get; set; }
    }
    [Table("InformationVMS")]
    public class InformationVMS
    {
        [Key, Column(Order = 0)]
        public int InformationID { get; set; }

        [Key, Column(Order = 1)]
        public int VMSID { get; set; }
        
        [ForeignKey("InformationID")]
        public virtual Information Information { get; set; }

        [ForeignKey("VMSID")]
        public virtual VMS VMS { get; set; }

        public bool Success { get; set; }

    }

    public class VMSHistoryStatus : EventTimeBase
    {
        public int VMSHistoryStatusID { get; set; }

        [Required]
        [Display(Name = "Liên hệ")]
        [StringLength(200, ErrorMessage = "Người liên hệ must be under 200 characters.")]
        public string ContactName { get; set; }
        
        [Display(Name = "Nhân viên")]
        [StringLength(50, ErrorMessage = "Nhân viên must be under 50 characters.")]
        public string OperatorName { get; set; }

        [Display(Name = "Chi tiết")]
        public string Details { get; set; }

        [Display(Name = "Ghi chú")]
        public string Note { get; set; }

        public bool Processed { get; set; }

        public int VMSID { get; set; }
        public virtual VMS VMS { get; set; }

        public int VMSStatusID { get; set; }
        public virtual VMSStatus VMSStatus { get; set; }
    }

    public class VMSStatus
    {
        public int VMSStatusID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<VMSHistoryStatus> VMSHistoryStatuses { get; set; }
    }


    public class VMSEvent : EventTimeBase
    {
        public int VMSEventID { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Chủ đề dưới 100 ký tự")]
        public string Subject { get; set; }

        [StringLength(300, ErrorMessage = "Mô tả dưới 300 ký tự")]
        public string Description { get; set; }
        
        //public bool IsFullDay { get; set; }
        public bool IsAlways { get; set; }
        public bool Uploaded { get; set; }
        public bool Unloaded { get; set; }

        public string ThemeColor
        {
            get
            {
                if (!IsAlways)
                {
                    if (this.DateCreate > DateTime.Now)
                    {
                        return "Blue";
                    }
                    else if (this.DateCreate <= DateTime.Now && this.DateOccur >= DateTime.Now)
                    {
                        if (!Uploaded)
                        {
                            return "sienna";
                        }
                        else
                        {
                          
                            if (!Unloaded)
                            {
                                return "Green";
                            }
                            else
                            {
                                return "Black";
                            }
                        }

                    }
                    else if (this.DateOccur < DateTime.Now)
                    {
                        if (!Uploaded)
                        {
                            return "Orange";
                        }
                        else
                        {
                            if (!Unloaded)
                            {
                                return "Red";
                            }
                            else
                            {
                                return "Black";
                            }

                        }
                    }
                }
                return "Black";


            }


        }

        public virtual AttachFile AttachFile { get; set; }
    }
}