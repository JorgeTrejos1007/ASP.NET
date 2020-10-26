using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PIBasesISGrupo1.Models
{
    public class EncuestaModel
    {
        public int id { get; set; }

        [Required(ErrorMessage = "Es necesario que ingreses el nombre de la encuesta")]
        public string nombreEncuesta { get; set; }

        
        [RegularExpression(@"^[_a-z0-9-]+(.[_a-z0-9-]+)*@[a-z0-9-]+(.[a-z0-9-]+)*(.[a-z]{2,4})$", ErrorMessage = "Por favor ingrese un correo valido")]
        public string correo { get; set; }

   
        [Required(ErrorMessage = "Es necesario que ingreses el topico de la encuesta")]
        public string topico { get; set; }

        [Required(ErrorMessage = "Es necesario que ingreses la vigencia de la encuesta")]
        public int vigencia { get; set; }
    }
}
