using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PIBasesISGrupo1.Models
{
    public class PreguntaModel
    {
        [Required(ErrorMessage = "Es necesario que ingrese una pregunta")]
        [Display(Name = "Ingrese la pregunta")]
        public string pregunta { get; set; }

        [Required(ErrorMessage = "Debe completar todas las opciones")]
        [Display(Name = "Ingrese una opción para responder")]
        public string opcion1 { get; set; }

        [Required(ErrorMessage = "Debe completar todas las opciones")]
        [Display(Name = "Ingrese una opción para responder")]
        public string opcion2 { get; set; }

        [Required(ErrorMessage = "Debe completar todas las opciones")]
        [Display(Name = "Ingrese una opción para responder")]
        public string opcion3 { get; set; }

        [Required(ErrorMessage = "Debe completar todas las opciones")]
        [Display(Name = "Ingrese una opción para responder")]
        public string opcion4 { get; set; }
    }
}
