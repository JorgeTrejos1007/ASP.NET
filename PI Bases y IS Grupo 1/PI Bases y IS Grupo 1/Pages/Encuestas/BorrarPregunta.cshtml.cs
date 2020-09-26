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
    public class BorrarPreguntaModel : PageModel
    {
        [BindProperty]
        public PreguntaModel pregunta { get; set; }
        public void OnGet(int idEnc, int idPreg)
        {
            PreguntasHandler accesodatos = new PreguntasHandler();
            this.pregunta = accesodatos.obtenerTuplaPregunta(idEnc, idPreg);
            ViewData["idEncuesta"] = idEnc;
        }
        public IActionResult OnPostBorrarPregunta()
        {
            PreguntasHandler accesodatos = new PreguntasHandler();
            accesodatos.borrarPregunta(pregunta.encuestaID ,pregunta.preguntaID);
            return RedirectToPage("ListaPreguntas", new { id = pregunta.encuestaID });
        }
    }
}