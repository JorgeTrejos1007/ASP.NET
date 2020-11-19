using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PIBasesISGrupo1.Models;
using PIBasesISGrupo1.Handler;
using System.Globalization;

namespace PIBasesISGrupo1.Pages.Eventos {



    public class MostrarEventosModel : PageModel
    {
        [BindProperty]
        public Evento evento { get; set; }
        public void OnGet()
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
        }
    }
}