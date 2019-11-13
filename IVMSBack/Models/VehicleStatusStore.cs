using System.ComponentModel.DataAnnotations.Schema;

namespace IVMSBack.Models
{
    public class VehicleStatusStore : DefaultData
    {
        [ForeignKey("Vehicle")]
        public string VehicleID { get; set; }

        [ForeignKey("VehicleStatus")]
        public string VehicleStatusID { get; set; }
    }
}
