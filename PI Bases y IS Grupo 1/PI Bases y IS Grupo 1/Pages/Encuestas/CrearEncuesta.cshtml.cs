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
            accesodatos.crearEncuesta(encuesta);      
            return Redirect("~/Encuestas/PaginaInicioEncuesta");
        }
    }
}