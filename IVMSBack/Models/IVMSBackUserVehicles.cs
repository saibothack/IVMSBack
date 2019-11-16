using System.ComponentModel.DataAnnotations.Schema;
using IVMSBack.Areas.Identity.Data;

namespace IVMSBack.Models
{
    public class IVMSBackUserVehicles : DefaultData
    {
        [ForeignKey("IVMSBackUser")]
        public string IVMSBackUserID { get; set; }

        public IVMSBackUser IVMSBackUser { get; set; }

        [ForeignKey("Vehicle")]
        public int VehicleID { get; set; }

        public Vehicle Vehicle { get; set; }
    }
}
