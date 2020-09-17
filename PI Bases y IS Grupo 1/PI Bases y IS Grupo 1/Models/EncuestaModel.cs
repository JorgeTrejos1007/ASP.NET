using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PIBasesISGrupo1.Models
{
    public class EncuestaModel
    {
        public int id { get; set; }

        public int nombreEncuesta { get; set; }

        public int autor { get; set; }

        public int topico { get; set; }

        public string pregunta { get; set; }

        public int opcion1 { get; set; }
        public int opcion2 { get; set; }
        public int opcion3 { get; set; }
        public int opcion4 { get; set; }

        public int vigencia { get; set; }
    }
}
