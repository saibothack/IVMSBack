using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IVMSBack.Areas.Identity.Data;

namespace IVMSBack.Models
{
    public class Vehicle : DefaultData
    {
        [DisplayName("Placas")]
        [Required(ErrorMessage = "Las placas son requeridas")]
        public string Plates { get; set; }

        [DisplayName("Modelo")]
        [Required(ErrorMessage = "El modelo es requerido")]
        public string Model { get; set; }

        [NotMapped]
        public string Line { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "La linea es requerida")]
        public int LineID { get; set; }

        [NotMapped]
        public string VehicleStatus { get; set; }

        [NotMapped]
        public int VehicleStatusID { get; set; }

        [NotMapped]
        public string User { get; set; }

        [NotMapped]
        public string UserId { get; set; }
    }
}
