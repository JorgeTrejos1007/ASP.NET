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
    public class EditarEncuestaModel : PageModel
    {
        [BindProperty]
        public EncuestaModel encuesta { get; set; }
        public void OnGet(int id)
        {
            EncuestasHandler accesodatos = new EncuestasHandler();
            this.encuesta = accesodatos.obtenerTuplaEncuesta(id);

            CatalogoHandler accesoCatalago = new CatalogoHandler();
            ViewData["TopicosYCategorias"] = accesoCatalago.obteneTodosLosTopicosYCategoriasAsociadas();
        }
        public IActionResult OnPostModificarEncuesta()
        {
            EncuestasHandler accesodatos = new EncuestasHandler();
            if (accesodatos.modificarEncuesta(encuesta))
            {
                TempData["mensaje"] = "Encuesta editada con exito";
                TempData["exitoAlEditar"] = true;
            }
            else
            {
                TempData["mensaje"] = "Algo salió mal y no fue posible editar la encuesta :(";
                TempData["exitoAlEditar"] = false;
            }
            return Redirect("~/Encuestas/PaginaInicioEncuesta");
        }
    }
}