﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PIBasesISGrupo1.Models;
using PIBasesISGrupo1.Handler;
namespace PIBasesISGrupo1.Pages.Curso
{
    public class GraficoModel : PageModel
    {
        // GET: Home
        private CursoHandler accesoACursos;
        private GraficoHandler grafico=new GraficoHandler();
        [BindProperty]        public string[] cursosAFiltrar { get; set; }
        public string[] habilidades = new string[17]
               {
                    "Empatia","Saber escuchar","Liderazgo", "Flexibilidad","Optimismo", "Confianza","Honestidad",
                    "Paciencia","Comunicacion","Convencimiento","Esfuerzo", "Esfuerzo","Dedicacion","Comprension",
                    "Comprension", "Asertividad", "Credibilidad"

               };

        public IActionResult OnGet()
        {
            accesoACursos = new CursoHandler();
            ViewData["cursos"] = accesoACursos.obtenerNombresDeCursos();
            return Page();

        }
        public IActionResult OnPost() {
            
            List<DataPoint> dataPoints = new List<DataPoint>();
            List<string> estudiantesConCertificado = grafico.obtenerEstudiantesCertificadosDeUnCurso(cursosAFiltrar[0]);
            List<string> estudiantesSinCertificado = grafico.obtenerEstudiantesQueEstanCursandoUnCurso(cursosAFiltrar[0]);
            double total = estudiantesConCertificado.Count + estudiantesSinCertificado.Count;
            double totalConCertificado = estudiantesConCertificado.Count;
            double totalSinCertificado = estudiantesSinCertificado.Count;

            if (totalConCertificado > 0)
            {


                double porcentajeConCertificado = ((totalConCertificado / total) * 100);
                dataPoints.Add(new DataPoint("Certificados", Math.Round(porcentajeConCertificado)));

            }
            else
            {
                dataPoints.Add(new DataPoint("Certificados", 0));

            }

            if (totalSinCertificado > 0)
            {

                double porcentajeSinCertificado = ((totalSinCertificado / total) * 100);

                dataPoints.Add(new DataPoint("Cursando", Math.Round(porcentajeSinCertificado)));

            }
            else
            {
                dataPoints.Add(new DataPoint("Cursando", 0));

            }

            TempData["Grafico"] = JsonConvert.SerializeObject(dataPoints);
            obtenerDatosDeMaterialesVistosDeUnCurso();
            obtenerDatosDeLasHabilidades(cursosAFiltrar);
            obtenerDatosDeLasPaises(cursosAFiltrar);
            obtenerDatosDeLasHabilidadesFrecuentesDeEstdiantesCertificados(cursosAFiltrar);
            return RedirectToPage("Grafico");
            
        }
        private void obtenerDatosDeMaterialesVistosDeUnCurso(){
            accesoACursos = new CursoHandler();
            double cantidadDeMaterialesVistosPorEstudiantes = 0.0;
            double cantidadDeMateriales = 0.0;
            double porcentajeDeMaterialesVistos = 0.0;
            List<string> estudiantes = new List<string>();
            foreach (var curso in cursosAFiltrar) {
                cantidadDeMateriales += accesoACursos.obtenerCantidadMaterialPorCurso(curso);
                estudiantes = accesoACursos.obtenerCorreorsDeEstudiantesMatriculadosEnUnCurso(curso);
                foreach (var estudiante in estudiantes) {
                    cantidadDeMaterialesVistosPorEstudiantes += accesoACursos.obtenerCantidadMaterialVistoPorEstudiante(curso, estudiante);
                      

                }
                 
            }
            porcentajeDeMaterialesVistos  = cantidadDeMaterialesVistosPorEstudiantes / (estudiantes.Count*cantidadDeMateriales);
            List<DataPoint> dataPoints = new List<DataPoint>();
            if (cantidadDeMaterialesVistosPorEstudiantes > 0)
            {
                        dataPoints.Add(new DataPoint("Materiales Vistos", Math.Round(porcentajeDeMaterialesVistos*100)));

            }
            else
            {
                dataPoints.Add(new DataPoint("Materiales Vistos", 0));

            }
            double porcentajeDeMaterialesNoVistos = 1.0 - porcentajeDeMaterialesVistos;
            if (porcentajeDeMaterialesNoVistos > 0)
            {
                dataPoints.Add(new DataPoint("Materiales No Vistos", Math.Round(porcentajeDeMaterialesNoVistos*100)));

            }
            else
            {
                dataPoints.Add(new DataPoint("Materiales No Vistos", 0));

            }
            TempData["GraficoMateriales"] = JsonConvert.SerializeObject(dataPoints);

        }
        private void obtenerDatosDeLasHabilidades(string[] cursos) {
            List<DataPoint> dataPoints = new List<DataPoint>();
            grafico = new GraficoHandler();
            List<Tuple<string,int>> habilidadesMasFrecuentes=grafico.obtenerLasHabilidadesMasFrecuentes(cursos);
            foreach (var habilidad in habilidadesMasFrecuentes)
            {
                dataPoints.Add(new DataPoint(habilidad.Item1,habilidad.Item2));
            }


            TempData["GraficoHabilidades"] = JsonConvert.SerializeObject(dataPoints);

        }

        private void obtenerDatosDeLasPaises(string[] cursos)
        {
            List<DataPoint> dataPoints = new List<DataPoint>();
            grafico = new GraficoHandler();
            List<Tuple<string, int>> pasiesMasFrecuentes = grafico.obtenerLosPaisesMasFrecuentes(cursos);

            foreach (var pais in pasiesMasFrecuentes)
            {
                dataPoints.Add(new DataPoint(pais.Item1, pais.Item2));
            }


            TempData["GraficoPaises"] = JsonConvert.SerializeObject(dataPoints);

        }
        private void obtenerDatosDeLasHabilidadesFrecuentesDeEstdiantesCertificados(string[] cursos)
        {
            List<DataPoint> dataPoints = new List<DataPoint>();
            grafico = new GraficoHandler();
            List<Tuple<string, int>> habilidadesMasFrecuentes = grafico.obtenerLasHabilidadesMasFrecuentesDeEstudiantesCertificados(cursos);
            foreach (var habilidad in habilidadesMasFrecuentes)
            {
                dataPoints.Add(new DataPoint(habilidad.Item1, habilidad.Item2));
            }


            TempData["HabilidadesEstudiantesCertificados"] = JsonConvert.SerializeObject(dataPoints);

        }
        



    }
}