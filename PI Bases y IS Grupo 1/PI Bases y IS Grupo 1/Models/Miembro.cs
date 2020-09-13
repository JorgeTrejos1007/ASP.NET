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
        public int Id { get; set; }
        public string Genero { get; set; }
        [Required(ErrorMessage = "Es necesario que ingreses tu nombre")]
        [Display(Name = "Ingrese su Nombre")]
        public string nombre { get; set; }
        [Required(ErrorMessage = "Es necesario que ingreses tu primer apellido")]
        [Display(Name = "Ingrese su primer apellido")]
        public string PrimerApellido{ get; set; }

        [Required(ErrorMessage = "Es necesario que ingreses tu segundo apellido")]
        [Display(Name = "Ingrese su segundo apellido")]
        public string SegundoApellido { get; set; }

        [Required(ErrorMessage = "Es necesario que ingreses un correo")]
        [Display(Name = "Ingrese su correo")]
        //[RegularExpression("^[0-9]*$", ErrorMessage = "Debe ingresar numeros")]
        public string Correo { get; set; }        
        public string Contraseña { get; set; }
        public string Nacionalidad { get; set; }
    }
}
