using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PIBasesISGrupo1.Handler;
using PIBasesISGrupo1.Models;
using System.Net.Mail;

namespace PIBasesISGrupo1.Pages.Eventos
{
    public class RegistrarmeEnEventoPresencialNumeradoModel : PageModel
    {

        EventoHandler baseDeDatosHandler = new EventoHandler();

        [BindProperty]
        public InformacionDeRegistroEnEvento registro { get; set; }

        public IActionResult OnGet()
        {
            IActionResult vista;
            try {
                string emailCoordinador = (string)TempData["emailCoordinador"];
                string nombreEvento = (string)TempData["nombreEvento"];
                DateTime fechaYHora = (DateTime)TempData["fechaEvento"];
                string lugar = (string)TempData["lugarEvento"];
                TempData["nombreLugar"] = lugar;
                ViewData["nombreEvento"] = nombreEvento;
                ViewData["fechaYHora"] = fechaYHora;
                ViewData["lugarEvento"] = lugar;
                ViewData["emailCoordinador"] = emailCoordinador;
                List<Sector> listaSectoresNoNumerados = new List<Sector>();
                listaSectoresNoNumerados = baseDeDatosHandler.obtenerSectoresNoNumeradosEventoPresencial(emailCoordinador,nombreEvento,fechaYHora);
                ViewData["listaSectoresNoNumerados"] = listaSectoresNoNumerados;

                List<Sector> listaSectoresNumerados = new List<Sector>();            
                listaSectoresNumerados = baseDeDatosHandler.obtenerSectoresNumeradosEventoPresencial(emailCoordinador, nombreEvento, fechaYHora);
                ViewData["listaSectoresNumerados"] = listaSectoresNumerados;


                
                vista = Page();
            }
            catch
            {
                vista = Redirect("~/Error");
            }
            return vista;
        }

        public IActionResult OnPostElegirAsientos(string nombreSectorElegido, string nombreEvento, string emailCoordinador, DateTime fechaYHora)
        {
            Sector sector = new Sector();
            sector.asientosDisponibles = baseDeDatosHandler.asientosDisponiblesEnSectorNumerado(emailCoordinador, nombreEvento, fechaYHora, nombreSectorElegido);
            return new JsonResult(sector);
        }

        public IActionResult OnPostRegistrarmeEnElEvento() {
            IActionResult vista;

            if(registro.tipoDeSector == "Numerado") {
                registro.asientosDeseados = convertirAsientosElegidosALista(registro.asientosElegidos);
            }
            

            vista = Redirect("~/index");

            var miembro = Sesion.obtenerDatosDeSesion(HttpContext.Session, "Miembro");
            bool exito = true;

            if (registro.tipoDeSector == "Numerado") {
                exito = baseDeDatosHandler.transaccionReservarAsientosNumerados(registro, miembro.email);
                if (exito == false) {
                    vista = Redirect("~/index");
                }
            }
            else
            {
                exito = baseDeDatosHandler.transaccionReservarAsientosNoNumerados(registro);
                if (exito == false) {
                    vista = Redirect("~/index");
                }
            }

            if (exito)
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("comunidad.practica.g1@gmail.com");
                mail.To.Add(miembro.email);
                mail.Subject = "Entradas para el evento " + registro.nombreEvento;


                mail.Body = "Hola, usted ha sido registrado en un evento. Los detalles de se registro se muestran a continuación:\n\n";
                mail.Body += "Evento: " + registro.nombreEvento + ".\n";
                mail.Body += "Sector: " + registro.nombreSector + ".\n";

                if (registro.tipoDeSector == "Numerado")
                {
                    mail.Body += "Asientos: ";
                    for (int index = 0; index < registro.asientosDeseados.Count; index++)
                    {
                        mail.Body += registro.asientosDeseados[index].ToString() + " ";
                    }
                }
                else
                {
                    mail.Body += "Cantidad de asientos: " + registro.cantidadAsientos.ToString();
                }

                mail.Body += ".\nFecha: " + registro.fechaYHora.ToString("dd/MM/yyyy") + ".\n";
                mail.Body += "Hora: " + registro.fechaYHora.ToString("HH:mm");
                mail.Body += "\nLugar: " + (string)TempData["nombreLugar"] + ".\n\n";
                mail.Body += "Presente este correo en la entrada el día del evento para poder ingresar.\nTe esperamos!";

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("comunidad.practica.g1@gmail.com", "AdriancitoG1.");
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
            }

            return vista;
        }

        public List<int> convertirAsientosElegidosALista(string asientos)
        {
            List<int> asientosDisponibles = new List<int>();
            string[] arregloAsientos = asientos.Split(",");

            for(int i = 0; i < arregloAsientos.Length; i++)
            {
                asientosDisponibles.Add(Convert.ToInt32(arregloAsientos[i]));
            }

            return asientosDisponibles;
        }
    }
}