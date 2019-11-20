using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IVMSBack.Models
{
    public class VehicleStatus : DefaultData
    {
        [DisplayName("Nombre")]
        [Required(ErrorMessage = "El nombre es requerido")]
        public string Name { get; set; }

        [DisplayName("Icono")]
        public string Icon { get; set; }

        [DisplayName("Color")]
        [Required(ErrorMessage = "El color es requerido")]
        public string Color { get; set; }
    }
}
