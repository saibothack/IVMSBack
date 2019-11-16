using IVMSBack.Areas.Identity.Data;

namespace IVMSBackApi.Models
{
    public class LoginResponse : DefaultData
    {
        public string Token { get; set; }

        public IVMSBackUser User { get; set; }
    }
}
