﻿using System.ComponentModel.DataAnnotations.Schema;
using IVMSBack.Areas.Identity.Data;

namespace IVMSBack.Models
{
    public class IVMSBackUserLines : DefaultData
    {
        [ForeignKey("IVMSBackUser")]
        public string IVMSBackUserID { get; set; }
        public IVMSBackUser IVMSBackUser { get; set; }

        [ForeignKey("Line")]
        public int LineID { get; set; }
        public Line Line { get; set; }
    }
}
