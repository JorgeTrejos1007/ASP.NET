using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations; //Para las librerias de Required y Display. Para hacer binding de las propiedades del modelo vista
using System.ComponentModel;
using Microsoft.AspNetCore.Http;



namespace PIBasesISGrupo1.Models
{
    public class Noticia
    {
        [Required(ErrorMessage = "Es necesario que ingreses tu genero")]
        [Display(Name = "Ingrese un titulo")]
        public string titulo { get; set; }

        [Required(ErrorMessage = "Es necesario que ingreses la fecha de publicacion")]
        [Display(Name = "Ingrese la fecha de publicacion")]
        public DateTime fecha { get; set; }

        
        public string tipoArchivoNoticia { get; set; }

        [Required(ErrorMessage = "Es necesario que ingreses un archivo en formato txt")]
        [Display(Name = "Ingrese la noticia")]
        public byte[] archivoNoticia { get; set; }

        public string tipoArchivoImagen { get; set; }

        [Required(ErrorMessage = "Es necesario que ingreses una imagen")]
        [Display(Name = "Ingrese la imagen de la noticia")]
        public byte[] archivoImagen{ get; set; }



    }
}
