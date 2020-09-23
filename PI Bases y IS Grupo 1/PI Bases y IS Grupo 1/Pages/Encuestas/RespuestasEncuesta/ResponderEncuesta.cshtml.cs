using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PIBasesISGrupo1.Models;
using PIBasesISGrupo1.Handler;

namespace PIBasesISGrupo1.Pages.Encuestas.RespuestasEncuesta
{
    public class ResponderEncuestaModel : PageModel
    {

        [BindProperty]
        public RespuestaModel respuesta { get; set; }

        [BindProperty]
        public EncuestaModel encuesta { get; set; }

        public void OnGet(int idEnc)
        {
            EncuestasHandler accesoDatosEncuesta = new EncuestasHandler();
            encuesta = accesoDatosEncuesta.obtenerTuplaEncuesta(idEnc);
            PreguntasHandler accesoDatosPregunta = new PreguntasHandler();
            ViewData["Preguntas"] = accesoDatosPregunta.obtenerPreguntas(idEnc);
        }
    }
}