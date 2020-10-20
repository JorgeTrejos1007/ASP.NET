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
    public class CrearEncuestaModel : PageModel
    {
        [BindProperty]
        public EncuestaModel encuesta { get; set; }
       
        public void OnGet()
        {
           CatalogoHandler accesoCatalago = new CatalogoHandler();
            ViewData["TopicosYCategorias"] = accesoCatalago.obteneTodosLosTopicosYCategoriasAsociadas();

        }
        public IActionResult OnPostEncuesta()
        {
            EncuestasHandler accesodatos = new EncuestasHandler();
            accesodatos.crearEncuesta(encuesta);      
            return Redirect("~/Encuestas/PaginaInicioEncuesta");
        }
    }
}