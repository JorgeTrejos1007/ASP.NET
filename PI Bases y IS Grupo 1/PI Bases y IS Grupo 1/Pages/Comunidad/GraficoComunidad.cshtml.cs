using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PIBasesISGrupo1.Models;
using PIBasesISGrupo1.Handler;
using PIBasesISGrupo1.Filters;

namespace PIBasesISGrupo1.Pages.Comunidad
{

    [PermisosDeVista("Coordinador")]
    public class GraficoComunidadModel : PageModel
    {
        private GraficoHandler grafico = new GraficoHandler();
        public void OnGet()
        {
            obtenerCantidadDeIdiomas();
            ObtenerTopCursos();
            ObtenerTiposUsuario();
            obtenerCantidadDeMiembrosPorPais();
            obtenerHabilidadesFrencuentes();
            obtenerIdiomasFrencuentes();
            obtenerEducadoresConMasCursosCreados();
            obtenerEstudiantesCertificadosYnoCertificados();
            obtenerCantidadDeEventos();
            obtenerCantidadCursosCreados();
            obtenerPerfilesConMasLikes();
            obtenerEducadoresYEstudiantes();

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
            double cantidadArestar = 0;
            double miembros = 0;
            List<Tuple<string, int>> tiposUsuario = grafico.obtenerTiposDeUsuarios();
            
            double total = 0;
            foreach (var tipoUsuario in tiposUsuario)
            {
                if (tipoUsuario.Item1 != "Miembro")
                {
                    cantidadArestar = cantidadArestar+ tipoUsuario.Item2;
                }
                else {
                    miembros = tipoUsuario.Item2;
                }
                 
            }
            miembros = miembros - cantidadArestar;

            total = miembros + cantidadArestar;

            foreach (var tipoUsuario in tiposUsuario)
            {
                if (tipoUsuario.Item1!= "Miembro") {
                    dataPoints.Add(new DataPoint(tipoUsuario.Item1, Math.Round((tipoUsuario.Item2 / total) * 100)));
                }
                
            }


            dataPoints.Add(new DataPoint("Miembros", Math.Round((miembros / total)*100)));


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

        public void obtenerHabilidadesFrencuentes()
        {
            List<DataPoint> dataPoints = new List<DataPoint>();

            List<Tuple<string, int>> CantidadDeHabilidadesFrencuentes = grafico.obtenerHabilidadesFrecuentesDeLaComunidad();

            foreach (var habilidad in CantidadDeHabilidadesFrencuentes)
            {
                dataPoints.Add(new DataPoint(habilidad.Item1, habilidad.Item2));
            }

            TempData["GraficoHabilidadesFrencuentes"] = JsonConvert.SerializeObject(dataPoints);

        }

        public void obtenerIdiomasFrencuentes()
        {
            List<DataPoint> dataPoints = new List<DataPoint>();

            List<Tuple<string, int>> CantidadIdiomasFrencuentess = grafico.obtenerIdiomasFrecuentesDeLaComunidad();

            foreach (var idioma in CantidadIdiomasFrencuentess)
            {
                dataPoints.Add(new DataPoint(idioma.Item1, idioma.Item2));
            }

            TempData["GraficoIdiomasFrencuentes"] = JsonConvert.SerializeObject(dataPoints);

        }

        public void obtenerEducadoresConMasCursosCreados()
        {
            List<DataPoint> dataPoints = new List<DataPoint>();

            List<Tuple<string, int>> cantidadEducadoresConMasCursosCreados = grafico.obtenerEducadoresConMasCursosCreados();

            foreach (var educador in cantidadEducadoresConMasCursosCreados)
            {
                dataPoints.Add(new DataPoint(educador.Item1, educador.Item2));
            }

            TempData["GraficoEducadoresConMasCursosCreados"] = JsonConvert.SerializeObject(dataPoints);

        }

        public void obtenerEstudiantesCertificadosYnoCertificados()
        {
            List<DataPoint> dataPoints = new List<DataPoint>();
            double cantidadDeEstudiantesCertificados = grafico.obtenerTotalDeEstudiantesCertificados();
            double cantidadDeEstudiantesNoCertificados = grafico.obtenerTotalDeEstudiantesNoCertificados();
            double total = cantidadDeEstudiantesCertificados + cantidadDeEstudiantesNoCertificados;

            dataPoints.Add(new DataPoint("Certificados", Math.Round((cantidadDeEstudiantesCertificados/total)*100)));
            dataPoints.Add(new DataPoint("No certificados", Math.Round((cantidadDeEstudiantesNoCertificados/total)*100)));

            TempData["GraficoEstudiantesCertificadosYnoCertificados"] = JsonConvert.SerializeObject(dataPoints);

        }

        public IActionResult OnPostObtenerHabilidaesYtiposDeUsuario(string pais)
        {
            //topHablidadesPorPais(pais);
            List<DataPoint> habilidades = new List<DataPoint>();
            List<DataPoint> tiposDeUsuarios = new List<DataPoint>();
            List<Tuple<string, int>> topHablidadesPorPais = grafico.obtenerLasHabilidadesMasFrecuentesPorPais(pais);
            List<Tuple<string, int>> tiposDeUsuarioPorPais = grafico.obtenerTipoDeUsuarioPorPais(pais);

            foreach (var habilidad in topHablidadesPorPais)
            {
                habilidades.Add(new DataPoint(habilidad.Item1, habilidad.Item2));
            }

            foreach (var tiposDeUsuario in tiposDeUsuarioPorPais)
            {
                tiposDeUsuarios.Add(new DataPoint(tiposDeUsuario.Item1, tiposDeUsuario.Item2));
            }


            Tuple<List<DataPoint>, List<DataPoint>> datos = new Tuple<List<DataPoint>,List<DataPoint>>(habilidades, tiposDeUsuarios);
            return new JsonResult(datos);

        }

        public void obtenerCantidadDeEventos()
        {
            List<DataPoint> dataPoints = new List<DataPoint>();
            double cantidadEventosPresenciales = grafico.obtenerCantidadEventosPresenciales();
            double cantidadEventosVirtuales = grafico.obtenerCantidadEventosVirtuales();
            double total = cantidadEventosPresenciales + cantidadEventosVirtuales;

            dataPoints.Add(new DataPoint("Presenciales", Math.Round((cantidadEventosPresenciales / total) * 100)));
            dataPoints.Add(new DataPoint("Virtuales", Math.Round((cantidadEventosVirtuales / total) * 100)));

            TempData["GraficoCantidadEventos"] = JsonConvert.SerializeObject(dataPoints);

        }

        public void obtenerCantidadCursosCreados()
        {

            TempData["CantidadCursosCreados"] = grafico.obtenerCantidadCursosCreados();

        }

        public void obtenerCantidadDeIdiomas()
        {

            TempData["CantidadIdiomas"] = grafico.obtenerCantidadDeIdiomas();

        }
        public void obtenerPerfilesConMasLikes() {
            List<DataPoint> dataPoints = new List<DataPoint>();
            List<Tuple<string, int>> perfilesConMasLikes = grafico.obtenerPerfilesConMasLikes();
            foreach (var perfil in perfilesConMasLikes)
            {
                dataPoints.Add(new DataPoint(perfil.Item1, perfil.Item2));
            }

            TempData["GraficoTopLikes"] = JsonConvert.SerializeObject(dataPoints);
        }
        public void obtenerEducadoresYEstudiantes()
        {
            List<DataPoint> dataPoints = new List<DataPoint>();
            int cantidadEstudiantes= grafico.obtenerCantidadEstudiantes();
            int cantidadEducadores = grafico.obtenerCantidadEducadores();
           
           
            dataPoints.Add(new DataPoint("Estudiantes", cantidadEstudiantes));
            dataPoints.Add(new DataPoint("Educador", cantidadEducadores));


            TempData["GraficoEstudientesEducadores"] = JsonConvert.SerializeObject(dataPoints);
        }



    }
}