using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PIBasesISGrupo1.Models
{
    public class PreguntaModel
    {
        public int encuestaID { get; set; }

        public int preguntaID { get; set; }
        public string pregunta { get; set; }

        public string opcion1 { get; set; }
        public string opcion2 { get; set; }
        public string opcion3 { get; set; }
        public string opcion4 { get; set; }
    }
}
