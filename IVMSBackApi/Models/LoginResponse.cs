using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVMSBackApi.Models
{
    public class LoginResponse : DefaultData
    {
        public string Token { get; set; }
    }
}
