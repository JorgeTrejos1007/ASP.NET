using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PIBasesISGrupo1.Models;
using PIBasesISGrupo1.Handler;

namespace PIBasesISGrupo1.Pages.Comunidad
{
    public class GraficoComunidadModel : PageModel
    {
        private GraficoHandler grafico = new GraficoHandler();
        public void OnGet()
        {
            TempData["GraficotopHablidadesPorPais"] = null;
            ObtenerTopCursos();
            ObtenerTiposUsuario();
            obtenerCantidadDeMiembrosPorPais();
        }
        public void ObtenerTopCursos()
        {
            List<DataPoint> dataPoints = new List<DataPoint>();
           
            List<Tuple<string, int>> topCursos = grafico.obtenerTopCursos();
            foreach (var curso in topCursos)
            {
                dataPoints.Add(new DataPoint(curso.Item1, curso.Item2));
            }


            TempData["GraficoTopCursos"] = JsonConvert.SerializeObject(dataPoints);
            ViewData["TopTopicos"] = grafico.obtenerTopicosDeTopCursos();
        }
        public void ObtenerTiposUsuario()
        {
            List<DataPoint> dataPoints = new List<DataPoint>();
            
            List<Tuple<string, int>> tiposUsuario = grafico.obtenerTiposDeUsuarios();
            int cantidadEstudiantes = grafico.obtenerCantidadEstudiantes();
            foreach (var tipoUsuario in tiposUsuario)
            {
                dataPoints.Add(new DataPoint(tipoUsuario.Item1, tipoUsuario.Item2));
            }
            dataPoints.Add(new DataPoint("Estudiates", cantidadEstudiantes));


            TempData["GraficoTiposUsuario"] = JsonConvert.SerializeObject(dataPoints);
            
        }
        public void obtenerCantidadDeMiembrosPorPais()
        {
            List<DataPoint> dataPoints = new List<DataPoint>();

            List<Tuple<string, int>> CantidadDeMiembrosPorPais = grafico.obtenerCantidadDeMiembrosPorPais();
            
            foreach (var miembro in CantidadDeMiembrosPorPais)
            {
                dataPoints.Add(new DataPoint(miembro.Item1, miembro.Item2));
            }
 
            TempData["GraficoCantidadDeMiembrosPorPais"] = JsonConvert.SerializeObject(dataPoints);

        }
        

        public IActionResult OnPostObtenerHabilidaesYtiposDeUsuario(string pais)
        {
            //topHablidadesPorPais(pais);
            List<DataPoint> dataPoints = new List<DataPoint>();

            List<Tuple<string, int>> topHablidadesPorPais = grafico.obtenerLasHabilidadesMasFrecuentesPorPais(pais);

            foreach (var habilidad in topHablidadesPorPais)
            {
                dataPoints.Add(new DataPoint(habilidad.Item1, habilidad.Item2));
            }
            Tuple<List<DataPoint>, List<DataPoint>> datos = new Tuple<List<DataPoint>,List<DataPoint>>(dataPoints, dataPoints);
            return new JsonResult(datos);

        }



    }
}