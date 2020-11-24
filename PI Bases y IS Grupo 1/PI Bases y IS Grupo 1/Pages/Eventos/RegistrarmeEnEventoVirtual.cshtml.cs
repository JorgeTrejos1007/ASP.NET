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
        
        public void OnPostRegistrarmeEnElEvento()
        {
        }
    }
}