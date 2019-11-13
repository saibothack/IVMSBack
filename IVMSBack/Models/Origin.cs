using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IVMSBack.Models
{
    public class Origin : DefaultData
    {
        [DisplayName("Nombre")]
        [Required(ErrorMessage = "El nombre es requerido")]
        public string Name { get; set; }

        [DisplayName("Direccion")]
        [Required(ErrorMessage = "La direccion es requerida")]
        public string Address { get; set; }

        [ScaffoldColumn(false)]
        [Required(ErrorMessage = "La latitud es requerida")]
        public string Latitude { get; set; }

        [ScaffoldColumn(false)]
        [Required(ErrorMessage = "La longitud es requerida")]
        public string Longitude { get; set; }
    }
}
