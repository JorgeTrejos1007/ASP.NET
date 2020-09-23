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

        [Required(ErrorMessage = "Es necesario que ingreses el autor de la encuesta")]
        public string autor { get; set; }

        [Required(ErrorMessage = "Es necesario que ingreses la categoria de la encuesta")]
        public string categoria { get; set; }

        [Required(ErrorMessage = "Es necesario que ingreses el topico de la encuesta")]
        public string topico { get; set; }

        [Required(ErrorMessage = "Es necesario que ingreses la vigencia de la encuesta")]
        public int vigencia { get; set; }
    }
}
