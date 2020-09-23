using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations; //Para las librerias de Required y Display. Para hacer binding de las propiedades del modelo vista

using System.ComponentModel;
using Microsoft.AspNetCore.Http;

namespace PIBasesISGrupo1.Models
{
    public class Curso
    {
        [Required(ErrorMessage = "Es necesario que ingreses el Nombre del Curso")]
        [Display(Name = "Ingrese el Nombre del Curso")]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z-0-9]+$", ErrorMessage = "Por favor ingrese solo letras")]
        public string nombre { get; set; }
        [Required(ErrorMessage = "Es necesario que ingreses el documento con la Descripcion y contenido del Curso")]
        [Display(Name = "Ingrese el documento descriptivo del Curso")]
        public byte[] byteArrayDocument { get; set; }
        [Required(ErrorMessage = "Es necesario queel curso con los Topicos disponibles en Nuestro Catalogo")]
        [Display(Name = "Favor elegir los Topicos")]
        public string[] topicos { get; set; }
        [Display(Name = "Ingrese su correo")]
        [RegularExpression(@"^[_a-z0-9-]+(.[_a-z0-9-]+)*@[a-z0-9-]+(.[a-z0-9-]+)*(.[a-z]{2,4})$", ErrorMessage = "Por favor ingrese un correo valido")]
        public string email { get; set; }
        public string emailDelQueLoPropone { get; set; }
        public string emailDelEducador { get; set; }
        public string emailDelMiembroDeNucleo { get; set; }
        public  string[] emailDeEstudiantes { get; set; }
        //public  List<Materiales> { get; set; }

    }
}
