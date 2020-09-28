using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PIBasesISGrupo1.Models
{
    public class PreguntaModel
    {

        public int encuestaID { get; set; }

        public int preguntaID { get; set; }

        [Required(ErrorMessage = "Es necesario que ingreses el nombre de la pregunta")]
        public string pregunta { get; set; }

        [Required(ErrorMessage = "Es necesario que ingreses la opcion1")]
        public string opcion1 { get; set; }

        [Required(ErrorMessage = "Es necesario que ingreses la opcion2")]
        public string opcion2 { get; set; }

        public string opcion3 { get; set; }
        public string opcion4 { get; set; }
    }
}
