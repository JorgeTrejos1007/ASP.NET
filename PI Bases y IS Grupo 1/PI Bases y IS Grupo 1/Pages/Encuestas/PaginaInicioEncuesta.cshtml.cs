using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PIBasesISGrupo1.Handler;
using PIBasesISGrupo1.Models;

namespace PIBasesISGrupo1.Pages.Encuestas
{
    public class PaginaInicioEncuestaModel : PageModel
    {

        public List<EncuestaModel> Encuesta { get; set; }
        public void OnGet()
        {
            EncuestasHandler accesoDatos = new EncuestasHandler();
            ViewData["Encuestas"] = accesoDatos.obtenerEncuestas();
        }
    }
}