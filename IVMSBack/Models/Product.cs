using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IVMSBack.Models
{
    public class Product : DefaultData
    {
        [DisplayName("Nombre")]
        [Required(ErrorMessage = "El nombre es requerido")]
        public string Name { get; set; }

        [DisplayName("Especificar carga")]
        public bool SpecifyLoad { get; set; }
    }
}
