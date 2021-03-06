using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PIBasesISGrupo1.Handler;
using PIBasesISGrupo1.Models;
using System.Net.Mail;
using ClosedXML.Excel;
using System.IO;
using PIBasesISGrupo1.Filters;

namespace PIBasesISGrupo1.Pages.Encuestas
{
    [PermisosDeVista("Miembro de Nucleo","Coordinador")]
    public class PaginaInicioEncuestaModel : PageModel
    {

        public List<EncuestaModel> Encuesta { get; set; }
        public void OnGet()
        {
            var miembroEnSesion = Sesion.obtenerDatosDeSesion(HttpContext.Session, "Miembro");
             EncuestasHandler accesoDatos = new EncuestasHandler();
            ViewData["Encuestas"] = accesoDatos.obtenerEncuestas(miembroEnSesion.email);
        }

         public IActionResult OnPostCompartir(int id)
        {
            
        
                EncuestasHandler accesoDatos = new EncuestasHandler();
                List<string> miembrosEmail = accesoDatos.obtenerTodosLosEmails();
                EncuestaModel encuesta = accesoDatos.obtenerTuplaEncuesta(id);
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                string url = "http://edustage.azurewebsites.net/Encuestas/RespuestasEncuesta/ResponderEncuesta?idEnc="+ id.ToString()+"&indexPregunta=0";
                mail.From = new MailAddress("comunidad.practica.g1@gmail.com");
                foreach (var email in miembrosEmail)
                {
                    mail.To.Add(email);
                }
                mail.Subject = "Encuesta disponible: " + encuesta.nombreEncuesta;
                mail.Body = "Hola te invitamos a responder la siguiente encuesta " + url
                +", tiene una vigencia de "+encuesta.vigencia+" dias";
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("comunidad.practica.g1@gmail.com", "AdriancitoG1.");
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
      
             return RedirectToAction("~/Encuestas/PaginaInicioEncuesta");

        }

        public IActionResult OnPostExportarRespuestas(int idEncuesta)
        {
            PreguntasHandler accesoDatosPregunta = new PreguntasHandler();
            RespuestasHandler accesoDatosRespuesta = new RespuestasHandler();

            List<PreguntaModel> listaPreguntas = accesoDatosPregunta.obtenerPreguntas(idEncuesta); ;
            List<MostarRespuestaModel> respuestasDeEncuestados = accesoDatosRespuesta.obtenerRespuestas(idEncuesta);

            var registroDeRespuestas = new XLWorkbook();
            var hojaDeRespuestas = registroDeRespuestas.Worksheets.Add("Respuestas");

            int filaPregunta = 1;
            int columnaPregunta= 1;

            foreach (var pregunta in listaPreguntas) {   
                hojaDeRespuestas.Cell(filaPregunta, columnaPregunta).Value = pregunta.pregunta;
                columnaPregunta++;
            }

            int filaRespuesta = 2;
            columnaPregunta = 0;
            string preguntaActual = " ";

            foreach (var respuestaEncuestado in respuestasDeEncuestados)
            {
                if (preguntaActual == respuestaEncuestado.pregunta)
                {
                    filaRespuesta = filaRespuesta + 1;
                }
                else
                {
                    preguntaActual = respuestaEncuestado.pregunta;
                    filaRespuesta = 2;
                    columnaPregunta = columnaPregunta + 1;
                    
                }
                hojaDeRespuestas.Cell(filaRespuesta, columnaPregunta).Value = respuestaEncuestado.respuesta;
            }

            var datosEnMemoria = new MemoryStream();
            registroDeRespuestas.SaveAs(datosEnMemoria);
            var contenido = datosEnMemoria.ToArray();
            return File(contenido, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Respuestas.xlsx");
        }



    }
}