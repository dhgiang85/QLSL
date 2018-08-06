using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLSL.Models
{
    public class AttachFile
    {
        public Guid ID { get; set; }

        public string Extension { get; set; }
        public string FileName { get; set; }
        
        public int VMSEventID { get; set; }

        [ForeignKey("VMSEventID")]
        public virtual VMSEvent VMSEvent { get; set; }
    }
}