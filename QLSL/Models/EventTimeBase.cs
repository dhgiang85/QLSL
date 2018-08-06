using System;

using System.ComponentModel.DataAnnotations;

namespace QLSL.Controllers
{
    public  class  EventTimeBase
    {
        [Display(Name = "Thời điểm tạo")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DateCreate { get; set; }

        [Display(Name = "Thời điểm cập nhật")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DateUpdate { get; set; }

        [Display(Name = "Thời gian")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DateOccur { get; set; }
    }
}