using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PIBasesISGrupo1.Models;
using PIBasesISGrupo1.Handler;
using System.Text;
using ClosedXML.Excel;
using System.IO;

namespace PIBasesISGrupo1.Pages.Encuestas
{
    public class MostrarRespuestasModel : PageModel
    {

        [BindProperty]
        public List<PreguntaModel> listaPreguntas { get; set; }

        public void OnGet(int id)
        {

            
            ViewData["idEncuesta"] = id;
            try
            {
                var listaConteoRespuestasPorOpcion = new List<int>();
                PreguntasHandler accesoDatosPregunta = new PreguntasHandler();
                RespuestasHandler accesoDatosRespuesta = new RespuestasHandler();
                listaPreguntas = accesoDatosPregunta.obtenerPreguntas(id);
                foreach (var item in listaPreguntas)
                {
                    listaConteoRespuestasPorOpcion.Add(accesoDatosRespuesta.cantidadVecesElegidaUnaOpcion(item.encuestaID, item.preguntaID, item.opcion1));
                    listaConteoRespuestasPorOpcion.Add(accesoDatosRespuesta.cantidadVecesElegidaUnaOpcion(item.encuestaID, item.preguntaID, item.opcion2));
                    listaConteoRespuestasPorOpcion.Add(accesoDatosRespuesta.cantidadVecesElegidaUnaOpcion(item.encuestaID, item.preguntaID, item.opcion3));
                    listaConteoRespuestasPorOpcion.Add(accesoDatosRespuesta.cantidadVecesElegidaUnaOpcion(item.encuestaID, item.preguntaID, item.opcion4));
                }
                ViewData["listaConteoRespuestasPorOpcion"] = listaConteoRespuestasPorOpcion;
            }
       
            catch {
                ViewData["Mensaje"] = "Aun no hay respuestas";
            }
        }



       


    }
}