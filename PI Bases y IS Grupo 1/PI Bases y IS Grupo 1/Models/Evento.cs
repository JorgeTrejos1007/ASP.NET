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
        [RegularExpression(@"([a-z]|[A-Z])+.*", ErrorMessage = "Por favor ingrese solo letras y numeros")]
        public string nombre { get; set; }

        [Required(ErrorMessage = "Es necesario que ingreses la fecha del evento")]
        [Display(Name = "Ingrese la fecha del evento")]
        public DateTime fechaYHora { get; set; }

        [Required(ErrorMessage = "Es necesario que ingreses la información sobre el evento")]
        [RegularExpression(@"([a-z]|[A-Z])+.*", ErrorMessage = "Por favor ingrese solo letras y numeros")]
        [Display(Name = "Ingrese informacion del evento")]
        public string descripcionDelEvento{ get; set; }

        public string tipoArchivoImagen { get; set; }

        public byte[] arrayArchivoImagen { get; set; }

        [Required(ErrorMessage = "Es necesario que ingrese el nombre del canal donde se realizara el stream")]
        [Display(Name = "Ingrese el nombre del canal de Twitch donde se va a realizar la transimision del evento")]
        [RegularExpression(@"^\s*[a-zA-Z0-9][a-zA-Z-0-9\s]*$", ErrorMessage = "Por favor ingrese solo letras y numeros")]
        public string nombreCanalStream { get; set; }

        [Required(ErrorMessage = "Es necesario que ingrese el lugar donde será el evento")]
        [Display(Name = "Ingrese la direccion de dónde se va a llevar a cabo el evento")]
        [RegularExpression(@"([a-z]|[A-Z])+.*", ErrorMessage = "Por favor ingrese solo letras y numeros")]
        public string lugar { get; set; }

        [Required(ErrorMessage = "Es necesario que elija el tipo de evento")]
        [Display(Name = "Elija el tipo de evento")]
        public string tipo { get; set; }

        [Required(ErrorMessage = "Es necesario que ingrese el cupo maximo")]
        [Display(Name = "Ingrese el cupo maximo")]
        [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "Por favor ingrese solo numeros")]
        public int cupoMaximo { get; set; }

        [Required(ErrorMessage = "Es necesario que ingrese su email para crear el evento")]
        [RegularExpression("^[_A-Za-z0-9-]+(.[_a-z0-9-]+)*@[a-z0-9-]+(.[a-z0-9-]+)*(.[a-z]{2,4})$", ErrorMessage = "Por favor ingrese solo letras y numeros")]
        [Display(Name = "Ingrese su email")]
        public string emailCoordinador { get; set; }

        public List<Sector> sectores { get; set; } = new List<Sector>();


        public string retorneElNombreDelMes(int numero)
        {
            string nombreDelMes = "";
            switch (numero.ToString())
            {
                case "1":
                    nombreDelMes = "Enero";
                    break;
                case "2":
                    nombreDelMes = "Febrero";
                    break;
                case "3":
                    nombreDelMes = "Marzo";
                    break;
                case "4":
                    nombreDelMes = "Abril";
                    break;
                case "5":
                    nombreDelMes = "Mayo";
                    break;
                case "6":
                    nombreDelMes = "Junio";
                    break;
                case "7":
                    nombreDelMes = "Julio";
                    break;
                case "8":
                    nombreDelMes = "Agosto";
                    break;
                case "9":
                    nombreDelMes = "Septiembre";
                    break;
                case "10":
                    nombreDelMes = "Octubre";
                    break;
                case "11":
                    nombreDelMes = "Noviembre";
                    break;
                case "12":
                    nombreDelMes = "Diciembre";
                    break;
            }
            return nombreDelMes;
        }
    }
}
