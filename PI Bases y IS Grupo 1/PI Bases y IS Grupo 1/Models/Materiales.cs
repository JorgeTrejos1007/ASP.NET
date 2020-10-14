using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PIBasesISGrupo1.Models
{
    public class Materiales
    {

        [Required(ErrorMessage = "Es necesario que ingreses el nombre del curso")]
        [Display(Name = "Ingrese el Nombre del curso")]
        [RegularExpression(@"^[a-zA-Z\s][a-zA-Z-0-9\s]+$", ErrorMessage = "Por favor ingrese solo letras")]
        public string nombreDeSeccion {get; set;}


        [Required(ErrorMessage = "Es necesario que ingreses el nombre del material")]
        [Display(Name = "Ingrese el Nombre del material")]
        [RegularExpression(@"^[a-zA-Z\s][a-zA-Z-0-9\s]+$", ErrorMessage = "Por favor ingrese solo letras")]
        public string nombreDeCurso {get; set;}

        public byte[] byteArrayMaterial { get; set; }
        public string tipoArchivo { get; set; }
    }
}
