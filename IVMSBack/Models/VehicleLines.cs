using System.ComponentModel.DataAnnotations.Schema;

namespace IVMSBack.Models
{
    public class VehicleLines : DefaultData
    {
        [ForeignKey("Vehicle")]
        public string VehicleID { get; set; }

        [ForeignKey("Line")]
        public string LineID { get; set; }
    }
}
