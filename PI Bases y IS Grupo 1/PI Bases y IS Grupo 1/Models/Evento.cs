using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations; //Para las librerias de Required y Display. Para hacer binding de las propiedades del modelo vista

namespace PIBasesISGrupo1.Models
{
    public class Evento
    {
        [Required(ErrorMessage = "Es necesario que ingreses el nombre del evento")]
        [Display(Name = "Ingrese el Nombre del evento")]
        [RegularExpression(@"^[a-zA-Z\s][a-zA-Z-0-9\s]+$", ErrorMessage = "Por favor ingrese solo letras y numeros")]
        public string nombre { get; set; }

        [Required(ErrorMessage = "Es necesario que ingreses la fecha del evento")]
        [Display(Name = "Ingrese la fecha del evento")]
        public DateTime fechaYHora { get; set; }

        [Required(ErrorMessage = "Es necesario que ingreses la información sobre el evento")]
        [Display(Name = "Ingrese informacion del evento")]
        public string descripcionDelEvento{ get; set; }

        public string tipoArchivoImagen { get; set; }

        [Required(ErrorMessage = "Es necesario que ingreses una imagen para el evento")]
        [Display(Name = "Ingrese la imagen de la noticia")]
        public byte[] arrayArchivoImagen { get; set; }
      
        [Display(Name = "Ingrese el nombre del canal de Twitch donde se va a realizar la transimision del evento")]
        [RegularExpression(@"^[a-zA-Z\s][a-zA-Z-0-9\s]+$", ErrorMessage = "Por favor ingrese solo letras y numeros")]
        public string nombreCanalStream { get; set; }
      
        [Display(Name = "Ingrese ingreses la direccion de dónde se va a llevar a cabo el evento")]
        public string lugar { get; set; }

        public List<Sector> sectores { get; set; }
    }
}
