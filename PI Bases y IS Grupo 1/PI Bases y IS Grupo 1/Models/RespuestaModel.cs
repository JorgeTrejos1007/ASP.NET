using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PIBasesISGrupo1.Models
{
    public class RespuestaModel
    {
        public int encuestaID { get; set; }

        public int preguntaID { get; set; }

        public int id { get; set; }

        public string correoEncuestado { get; set; }

        public string respuesta { get; set; }

    }
}
