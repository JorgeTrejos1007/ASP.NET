using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PIBasesISGrupo1.Models
{
    public class InformacionDeRegistroEnEvento
    {
        public string nombreEvento { get; set; }
        public DateTime fechaYHora { get; set; }
        public string emailCoordinador { get; set; }
        public string nombreSector { get; set; }
        public List<int> asientosDeseados { get; set; } = new List<int>();
    }
}
