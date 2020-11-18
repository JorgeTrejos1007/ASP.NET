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
            List<DataPoint> dataPoints = new List<DataPoint>();
            grafico = new GraficoHandler();
            List<Tuple<string, int>> topCursos = grafico.obtenerTopCursos();
            foreach (var curso in topCursos)
            {
                dataPoints.Add(new DataPoint(curso.Item1, curso.Item2));
            }


            TempData["GraficoTopCursos"] = JsonConvert.SerializeObject(dataPoints);
           ViewData["TopTopicos"]= grafico.obtenerTopicosDeTopCursos();
        }
    }
}