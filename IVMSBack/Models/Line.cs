﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IVMSBack.Models
{
    public class Line : DefaultData
    {
        [DisplayName("Nombre")]
        [Required(ErrorMessage = "El nombre es requerido")]
        public string Name { get; set; }
    }
}
