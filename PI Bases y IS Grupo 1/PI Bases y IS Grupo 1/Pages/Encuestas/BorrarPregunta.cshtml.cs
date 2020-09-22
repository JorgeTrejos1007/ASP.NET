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
        public void OnGetBorrarPregunta(int idEncuesta, int idPregunta)
        {
            PreguntasHandler accesodatos = new PreguntasHandler();
            this.pregunta = accesodatos.obtenerTuplaPregunta(idEncuesta, idPregunta);
        }
        public IActionResult OnPostBorrarEncuesta()
        {
            PreguntasHandler accesodatos = new PreguntasHandler();
            accesodatos.borrarPregunta(pregunta.preguntaID);
            return Redirect("~/Encuestas/ListaPreguntas");
        }
    }
}