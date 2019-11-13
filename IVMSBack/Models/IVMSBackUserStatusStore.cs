using System.ComponentModel.DataAnnotations.Schema;

namespace IVMSBack.Models
{
    public class IVMSBackUserStatusStore : DefaultData
    {
        [ForeignKey("IVMSBackUser")]
        public string IVMSBackUserID { get; set; }

        [ForeignKey("IVMSBackUserStatus")]
        public string IVMSBackUserStatusID { get; set; }
    }
}
