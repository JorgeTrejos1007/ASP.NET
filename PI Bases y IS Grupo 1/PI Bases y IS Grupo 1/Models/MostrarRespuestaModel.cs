using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PIBasesISGrupo1.Models
{
    public class MostarRespuestaModel
    {

        public int preguntaID { get; set; }

        public string correoEncuestado { get; set; }

        public string respuesta { get; set; }

        public string pregunta { get; set; }
    }
}
