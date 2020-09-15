using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations; //Para las librerias de Required y Display. Para hacer binding de las propiedades del modelo vista
using System.Web; //Para incluir los objetos HttpPostedFileBase para recuperar archivos (pdf,imagenes)
using System.ComponentModel;

namespace PIBasesISGrupo1.Models
{
    public class Miembro
    {
        [Required(ErrorMessage = "Es necesario que ingreses tu genero")]
        [Display(Name = "Ingrese su genero")]
        public string Genero { get; set; }

        [Required(ErrorMessage = "Es necesario que ingreses tu nombre")]
        [Display(Name = "Ingrese su Nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Es necesario que ingreses tu primer apellido")]
        [Display(Name = "Ingrese su primer apellido")]
        public string PrimerApellido{ get; set; }

        [Display(Name = "Ingrese su segundo apellido")]
        public string SegundoApellido { get; set; }

        [Required(ErrorMessage = "Es necesario que ingreses un correo")]
        [Display(Name = "Ingrese su correo")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Es necesario que ingreses tu contraseña")]
        [Display(Name = "Ingrese su contraseña")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Es necesario que ingreses tu pais")]
        [Display(Name = "Ingrese su pais")]
        public string Pais { get; set; }

        [Display(Name = "Ingrese sus hobbies")]
        public string Hobbies { get; set; }
    }
}
