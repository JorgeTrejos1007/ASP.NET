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

        public string nombreMaterial { get; set; }

        [Required(ErrorMessage = "Es necesario que ingreses el nombre del curso")]
        [Display(Name = "Ingrese el Nombre del curso")]
        [RegularExpression(@"^[a-zA-Z\s][a-zA-Z-0-9\s]+$", ErrorMessage = "Por favor ingrese solo letras")]
        public string nombreDeSeccion {get; set;}


        [Required(ErrorMessage = "Es necesario que ingreses el nombre del material")]
        [Display(Name = "Ingrese el Nombre del material")]
        [RegularExpression(@"^[a-zA-Z\s][a-zA-Z-0-9\s]+$", ErrorMessage = "Por favor ingrese solo letras")]
        public string nombreDeCurso {get; set;}

        public byte[] archivo { get; set; }

        public string tipoArchivo { get; set; }
    }
}
