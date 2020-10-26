using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PIBasesISGrupo1.Models;
using PIBasesISGrupo1.Handler;
using PIBasesISGrupo1.Filters;

namespace PIBasesISGrupo1.Pages.Encuestas
{
    [PermisosDeVista("Miembro de Nucleo", "Coordinador")]
    public class BorrarEncuestaModel : PageModel
    {
        [BindProperty]
        public EncuestaModel encuesta { get; set; }
        public void OnGet(int id)
        {
            EncuestasHandler accesodatos = new EncuestasHandler();
            this.encuesta = accesodatos.obtenerTuplaEncuesta(id);
        }
        public IActionResult OnPostBorrarEncuesta()
        {
            EncuestasHandler accesoDatos = new EncuestasHandler();
            accesoDatos.borrarEncuesta(encuesta.id);
            return Redirect("~/Encuestas/PaginaInicioEncuesta");
        }

    }
}