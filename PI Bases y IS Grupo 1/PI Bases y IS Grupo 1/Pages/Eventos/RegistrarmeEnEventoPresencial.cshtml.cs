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
        [BindProperty]
        public Evento evento { get; set; }

        EventoHandler baseDeDatosHandler = new EventoHandler();
        List<Sector> sectores = new List<Sector>();

        [BindProperty]
        public InformacionDeRegistroEnEvento registro { get; set; }


        public void OnGet()
        {
            string emailCoordinador = (string)TempData["emailCoordinador"];
            string nombreEvento = (string)TempData["nombreEvento"];
            DateTime fechaYHora = (DateTime)TempData["fechaEvento"];
            string lugar = (string)TempData["lugarEvento"];
            TempData["nombreLugar"] = lugar;

            ViewData["nombreEvento"] = nombreEvento;
            ViewData["fechaYHora"] = fechaYHora;
            ViewData["lugar"] = lugar;
            ViewData["emailCoordinador"] = emailCoordinador;
            DateTime fecha = Convert.ToDateTime(fechaYHora);
            ViewData["listaSectores"] = baseDeDatosHandler.obtenerSectoresEventoPresencial(emailCoordinador, nombreEvento, fecha);

            sectores = baseDeDatosHandler.obtenerSectoresEventoPresencial(emailCoordinador, nombreEvento, fecha);

            for (int index = 0; index < sectores.Count; index++) {
                if (sectores[index].tipo == "Numerado") {
                    sectores[index].asientosDisponibles = baseDeDatosHandler.asientosDisponiblesEnSector(emailCoordinador, nombreEvento, fecha, sectores[index].nombreDeSector);
                }
            }
        }

        public IActionResult OnPost () {
            IActionResult vista;

            // datos de prueba
            InformacionDeRegistroEnEvento info = new InformacionDeRegistroEnEvento();
            info.nombreEvento = "Adriancito con Daddy Yankee en concierto";
            info.emailCoordinador = "stevegc112016@gmail.com";
            info.nombreSector = "China";
            info.fechaYHora = Convert.ToDateTime("2020-11-25 17:13:00.000");
            info.tipoDeSector = "No numerado";
            info.cantidadAsientos = 3;
            /*List<int> asientos = new List<int>();
            asientos.Add(2);
            asientos.Add(4);
            asientos.Add(5);
            info.asientosDeseados = asientos;*/

            var miembro = Sesion.obtenerDatosDeSesion(HttpContext.Session, "Miembro");
            vista = Redirect("~/index");

            bool exito = true;

            if (info.tipoDeSector == "Numerado") {
                exito = baseDeDatosHandler.transaccionReservarAsientosNumerados(info, miembro.email);
                if (exito == false) {
                    vista = Redirect("~/index");
                }
            }
            else
            {
                exito = baseDeDatosHandler.transaccionReservarAsientosNoNumerados(info);
                if (exito == false) {
                    vista = Redirect("~/index");
                }
            }

            if (exito) {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("comunidad.practica.g1@gmail.com");
                mail.To.Add(miembro.email);
                mail.Subject = "Entradas para el evento " + info.nombreEvento;


                mail.Body = "Hola, has sido registrado en un evento. Los detalles de tu registro se muestran a continuación:\n\n";
                mail.Body += "Evento: " + info.nombreEvento + ".\n";
                mail.Body += "Sector: " + info.nombreSector + ".\n";

                if (info.tipoDeSector == "Numerado")
                {
                    mail.Body += "Asientos: ";
                    for (int index = 0; index < info.asientosDeseados.Count; index++)
                    {
                        mail.Body += info.asientosDeseados[index].ToString() + " ";
                    }
                }
                else
                {
                    mail.Body += "Cantidad de asientos: " + info.cantidadAsientos.ToString();
                }

                mail.Body += ".\n Fecha: " + info.fechaYHora.Date.ToString() + ".\n";
                mail.Body += "Hora: " + info.fechaYHora.Hour.ToString() + ":" + info.fechaYHora.Minute.ToString();
                mail.Body += "Lugar: " + (string)TempData["nombreLugar"] + ".\n\n";
                mail.Body += "Presente este correo en la entrada el día del evento para poder ingresar.\n Te esperamos!"; 

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("comunidad.practica.g1@gmail.com", "AdriancitoG1.");
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
            }

            return vista;
        }
    }
}