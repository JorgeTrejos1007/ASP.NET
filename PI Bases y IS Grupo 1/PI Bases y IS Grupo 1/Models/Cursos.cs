﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations; //Para las librerias de Required y Display. Para hacer binding de las propiedades del modelo vista

using System.ComponentModel;
using Microsoft.AspNetCore.Http;

namespace PIBasesISGrupo1.Models
{
    public class Cursos
    {
        [Required(ErrorMessage = "Es necesario que ingreses el Nombre del Curso")]
        [Display(Name = "Ingrese el Nombre del Curso")]
        [RegularExpression(@"^[a-zA-Z\s][a-zA-Z-0-9\s]+$", ErrorMessage = "Por favor ingrese solo letras")]
        public string nombre { get; set; }
        [Required(ErrorMessage = "Es necesario que ingreses el documento con la Descripcion y contenido del Curso")]
        [Display(Name = "Ingrese el documento descriptivo del Curso")]
        public byte[] byteArrayDocument { get; set; }
        [Required(ErrorMessage = "Es necesario que relacione el curso con los Topicos disponibles en Nuestro Catalogo")]
        [Display(Name = "Favor elegir los Topicos")]
        public string[] topicos { get; set; }
        [Display(Name = "Ingrese su correo")]
        [RegularExpression(@"^[_a-z0-9-]+(.[_a-z0-9-]+)*@[a-z0-9-]+(.[a-z0-9-]+)*(.[a-z]{2,4})$", ErrorMessage = "Por favor ingrese un correo valido")]
        public string estado { get; set; }
        [Required(ErrorMessage = "Es necesario que el curso tenga un precio asignado")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Por favor ingrese solo digitos validos. Ejemplo: 99 ")]
        public double precio {get; set;}

        public int version {get; set;}
  
        [Required(ErrorMessage = "Es necesario que ingreses el email")]
        [Display(Name = "Ingrese el documento descriptivo del Curso")]
        [RegularExpression(@"^[_a-z0-9-]+(.[_a-z0-9-]+)*@[a-z0-9-]+(.[a-z0-9-]+)*(.[a-z]{2,4})$", ErrorMessage = "Por favor ingrese un correo valido")]
        public string emailDelQueLoPropone { get; set; }
        public string emailDelEducador { get; set; }
        public string emailDelMiembroDeNucleo { get; set; }

        public string tipoDocInformativo { get; set; }
        

    }
}
