using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PIBasesISGrupo1.Handler;
using PIBasesISGrupo1.Models;
using System.Net.Mail;

namespace PIBasesISGrupo1.Pages.Encuestas
{
    public class PaginaInicioEncuestaModel : PageModel
    {

        public List<EncuestaModel> Encuesta { get; set; }
        public void OnGet()
        {
            EncuestasHandler accesoDatos = new EncuestasHandler();
            ViewData["Encuestas"] = accesoDatos.obtenerEncuestas();
        }

         public IActionResult OnPostCompartir(int id)        {
                                    EncuestasHandler accesoDatos = new EncuestasHandler();                List<string> miembrosEmail = accesoDatos.obtenerTodosLosEmails();                EncuestaModel encuesta = accesoDatos.obtenerTuplaEncuesta(id);                MailMessage mail = new MailMessage();                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");                string url = "https://localhost:44326/Encuestas/RespuestasEncuesta/ResponderEncuesta?idEnc="+ id.ToString()+"&indexPregunta=0" ;                mail.From = new MailAddress("comunidad.practica.g1@gmail.com");                foreach (var email in miembrosEmail)                {                    mail.To.Add(email);                }                mail.Subject = "Encuesta disponible: " + encuesta.nombreEncuesta;                mail.Body = "Hola te invitamos a responder la siguiente encuesta " + url                +", tiene una vigencia de "+encuesta.vigencia+" dias";                SmtpServer.Port = 587;                SmtpServer.Credentials = new System.Net.NetworkCredential("comunidad.practica.g1@gmail.com", "AdriancitoG1.");                SmtpServer.EnableSsl = true;                SmtpServer.Send(mail);                   return RedirectToAction("~/Encuestas/PaginaInicioEncuesta");

        }



    }
}