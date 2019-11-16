using System.ComponentModel.DataAnnotations.Schema;

namespace IVMSBack.Models
{
    public class VehicleLines : DefaultData
    {
        [ForeignKey("Vehicle")]
        public int VehicleID { get; set; }
        public Vehicle Vehicle { get; set; }

        [ForeignKey("Line")]
        public int LineID { get; set; }
        public Line Line { get; set; }
    }
}
