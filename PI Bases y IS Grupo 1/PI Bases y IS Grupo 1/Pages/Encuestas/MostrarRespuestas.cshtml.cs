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
    public class MostrarRespuestasModel : PageModel
    {

        [BindProperty]
        public List<PreguntaModel> listaPreguntas { get; set; }

        [BindProperty]
        public List<int> listaRespuestaOpciones { get; set; }
        public void OnGet(int id)
        {
            ViewData["idEncuesta"] = id;
            try
            {
                PreguntasHandler accesoDatosPregunta = new PreguntasHandler();
                RespuestasHandler accesoDatosRespuesta = new RespuestasHandler();
                listaPreguntas = accesoDatosPregunta.obtenerPreguntas(id);
                foreach (var item in listaPreguntas)
                {
                    listaRespuestaOpciones.Add(accesoDatosRespuesta.cantidadVecesElegidaUnaOpcion(item.encuestaID, item.preguntaID, item.opcion1));
                    listaRespuestaOpciones.Add(accesoDatosRespuesta.cantidadVecesElegidaUnaOpcion(item.encuestaID, item.preguntaID, item.opcion2));
                    listaRespuestaOpciones.Add(accesoDatosRespuesta.cantidadVecesElegidaUnaOpcion(item.encuestaID, item.preguntaID, item.opcion3));
                    listaRespuestaOpciones.Add(accesoDatosRespuesta.cantidadVecesElegidaUnaOpcion(item.encuestaID, item.preguntaID, item.opcion4));
                }
            }
            catch {
                ViewData["Mensaje"] = "Aun no hay respuestas";

            }
        }
    }
}