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



        public IActionResult OnPostExportarRespuestas(int id)
        {
            var registroDeRespuestas = new XLWorkbook();
            var hojaDeRespuestas = registroDeRespuestas.Worksheets.Add("Respuestas");
            //fila//columna
            int fila = 1;
            hojaDeRespuestas.Cell(fila, 1).Value = "ID";
            hojaDeRespuestas.Cell(fila, 2).Value = "Nombre";

            fila = fila+1;
            hojaDeRespuestas.Cell(fila, 1).Value = "0";
            hojaDeRespuestas.Cell(fila, 2).Value = "Ronny";

            var stream = new MemoryStream();
            registroDeRespuestas.SaveAs(stream);
            var content = stream.ToArray();
            string excelContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            return File(content, excelContentType,"Respuestas.xlsx");
        }


    }
}