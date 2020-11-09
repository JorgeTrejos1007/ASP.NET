using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PIBasesISGrupo1.Models
{
    public class Certificado
    {
        public int idCertificado { get; set; }
        public string nombreCurso { get; set; }
        public string nombreEducador { get; set; }
        public string nombreEstudiante { get; set; }
        public string firmaEducador { get; set; }
        public string firmaCoordinador { get; set; }
    }
}
