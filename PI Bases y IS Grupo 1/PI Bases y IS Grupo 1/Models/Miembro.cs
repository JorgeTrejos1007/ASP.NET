using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations; //Para las librerias de Required y Display. Para hacer binding de las propiedades del modelo vista

using System.ComponentModel;
using Microsoft.AspNetCore.Http;

namespace PIBasesISGrupo1.Models
{
    public class Miembro
    {
        [Required(ErrorMessage = "Es necesario que ingreses tu genero")]
        [Display(Name = "Ingrese su genero")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Por favor ingrese solo letras")]
        public string genero { get; set; }

        [Required(ErrorMessage = "Es necesario que ingreses tu nombre")]
        [Display(Name = "Ingrese su Nombre")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Por favor ingrese solo letras")]
        public string nombre { get; set; }

        [Required(ErrorMessage = "Es necesario que ingreses tu primer apellido")]
        [Display(Name = "Ingrese su primer apellido")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Por favor ingrese solo letras")]
        public string primerApellido { get; set; }

        [Display(Name = "Ingrese su segundo apellido")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Por favor ingrese solo letras")]
        public string segundoApellido { get; set; }

        [Required(ErrorMessage = "Es necesario que ingreses un correo")]
        [Display(Name = "Ingrese su correo")]
        public string email { get; set; }

        [Required(ErrorMessage = "Es necesario que ingreses tu contraseña")]
        [Display(Name = "Ingrese su contraseña")]
        public string password { get; set; }

        [Required(ErrorMessage = "Es necesario que ingreses tu pais")]
        [Display(Name = "Ingrese su pais")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Por favor ingrese solo letras")]
        public string pais { get; set; }

        [Display(Name = "Ingrese sus hobbies")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Por favor ingrese solo letras")]
        public string hobbies { get; set; }


        public string tipoArchivo { get; set; }

        public byte[] byteArrayImage { get; set; }

    }
}
