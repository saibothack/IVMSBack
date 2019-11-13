using System.ComponentModel.DataAnnotations.Schema;

namespace IVMSBack.Models
{
    public class IVMSBackUserLines : DefaultData
    {
        [ForeignKey("IVMSBackUser")]
        public string IVMSBackUserID { get; set; }

        [ForeignKey("Line")]
        public string LineID { get; set; }
    }
}
