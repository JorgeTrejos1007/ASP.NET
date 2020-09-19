using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PIBasesISGrupo1.Models;
using PIBasesISGrupo1.Handler;

namespace PIBasesISGrupo1.Pages.Encuestas
{
    public class CrearEncuestaModel : PageModel
    {
       [BindProperty]
        public EncuestaModel encuesta { get; set; }
        public void OnGet()
        {

        }
        public IActionResult OnPostEncuesta()
        {
            EncuestasHandler accesodatos = new EncuestasHandler();
            if (accesodatos.crearEncuesta(encuesta))
            {
                TempData["mensaje"] = "Informacion editada con exito";
                TempData["exitoAlEditar"] = true;
            }
            else
            {
                TempData["mensaje"] = "Algo salió mal y no fue posible editadar la informacion :(";
                TempData["exitoAlEditar"] = false;
            }
            return Redirect("~/Encuestas/PaginaInicioEncuesta");
        }
    }
}