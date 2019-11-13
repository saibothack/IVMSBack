using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IVMSBack.Models
{
    public class Product : DefaultData
    {
        [DisplayName("Nombre")]
        [Required(ErrorMessage = "El nombre es requerido")]
        public string Name { get; set; }

        [DisplayName("Tipo de producto")]
        [Required(ErrorMessage = "El tipo de producto es requerido")]
        public ProductType ProductType { get; set; }

        [ForeignKey("Vehicle")]
        public string VehicleID { get; set; }
    }
}
