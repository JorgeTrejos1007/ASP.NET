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
        public byte[] firmaEducador { get; set; }
        public byte []  firmaCoordinador { get; set; }
        public string imagenCertificado { get; set; }
        public string fecha { get; set; }
        public string emailEstudiante { get; set; }
    }
}
