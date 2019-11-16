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
        public string Latitude { get; set; }

        [ScaffoldColumn(false)]
        public string Longitude { get; set; }
    }
}
