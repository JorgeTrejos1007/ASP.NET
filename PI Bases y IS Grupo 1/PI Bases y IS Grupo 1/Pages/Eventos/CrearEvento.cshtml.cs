using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PIBasesISGrupo1.Models;
using PIBasesISGrupo1.Handler;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;


namespace PIBasesISGrupo1.Pages.Eventos
{
    public class CrearEventoModel : PageModel
    {
        [BindProperty]
        public Evento evento { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Es necesario que ingreses una imagen del evento")]
        public IFormFile imagenEvento { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Es necesario que ingreses la fecha del evento")]
        [Display(Name = "Ingrese la fecha del evento")]
        public string fecha { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Es necesario que ingreses la hora del evento")]
        [Display(Name = "Ingrese la hora del evento")]
        public string hora { get; set; }

        public void OnGet()
        {

        }

        public IActionResult OnPostCrearEventoVirtual()
        {
            IActionResult vista;
            try {
                Miembro datosDelMiembro = Sesion.obtenerDatosDeSesion(HttpContext.Session, User.Identity.Name);
                evento.emailCoordinador = datosDelMiembro.email;
                evento.fechaYHora = Convert.ToDateTime(concatenarFechaYHora(fecha, hora));
                EventoHandler accesoDatos = new EventoHandler();
                accesoDatos.registrarEvento(evento, imagenEvento);
                accesoDatos.registrarEventoVirtual(evento);
                vista = Redirect("~/Eventos/MostrarEventos");
            }
            catch
            {
                vista = Redirect("~/Error");
            }
            return vista;
            
        }

        public IActionResult OnPostCrearSeccionEventoPresencial()
        {
            IActionResult vista;
            try
            {

                Miembro datosDelMiembro = Sesion.obtenerDatosDeSesion(HttpContext.Session, User.Identity.Name); 
                TempData["emailCoordinador"] = datosDelMiembro.email;
                TempData["nombreEvento"] = evento.nombre;
                TempData["fechaYHora"] = Convert.ToDateTime(concatenarFechaYHora(fecha, hora));
                vista = Redirect("~/Eventos/CrearSectores");
            }
            catch
            {
                vista = Redirect("~/Error");
            }
            return vista;
        }

        public string concatenarFechaYHora(string fecha, string hora)
        {     
            return fecha+" "+hora;
        }
    }
}