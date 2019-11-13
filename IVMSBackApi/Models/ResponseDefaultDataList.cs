using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVMSBackApi.Models
{
    public class ResponseDefaultDataList : DefaultData
    {
        public int total { get; set; }
        public dynamic data { get; set; }
    }
}
