using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations; 

namespace PIBasesISGrupo1.Models
{
    public class Sector
    {
        [Required(ErrorMessage = "Es necesario que ingrese el nombre del sector")]
        [Display(Name = "Ingrese el nombre del sector")]
        [RegularExpression(@"^[a-zA-Z\s][a-zA-Z-0-9\s]+$", ErrorMessage = "Por favor ingrese el nombre del sector")]
        public string nombreDeSector { get; set; }

        [Required(ErrorMessage = "Es necesario que ingrese el tipo de sector")]
        [Display(Name = "Ingrese el tipo")]
        [RegularExpression(@"^(Numerado|No numerado)$", ErrorMessage = "Por favor ingrese si el sector es Numerado o No numerado")]
        public string tipo { get; set; }

        [Required(ErrorMessage = "Es necesario que ingrese la cantidad de asientos")]
        [Display(Name = "Ingrese la cantidad de asientos")]
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Por favor ingrese solo numeros")]
        public int cantidadAsientos { get; set; }
    }
}
