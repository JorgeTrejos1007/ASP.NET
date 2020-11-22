using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PIBasesISGrupo1.Models;
using PIBasesISGrupo1.Handler;
using System.Globalization;

namespace PIBasesISGrupo1.Pages.Eventos
{



    public class MostrarEventosModel : PageModel
    {
        [BindProperty]
        public Evento evento { get; set; }
        public IActionResult OnGet()
        {
            IActionResult vista;
            try
            {
                EventoHandler consultasEvento = new EventoHandler();
                List<Evento> eventosPresenciales = consultasEvento.obtenerTodosLosEventosPresenciales();
                List<Evento> eventosVirtuales = consultasEvento.obtenerTodosLosEventosVirtuales();

                if (eventosPresenciales.Any())
                {
                    ViewData["EventosPresenciales"] = eventosPresenciales;
                }
                if (eventosPresenciales.Any())
                {
                    ViewData["EventosVirtuales"] = eventosVirtuales;
                }
                vista = Page();
            }
            catch
            {
                vista = Redirect("~/Error");
            }
            return vista;
        }

        public IActionResult OnPostRegistrarmeEnElEventoPresencial(string nombre, string lugar, DateTime fechaYHora, string emailCoordinador)
        {
            IActionResult vista;
            try                   
            {
                TempData["nombreEvento"] = evento.nombre;
                TempData["lugarEvento"] = evento.lugar;
                TempData["fechaEvento"] = evento.fechaYHora;
                TempData["emailCoordinador"] = evento.emailCoordinador;              
                vista = Redirect("~/Eventos/RegistrarmeEnEventoPresencialNumerado");
            }
            catch
            {
                vista = Redirect("~/Error");
            }
            return vista;
        }
    }
}