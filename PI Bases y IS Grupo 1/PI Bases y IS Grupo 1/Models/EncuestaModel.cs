using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PIBasesISGrupo1.Models
{
    public class EncuestaModel
    {
        public int id { get; set; }
        public string nombreEncuesta { get; set; }

        public string autor { get; set; }

        public string categoria { get; set; }
        public string topico { get; set; }

        public List<PreguntaModel> Pregunta { get; set; }

        public int vigencia { get; set; }
    }
}
