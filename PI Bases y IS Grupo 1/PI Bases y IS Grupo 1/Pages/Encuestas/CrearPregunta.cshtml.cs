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
    public class CrearPreguntaModel : PageModel
    {
        [BindProperty]
        public PreguntaModel pregunta { get; set; }

        public int idEncuesta;
        public void OnGet(int id)
        {
            ViewData["idEncuesta"] = id; 
        }
        public IActionResult OnPostPregunta()
        {

            PreguntasHandler accesodatos = new PreguntasHandler();
            accesodatos.crearPregunta(pregunta);

            return RedirectToPage("ListaPreguntas", new { id = pregunta.encuestaID });
        }
    }
}