using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PIBasesISGrupo1.Models
{
    public class Catalogo
    {

        [Required(ErrorMessage = "Es necesario que ingrese un tópico")]
        [Display(Name = "Ingrese un tópico")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Por favor ingrese solo letras")]

        public string topico { get; set; }

        [Required(ErrorMessage = "Es necesario que ingrese una categoría")]
        [Display(Name = "Ingrese un tópico")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Por favor ingrese solo letras")]
        public string categoria { get; set; }
    }
}
