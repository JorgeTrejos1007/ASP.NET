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
    public class RegistrarmeEnEventoPresencialNumeradoModel : PageModel
    {

        [BindProperty]
        public Evento evento { get; set; }
        public IActionResult OnGet()
        {
            IActionResult vista;
            try
            {
                if(TempData["nombreEvento"] != null) {
                    ViewData["nombreEvento"] = TempData["nombreEvento"];
                    ViewData["fechaEvento"] = TempData["fechaEvento"];
                    ViewData["emailCoordinador"] = TempData["emailCoordinador"];
                    ViewData["lugarEvento"] = TempData["lugarEvento"];

                    EventoHandler accesoDatos = new EventoHandler();
                    string emailCoordinador = (string)ViewData["emailCoordinador"];
                    string nombreEvento = (string)ViewData["nombreEvento"];
                    DateTime fechaYHora = (DateTime)ViewData["fechaEvento"];
                    ViewData["listaSectores"] = accesoDatos.obtenerSectoresEventoPresencial(emailCoordinador,nombreEvento,fechaYHora);
                    vista = Page();
                }
                else
                {
                    vista = Redirect("~/Error");
                }
                
            }
            catch
            {
                vista = Redirect("~/Error");
            }
            return vista;
        }
    }
}