using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PIBasesISGrupo1.Handler;
using PIBasesISGrupo1.Models;
using System.Net.Mail;
using System.ComponentModel.DataAnnotations;

namespace PIBasesISGrupo1.Pages.Eventos
{
    public class RegistrarmeEnEventoVirtualModel : PageModel
    {
        EventoHandler baseDeDatosHandler = new EventoHandler();

        [BindProperty]
        public Evento evento { get; set; }

        
        [BindProperty]
        [Required(ErrorMessage = "Es necesario que ingreses los cupos que deseas")]
        public int cuposSolicitados { get; set; }

        public IActionResult OnGet()
        {
            IActionResult vista;
            try
            {
                string emailCoordinador = (string)TempData["emailCoordinador"];
                string nombreEvento = (string)TempData["nombreEvento"];
                DateTime fechaYHora = (DateTime)TempData["fechaEvento"];
                string canalDeStream = (string)TempData["canalDeStream"];
                ViewData["nombreEvento"] = nombreEvento;
                ViewData["fechaYHora"] = fechaYHora;
                ViewData["canalDeStream"] = canalDeStream;
                ViewData["emailCoordinador"] = emailCoordinador;
                ViewData["cuposDisponibles"] = baseDeDatosHandler.cuposDisponiblesEventoVirtual(emailCoordinador, nombreEvento, fechaYHora, canalDeStream);
                vista = Page();
            }
            catch
            {
                vista = Redirect("~/Error");
            }
            return vista;
        }
        
        public IActionResult OnPostRegistrarmeEnElEvento()
        {
            IActionResult vista;
            EventoHandler eventoHandler = new EventoHandler();
            var miembro = Sesion.obtenerDatosDeSesion(HttpContext.Session, "Miembro");

            bool exito = eventoHandler.transaccionReservarCuposEventoVirtual(evento, cuposSolicitados);

            if (exito)
            {
                vista = Redirect("~/Eventos/MostrarEventos");
                TempData["mensaje"] = "Su registro ha sido exitoso. Se le enviara la información a su correo";

                string url = "http://edustage.azurewebsites.net/Eventos/StreamDeEvento?nombreE=" + evento.nombre.Replace(" ", "%20") + "&nombreCanal=" + evento.nombreCanalStream.Replace(" ", "%20");
                //string url = "https://localhost:44326/Eventos/StreamDeEvento?nombreE=" + evento.nombre.Replace(" ","%20") + "&nombreCanal=" + evento.nombreCanalStream.Replace(" ", "%20");

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("comunidad.practica.g1@gmail.com");
                mail.To.Add(miembro.email);
                mail.Subject = "Link para el evento: " + evento.nombre;

                mail.Body = "Hola, usted ha sido registrado en un evento virtual. Los detalles de su registro se muestran a continuación:\n\n";
                mail.Body += "Cantidad de cupos: " + cuposSolicitados.ToString();
                mail.Body += ".\nFecha: " + evento.fechaYHora.ToString("dd/MM/yyyy") + ".\n";
                mail.Body += "Hora: " + evento.fechaYHora.ToString("HH:mm");
                mail.Body += ".\nCanal de twitch: " + evento.nombreCanalStream + ".\n";
                mail.Body += "Link para ingresar al stream: " + url + ".\n\n";
                mail.Body += "Gracias por participar en este evento.\nTe esperamos!";

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("comunidad.practica.g1@gmail.com", "AdriancitoG1.");
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
            }
            else {
                TempData["mensaje"] = "Ups! Usted a pedido una cantidad de cupos invalida, vuelva a intentarlo";
                TempData["nombreEvento"] = evento.nombre;
                TempData["fechaEvento"] = evento.fechaYHora;
                TempData["canalDeStream"] = evento.nombreCanalStream;
                TempData["emailCoordinador"] = evento.emailCoordinador;
                vista = Redirect("~/Eventos/RegistrarmeEnEventoVirtual");
            }

            return vista;
        }
    }
}