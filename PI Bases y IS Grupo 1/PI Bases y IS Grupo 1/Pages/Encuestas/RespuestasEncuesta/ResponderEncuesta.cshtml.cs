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

        [BindProperty]
        public List<PreguntaModel> preguntas { get; set; }

        [BindProperty]
        public PreguntaModel preguntaActual { get; set; }

        [BindProperty]
        public int idSiguientePregunta { get; set; }

        public void OnGet(int idEnc, int idPreg, int indexSiguiente)
        {
            EncuestasHandler accesoDatosEncuesta = new EncuestasHandler();
            encuesta = accesoDatosEncuesta.obtenerTuplaEncuesta(idEnc);
            if (idPreg == 0) {
                PreguntasHandler accesoDatosPregunta = new PreguntasHandler();
                preguntas = accesoDatosPregunta.obtenerPreguntas(idEnc);

                preguntaActual = preguntas[0];
                idPreg = preguntaActual.preguntaID;
                idSiguientePregunta = preguntas[indexSiguiente].preguntaID;
                ViewData["idSiguiente"] = idSiguientePregunta;
            }
            else
            {
                PreguntasHandler accesoDatosPregunta = new PreguntasHandler();
                preguntaActual = accesoDatosPregunta.obtenerTuplaPregunta(idEnc, idPreg);
            }
        }

        public IActionResult OnPostRespuesta( )
        {   
            respuesta.respuesta = String.Format("{0}", Request.Form["opcion"]);
            RespuestasHandler accesodatos = new RespuestasHandler();
            accesodatos.crearRespuesta(respuesta);
            return RedirectToPage("ResponderEncuesta", new { idEnc = preguntaActual.encuestaID, idPreg = preguntaActual.preguntaID });
        }
    }
}