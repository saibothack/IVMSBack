using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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
    }
}
