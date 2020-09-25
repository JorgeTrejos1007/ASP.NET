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

      

        public IActionResult OnGet(int idEnc, int indexSig)
        {
            EncuestasHandler accesoDatosEncuesta = new EncuestasHandler();
            encuesta = accesoDatosEncuesta.obtenerTuplaEncuesta(idEnc);
            PreguntasHandler accesoDatosPregunta = new PreguntasHandler();
            preguntas = accesoDatosPregunta.obtenerPreguntas(idEnc);
            if (preguntas.Count > indexSig)
            {
                if (indexSig == 0)
                {

                    preguntaActual = preguntas[0];

                    ViewData["indexSig"] = indexSig; // =0
                    return Page();
                }
                else
                {
                    preguntaActual = preguntas[indexSig];
                    ViewData["indexSig"] = indexSig;
                    return Page();
                }
            }
            else {
                return Redirect("../../Index");
            }
        }

        public IActionResult OnPostRespuesta(int indexSig)
        {   
            respuesta.respuesta = String.Format("{0}", Request.Form["opcion"]);
            RespuestasHandler accesodatos = new RespuestasHandler();
            accesodatos.crearRespuesta(respuesta);
            return RedirectToPage("ResponderEncuesta", new { idEnc = preguntaActual.encuestaID, indexSig = indexSig + 1 });
        }
    }
}