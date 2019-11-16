using System.ComponentModel.DataAnnotations.Schema;

namespace IVMSBack.Models
{
    public class VehicleStatusStore : DefaultData
    {
        [ForeignKey("Vehicle")]
        public int VehicleID { get; set; }
        public Vehicle Vehicle { get; set; }

        [ForeignKey("VehicleStatus")]
        public int VehicleStatusID { get; set; }
        public VehicleStatus VehicleStatus { get; set; }
    }
}
