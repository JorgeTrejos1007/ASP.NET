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
using PIBasesISGrupo1.Filters;

namespace PIBasesISGrupo1.Pages.Encuestas
{
    [PermisosDeVista("Miembro de Nucleo", "Coordinador")]
    public class MostrarRespuestasModel : PageModel
    {

        [BindProperty]
        public List<PreguntaModel> listaPreguntas { get; set; }

        public void OnGet(int id)
        {

            
            ViewData["idEncuesta"] = id;
            try
            {
                var listaConteoRespuestasPorPregunta = new List<int>();
                var listaConteoRespuestasPorOpcion = new List<int>();
                var sumaRespuestasPorOpcion = 0;
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
                /*forea(listaConteoRespuestasPorOpcion.Count / 4); i++)
                {
                }*/

                for (int i = 0; listaConteoRespuestasPorOpcion.Count > i; i=i+4)
                {
                    sumaRespuestasPorOpcion = listaConteoRespuestasPorOpcion[i];
                    sumaRespuestasPorOpcion += listaConteoRespuestasPorOpcion[i+1];
                    if (listaConteoRespuestasPorOpcion[i + 2] >=0 ) {
                        sumaRespuestasPorOpcion += listaConteoRespuestasPorOpcion[i + 2];
                    }
                    if(listaConteoRespuestasPorOpcion[i+3] >= 0)
                    {
                        sumaRespuestasPorOpcion += listaConteoRespuestasPorOpcion[i + 3];
                    }
                    listaConteoRespuestasPorPregunta.Add(sumaRespuestasPorOpcion);
                }
                ViewData["listaConteoRespuestasPorPregunta"] = listaConteoRespuestasPorPregunta;
                ViewData["listaConteoRespuestasPorOpcion"] = listaConteoRespuestasPorOpcion;
            }
       
            catch {
                ViewData["Mensaje"] = "Aun no hay respuestas";
            }
        }
    }
}