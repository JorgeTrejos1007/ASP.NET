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
        [RegularExpression(@"([a-zA-Z0-9\s_\\.\-:()])+(.png|.jpg)$", ErrorMessage = "Ingrese una imagen png o jpg")]
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
            if (TempData["MensajeError"] == null)
            {
                TempData["MensajeError"] = "";
            }
        }

        public IActionResult OnPostCrearEventoVirtual()
        {
            IActionResult vista;
            try
            {               
                evento.fechaYHora = Convert.ToDateTime(concatenarFechaYHora(fecha, hora));
                if (fechaYHoraValida(evento.fechaYHora)) {
                    Miembro datosDelMiembro = Sesion.obtenerDatosDeSesion(HttpContext.Session, User.Identity.Name);
                    evento.emailCoordinador = datosDelMiembro.email;
                    EventoHandler accesoDatos = new EventoHandler();
                    if (accesoDatos.registrarEvento(evento, imagenEvento))
                    {
                        if (accesoDatos.registrarEventoVirtual(evento))
                        {
                            vista = Redirect("~/Eventos/MostrarEventos");
                        }
                        else
                        {
                            vista = Redirect("~/Eventos/CrearEvento");
                        }
                    }
                    else
                    {
                        vista = Redirect("~/Eventos/CrearEvento");
                        TempData["MensajeError"] = "Este evento ya fue creado anteriormente";
                    }
                }
                else
                {
                    vista = Redirect("~/Eventos/CrearEvento");
                }
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
                evento.emailCoordinador = datosDelMiembro.email;
                evento.fechaYHora = Convert.ToDateTime(concatenarFechaYHora(fecha, hora));
                TempData["nombreEvento"] = evento.nombre;
                TempData["fechaYHora"] = evento.fechaYHora;
                EventoHandler accesoDatos = new EventoHandler();
                accesoDatos.registrarEvento(evento, imagenEvento);
                accesoDatos.registrarEventoPresencial(evento);

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
            return fecha + " " + hora;
        }

        public bool fechaYHoraValida(DateTime fechaYHora)
        {
            bool fechaValida = false;
            if (fechaYHora > DateTime.UtcNow)
            {
                fechaValida = true;
            }
            return fechaValida;

        }
    }
}