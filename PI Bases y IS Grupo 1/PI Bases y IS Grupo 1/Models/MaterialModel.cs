using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Mime;
using System.IO;
using PIBasesISGrupo1.Handler;
using PIBasesISGrupo1.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace PIBasesISGrupo1.Models
{
    public class MaterialModel
    {

        [Required(ErrorMessage = "Es necesario que ingreses el nombre del material")]
        [RegularExpression(@"^[a-z|A-Z|0-9\s]+$", ErrorMessage = "Por favor ingrese solo letras")]
        public string nombreMaterial { get; set; }

        [Required(ErrorMessage = "Es necesario que ingreses el nombre de la seccion")]
        [RegularExpression(@"^[a-z|A-Z|0-9\s]+$", ErrorMessage = "Por favor ingrese solo letras")]
        public string nombreDeSeccion {get; set;}


        [Required(ErrorMessage = "Es necesario que ingreses el nombre del curso")]
        [RegularExpression(@"^[a-z|A-Z|0-9\s]+$", ErrorMessage = "Por favor ingrese solo letras")]
        public string nombreDeCurso {get; set;}

        [Required(ErrorMessage = "Es necesario que ingreses un archivo")]
        public byte[] archivo { get; set; }

        public string tipoArchivo { get; set; }

        public int visto { get; set; }
    }
}
