﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using PIBasesISGrupo1.Models;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Web;
using Microsoft.AspNetCore.Http;
using PIBasesISGrupo1.Handler;

namespace PIBasesISGrupo1.Handler
{

    public class GraficoHandler
    {
        private BaseDeDatosHandler baseDeDatos;
        int totalEstudiantes;
        int totalDeEstudiantesConCiertaHabilidad;
        int totalDeEstudiantesConCiertoIdioma;
        int totalEstudiantesPorPaisEnCursos= 0;
        public GraficoHandler()
        {

            baseDeDatos = new BaseDeDatosHandler();
            totalEstudiantes = 0;
            totalDeEstudiantesConCiertaHabilidad = 0;
            totalDeEstudiantesConCiertoIdioma = 0;
            totalEstudiantesPorPaisEnCursos = 0;

        }


        public int obtenerEstudiantesCertificadosDeUnCurso(string  []cursos) {
            string consulta = "SELECT DISTINCT COUNT( emailEstudianteFK) AS Cantidad FROM Certificado " 
            +" WHERE(";
            for (int curso = 0; curso < cursos.Length; ++curso)
            {
                consulta += " nombreCursoFK = '" + cursos[curso] + "'";
                if (curso + 1 < cursos.Length)
                {
                    consulta += " OR ";
                }

            }
            consulta += ") AND estado='Aprobado' ";
            int totalCertificados = 0;
            SqlCommand comandoParaConsulta = baseDeDatos.crearComandoParaConsulta(consulta);
            DataTable estudiantesConCertificado = baseDeDatos.crearTablaConsulta(comandoParaConsulta);
            foreach (DataRow columnaCursosAprobados in estudiantesConCertificado.Rows)
            {
                totalCertificados= Convert.ToInt32(columnaCursosAprobados["Cantidad"]);

            }
               
            return totalCertificados;
        }

        public int obtenerEstudiantesQueEstanCursandoUnCurso(string []cursos)
        {
            string consulta = "SELECT DISTINCT COUNT( emailEstudianteFK) AS Cantidad FROM Certificado "
            + " WHERE(";
            for (int curso = 0; curso < cursos.Length; ++curso)
            {
                consulta += " nombreCursoFK = '" + cursos[curso] + "'";
                if (curso + 1 < cursos.Length)
                {
                    consulta += " OR ";
                }

            }
            consulta += ") AND estado !='Aprobado' ";
            int totalCertificados = 0;
            SqlCommand comandoParaConsulta = baseDeDatos.crearComandoParaConsulta(consulta);
            DataTable estudiantesConCertificado = baseDeDatos.crearTablaConsulta(comandoParaConsulta);
            foreach (DataRow columnaCursosAprobados in estudiantesConCertificado.Rows)
            {
                totalCertificados = Convert.ToInt32(columnaCursosAprobados["Cantidad"]);

            }

            return totalCertificados;
        }
        public List<Tuple<string, int>> obtenerLasHabilidadesMasFrecuentes(string [] cursos){

            List<Tuple<string, int>> habilidades = new List<Tuple<string, int>>();  
            string consulta = " SELECT TOP 5 COUNT(habilidadPK) AS Cantidad,habilidadPK" +
            " FROM Habilidades WHERE emailFK IN (SELECT emailEstudianteFK FROM Certificado " +
            " WHERE";
            for (int curso = 0; curso < cursos.Length; ++curso) {
               consulta += " nombreCursoFK = '"+cursos[curso]+"'";
                if (curso + 1 < cursos.Length) {
                    consulta += " OR ";
                }

            }
            consulta += ") GROUP BY habilidadPK  ORDER BY COUNT(habilidadPK)DESC";
            SqlCommand comando = baseDeDatos.crearComandoParaConsulta(consulta);
            DataTable topHabilidades = baseDeDatos.crearTablaConsulta(comando);
            foreach (DataRow columnaCursosAprobados in topHabilidades.Rows)
            {
                habilidades.Add(new Tuple<string, int>(Convert.ToString(columnaCursosAprobados["habilidadPK"]), Convert.ToInt32(columnaCursosAprobados["Cantidad"])));

            }
            return habilidades;

        }

        public List<Tuple<string, int>> obtenerLosPaisesMasFrecuentes(string[] cursos)
        {

            List<Tuple<string, int>> paises = new List<Tuple<string, int>>();
            string consulta = " SELECT TOP 5 COUNT(pais) AS Cantidad,pais" +
            " FROM Usuario  WHERE emailPK IN (SELECT emailEstudianteFK FROM Certificado " +
            " WHERE";
            for (int curso = 0; curso < cursos.Length; ++curso)
            {
                consulta += " nombreCursoFK = '" + cursos[curso] + "'";
                if (curso + 1 < cursos.Length)
                {
                    consulta += " OR ";
                }

            }
            consulta += ") GROUP BY pais  ORDER BY COUNT(pais)DESC";
            SqlCommand comando = baseDeDatos.crearComandoParaConsulta(consulta);
            DataTable topHabilidades = baseDeDatos.crearTablaConsulta(comando);
            foreach (DataRow columnaCursosAprobados in topHabilidades.Rows)
            {
                paises.Add(new Tuple<string, int>(Convert.ToString(columnaCursosAprobados["pais"]), Convert.ToInt32(columnaCursosAprobados["Cantidad"])));

            }
            return paises;

        }
        public List<Tuple<string, int>> obtenerLasHabilidadesMasFrecuentesDeEstudiantesCertificados(string[] cursos)
        {
           List<Tuple<string, int>> habilidades = new List<Tuple<string, int>>();
            string consulta = "  select COUNT(Distinct h.emailFK) AS Cantidad,h.habilidadPK  AS habilidadPK FROM Habilidades H " +
           "join Certificado C ON C.emailEstudianteFK = H.emailFK " +
            "   WHERE C.estado = 'Aprobado' AND (";
            for (int curso = 0; curso < cursos.Length; ++curso)
            {
                consulta += " C.nombreCursoFK = '" + cursos[curso] + "'";
                if (curso + 1 < cursos.Length)
                {
                    consulta += " OR ";
                }

            }
            consulta += ") GROUP BY H.habilidadPK ";
            SqlCommand comando = baseDeDatos.crearComandoParaConsulta(consulta);
            DataTable topHabilidades = baseDeDatos.crearTablaConsulta(comando);
            foreach (DataRow columnaCursosAprobados in topHabilidades.Rows)
            {
                habilidades.Add(new Tuple<string, int>(Convert.ToString(columnaCursosAprobados["habilidadPK"]), Convert.ToInt32(columnaCursosAprobados["Cantidad"])));

            }
            return habilidades;

        }
        public List<Tuple<string, int>> obtenerIdiomasMasFrecuentesDeEstudiantes(string[] cursos)
        {

            List<Tuple<string, int>> idiomas = new List<Tuple<string, int>>();
            string consulta = " SELECT TOP 15 COUNT(idiomaPK) AS Cantidad,idiomaPK" +
            " FROM Idiomas WHERE emailFK IN (SELECT emailEstudianteFK FROM Certificado " +
            " WHERE (";
            for (int curso = 0; curso < cursos.Length; ++curso)
            {
                consulta += " nombreCursoFK = '" + cursos[curso] + "'";
                if (curso + 1 < cursos.Length)
                {
                    consulta += " OR ";
                }

            }
            consulta += ")) GROUP BY idiomaPK ORDER BY COUNT(idiomaPK)DESC";
            SqlCommand comando = baseDeDatos.crearComandoParaConsulta(consulta);
            DataTable topHabilidades = baseDeDatos.crearTablaConsulta(comando);
            foreach (DataRow columnaCursosAprobados in topHabilidades.Rows)
            {
                idiomas.Add(new Tuple<string, int>(Convert.ToString(columnaCursosAprobados["idiomaPK"]), Convert.ToInt32(columnaCursosAprobados["Cantidad"])));

            }
            return idiomas;

        }
        public List<Tuple<string, int>> obtenerEstudiantesPorCurso(string[] cursos)
        {

            List<Tuple<string, int>> estudiantes = new List<Tuple<string, int>>();
            string consulta = "SELECT     COUNT(  DISTINCT emailEstudianteFK) as Cantidad,nombreCursoFK  FROM Certificado     " +
            " WHERE ";
            for (int curso = 0; curso < cursos.Length; ++curso)
            {
                consulta += " nombreCursoFK = '" + cursos[curso] + "'";
                if (curso + 1 < cursos.Length)
                {
                    consulta += " OR ";
                }

            }
            consulta += " group by nombreCursoFK";
            SqlCommand comando = baseDeDatos.crearComandoParaConsulta(consulta);
            DataTable estudiantesPorCurso= baseDeDatos.crearTablaConsulta(comando);
            foreach (DataRow columnaCursosAprobados in estudiantesPorCurso.Rows)
            {
                totalEstudiantes += Convert.ToInt32(columnaCursosAprobados["Cantidad"]);
                estudiantes.Add(new Tuple<string, int>(Convert.ToString(columnaCursosAprobados["nombreCursoFK"]), Convert.ToInt32(columnaCursosAprobados["Cantidad"])));

            }
            return estudiantes;

        }

        public List<Tuple<string, int>> obtenerTopCursos()
        {

            List<Tuple<string, int>> topCursos = new List<Tuple<string, int>>();
            string consulta = "SELECT TOP 5 COUNT(nombreCursoFK) AS cantidad, nombreCursoFK" +            " FROM Certificado GROUP BY nombreCursoFK ORDER BY COUNT(nombreCursoFK) DESC";
            
            SqlCommand comando = baseDeDatos.crearComandoParaConsulta(consulta);
            DataTable tablaTopCursos = baseDeDatos.crearTablaConsulta(comando);
            foreach (DataRow columnaTopCursos in tablaTopCursos.Rows)
            {
                topCursos.Add(new Tuple<string, int>(Convert.ToString(columnaTopCursos["nombreCursoFK"]), Convert.ToInt32(columnaTopCursos["Cantidad"])));

            }
            return topCursos;

        }
        public List<string> obtenerTopicosDeTopCursos()
        {

            List<string> topTopicos = new List<string>();
            string consulta = "SELECT nombreTopicoFK FROM Contiene WHERE nombreCursoFK IN("+
                               " SELECT TOP 5 nombreCursoFK FROM Certificado GROUP BY nombreCursoFK ORDER BY COUNT(nombreCursoFK) DESC)";

            SqlCommand comando = baseDeDatos.crearComandoParaConsulta(consulta);
            DataTable tablaTopicosTopCursos = baseDeDatos.crearTablaConsulta(comando);
            foreach (DataRow columnaTopicos in tablaTopicosTopCursos.Rows)
            {
                topTopicos.Add(Convert.ToString(columnaTopicos["nombreTopicoFK"]));

            }
            return topTopicos;

        }
        public List<Tuple<string, int>> obtenerTiposDeUsuarios()
        {

            List<Tuple<string, int>> tiposUsuario = new List<Tuple<string, int>>();
            string consulta = "SELECT COUNT(rolUsuarioPK) AS cantidad, rolUsuarioPK FROM Rol GROUP BY rolUsuarioPK ";

            SqlCommand comando = baseDeDatos.crearComandoParaConsulta(consulta);
            DataTable tablaTiposDeUsuarios = baseDeDatos.crearTablaConsulta(comando);
            foreach (DataRow tipoUsuario in tablaTiposDeUsuarios.Rows)
            {
                tiposUsuario.Add(new Tuple<string, int>(Convert.ToString(tipoUsuario["rolUsuarioPK"]), Convert.ToInt32(tipoUsuario["cantidad"])));

            }
            return tiposUsuario;

        }
        public int obtenerCantidadEstudiantes()
        {

            int cantidadEstudiantes = 0;
            string consulta = "SELECT COUNT(*) as CantidadDeEstudiantes  FROM Estudiante";

            SqlCommand comando = baseDeDatos.crearComandoParaConsulta(consulta);
            DataTable tablaCantidadEstudiantes = baseDeDatos.crearTablaConsulta(comando);
            foreach (DataRow columnaCantidad in tablaCantidadEstudiantes.Rows)
            {
                cantidadEstudiantes=Convert.ToInt32(columnaCantidad["CantidadDeEstudiantes"]);

            }
            return cantidadEstudiantes;

        }

        public List<Tuple<string, int>> obtenerCantidadDeMiembrosPorPais()
        {

            List<Tuple<string, int>> CantidadDeMiembrosPorPais = new List<Tuple<string, int>>();
            string consulta = " SELECT TOP 5 COUNT(pais) AS Cantidad, pais FROM Usuario WHERE emailPK IN(SELECT emailMiembroFK FROM Miembro)"+
                                " GROUP BY pais ORDER BY COUNT(pais)DESC";

            SqlCommand comando = baseDeDatos.crearComandoParaConsulta(consulta);
            DataTable tablaCantidadDeMiembrosPorPais = baseDeDatos.crearTablaConsulta(comando);
            foreach (DataRow miembrosPorPais in tablaCantidadDeMiembrosPorPais.Rows)
            {
                CantidadDeMiembrosPorPais.Add(new Tuple<string, int>(Convert.ToString(miembrosPorPais["pais"]), Convert.ToInt32(miembrosPorPais["Cantidad"])));

            }
            return CantidadDeMiembrosPorPais;

        }
        public List<Tuple<string, int>> obtenerLasHabilidadesMasFrecuentesPorPais(string pais)
        {

            List<Tuple<string, int>> habilidades = new List<Tuple<string, int>>();
            string consulta = " SELECT TOP 5 COUNT(habilidadPK) AS Cantidad,habilidadPK FROM Habilidades WHERE emailFK IN( " +                             " SELECT emailPK FROM Usuario WHERE pais = @pais) " +                             " GROUP BY habilidadPK ORDER BY COUNT(habilidadPK)DESC";
            SqlCommand comando = baseDeDatos.crearComandoParaConsulta(consulta);
            comando.Parameters.AddWithValue("@pais", pais);
            DataTable topHabilidades = baseDeDatos.crearTablaConsulta(comando);
            foreach (DataRow columnaTopHabilidades in topHabilidades.Rows)
            {
                habilidades.Add(new Tuple<string, int>(Convert.ToString(columnaTopHabilidades["habilidadPK"]), Convert.ToInt32(columnaTopHabilidades["Cantidad"])));

            }
            return habilidades;

        }

        public List<Tuple<string, int>> obtenerTipoDeUsuarioPorPais(string pais)
        {

            List<Tuple<string, int>> tipoUsuario = new List<Tuple<string, int>>();
            string consulta = " SELECT COUNT(rolUsuarioPK) AS cantidad, rolUsuarioPK" +                             " FROM Rol WHERE emailUsuarioFK IN(SELECT emailPK FROM Usuario WHERE pais=@pais)" +
                             " GROUP BY rolUsuarioPK";
            SqlCommand comando = baseDeDatos.crearComandoParaConsulta(consulta);
            comando.Parameters.AddWithValue("@pais", pais);
            DataTable TipoDeUsuarioPorPais = baseDeDatos.crearTablaConsulta(comando);
            foreach (DataRow columnaTipoDeUsuario in TipoDeUsuarioPorPais.Rows)
            {
                tipoUsuario.Add(new Tuple<string, int>(Convert.ToString(columnaTipoDeUsuario["rolUsuarioPK"]), Convert.ToInt32(columnaTipoDeUsuario["Cantidad"])));

            }
            return tipoUsuario;

        }

        public List<Tuple<string, int>> obtenerHabilidadesFrecuentesDeLaComunidad()
        {

            List<Tuple<string, int>> habilidades = new List<Tuple<string, int>>();
            string consulta = " SELECT TOP 5 COUNT(habilidadPK) as cantidad, habilidadPK" +                             " FROM Habilidades" +
                             " GROUP BY habilidadPK ORDER BY COUNT(habilidadPK)DESC";
            SqlCommand comando = baseDeDatos.crearComandoParaConsulta(consulta);
            DataTable HabilidadesFrecuentes = baseDeDatos.crearTablaConsulta(comando);
            foreach (DataRow columnaHabilidadesFrecuentes in HabilidadesFrecuentes.Rows)
            {
                habilidades.Add(new Tuple<string, int>(Convert.ToString(columnaHabilidadesFrecuentes["habilidadPK"]), Convert.ToInt32(columnaHabilidadesFrecuentes["cantidad"])));

            }
            return habilidades;

        }

        public List<Tuple<string, int>> obtenerIdiomasFrecuentesDeLaComunidad()
        {

            List<Tuple<string, int>> idiomas = new List<Tuple<string, int>>();
            string consulta = " SELECT TOP 5 COUNT(idiomaPK) as cantidad, idiomaPK" +                             "  FROM Idiomas" +
                             "  GROUP BY idiomaPK ORDER BY COUNT(idiomaPK)DESC";
            SqlCommand comando = baseDeDatos.crearComandoParaConsulta(consulta);
            DataTable idiomasFrecuentes = baseDeDatos.crearTablaConsulta(comando);
            foreach (DataRow columnaIdiomasFrecuentes in idiomasFrecuentes.Rows)
            {
                idiomas.Add(new Tuple<string, int>(Convert.ToString(columnaIdiomasFrecuentes["idiomaPK"]), Convert.ToInt32(columnaIdiomasFrecuentes["cantidad"])));

            }
            return idiomas;

        }

        public List<Tuple<string, int>> obtenerEducadoresConMasCursosCreados()
        {

            List<Tuple<string, int>> educadores = new List<Tuple<string, int>>();
            string consulta = " SELECT TOP 5 COUNT(C.emailEducadorFK) as cantidad, U.nombre +' '+ U.primerApellido as nombreCompleto" +                             "  FROM Curso C JOIN Usuario U ON C.emailEducadorFK=U.emailPK WHERE C.estado='Creado'" +
                             "  GROUP BY U.nombre,U.primerApellido ORDER BY COUNT(C.emailEducadorFK)DESC";
            SqlCommand comando = baseDeDatos.crearComandoParaConsulta(consulta);
            DataTable idiomasFrecuentes = baseDeDatos.crearTablaConsulta(comando);
            foreach (DataRow columnaIdiomasFrecuentes in idiomasFrecuentes.Rows)
            {
                educadores.Add(new Tuple<string, int>(Convert.ToString(columnaIdiomasFrecuentes["nombreCompleto"]), Convert.ToInt32(columnaIdiomasFrecuentes["cantidad"])));

            }
            return educadores;

        }

        public int obtenerTotalDeEstudiantesCertificados()
        {
          
            string consulta = "SELECT COUNT(1) FROM Certificado WHERE estado='Aprobado' ";
            SqlCommand comando = baseDeDatos.crearComandoParaConsulta(consulta);
            int totalDeEstudiantesCertificados = baseDeDatos.obtenerCantidadDeElementos(comando);
            return totalDeEstudiantesCertificados;

        }


        public int obtenerTotalDeEstudiantesNoCertificados()
        {
            string consulta = "SELECT COUNT(1) FROM Certificado WHERE estado!='Aprobado' ";
            SqlCommand comando = baseDeDatos.crearComandoParaConsulta(consulta);
            int totalDeEstudiantesNoCertificados = baseDeDatos.obtenerCantidadDeElementos(comando);

            return totalDeEstudiantesNoCertificados;


        }
        public int obtenerCantidadEventosPresenciales()
        {
            string consulta = " SELECT COUNT(nombreEventoFK) AS cantidad FROM Presencial";
            SqlCommand comando = baseDeDatos.crearComandoParaConsulta(consulta);
            int totalCantidadEventosPresenciales = baseDeDatos.obtenerCantidadDeElementos(comando);

            return totalCantidadEventosPresenciales;

        }
        public int obtenerCantidadEventosVirtuales()
        {
            string consulta = " SELECT COUNT(nombreEventoFK) AS cantidad FROM Virtual";
            SqlCommand comando = baseDeDatos.crearComandoParaConsulta(consulta);
            int totalCantidadEventosVirtuales = baseDeDatos.obtenerCantidadDeElementos(comando);

            return totalCantidadEventosVirtuales;


        }
        public int obtenerCantidadCursosCreados()
        {
            string consulta = " SELECT COUNT(1) AS cantidad FROM Curso";
            SqlCommand comando = baseDeDatos.crearComandoParaConsulta(consulta);
            int totalCursosCreados = baseDeDatos.obtenerCantidadDeElementos(comando);

            return totalCursosCreados;


        }

        public int obtenerCantidadDeIdiomas()
        {
            string consulta = " SELECT COUNT( DISTINCT idiomaPK) FROM Idiomas";
            SqlCommand comando = baseDeDatos.crearComandoParaConsulta(consulta);
            int totalDeIdiomas = baseDeDatos.obtenerCantidadDeElementos(comando);

            return totalDeIdiomas;


        }


        public int retornarTotalDeEstudiantesEnLosCursosFiltrados() {
            return totalEstudiantes;
        }
         
        public List<string> obtenerDatosTopicos() {
            List<string> topicos = new List<string>();
            string consulta = "SELECT DISTINCT nombreTopicoPK FROM Topico;";
            SqlCommand comando = baseDeDatos.crearComandoParaConsulta(consulta);
            DataTable estudiantesPorCurso = baseDeDatos.crearTablaConsulta(comando);
            foreach (DataRow columnaCursosAprobados in estudiantesPorCurso.Rows)
            {
                 topicos.Add(Convert.ToString(columnaCursosAprobados["nombreTopicoPK"]));

            }
            return topicos;

        }
        public List<Tuple<string, List<string>>> obtenerTopicosDeCursos(string [] cursos)
        {
            BaseDeDatosHandler consultasBaseDatos = new BaseDeDatosHandler();
            List<Tuple<string, List<string>>> topicos = new List<Tuple<string, List<string>>>();
            string consulta = "SELECT nombreTopicoFK FROM Contiene WHERE nombreCursoFK = @curso;";
            SqlCommand comando; 
            List<string> topicosPorCurso;
            foreach (var curso in cursos)
            {
                comando= consultasBaseDatos.crearComandoParaConsulta(consulta);
                comando.Parameters.AddWithValue("@curso",curso);
                topicosPorCurso = consultasBaseDatos.obtenerDatosDeColumna(comando, "nombreTopicoFK");
                topicos.Add(new Tuple<string, List<string>>( curso,topicosPorCurso));
            }
            return topicos;

        }
        public List<Tuple<string, int>> obtenerTopicosMasFrecuentesDeCursos(string[] cursos)
        {
            BaseDeDatosHandler consultasBaseDatos = new BaseDeDatosHandler();
            List<Tuple<string, int>> topicos = new List<Tuple<string, int>>();
            string consulta = "SELECT     COUNT( nombreCursoFK) as Cantidad,nombreTopicoFK  FROM Contiene " +
            " WHERE ";
            for (int curso = 0; curso < cursos.Length; ++curso)
            {
                consulta += " nombreCursoFK = '" + cursos[curso] + "'";
                if (curso + 1 < cursos.Length)
                {
                    consulta += " OR ";
                }

            }
            consulta += " group by nombreTopicoFK";
            SqlCommand comando = consultasBaseDatos.crearComandoParaConsulta(consulta);
            DataTable topHabilidades = consultasBaseDatos.crearTablaConsulta(comando);
            foreach (DataRow columnaCursosAprobados in topHabilidades.Rows)
            {
                topicos.Add(new Tuple<string, int>(Convert.ToString(columnaCursosAprobados["nombreTopicoFK"]), Convert.ToInt32(columnaCursosAprobados["Cantidad"])));

            }
            return topicos;

        }
        public List<Tuple<string, int>> obtenerHabilidadesDeEstudiantePorCurso(string[] cursos,string habilidad)
        {

            List<Tuple<string, int>> habilidadPorCurso = new List<Tuple<string, int>>();
            string consulta = " SELECT COUNT( DISTINCT emailEstudianteFK) AS Cantidad,nombreCursoFK" +
            " FROM Certificado WHERE ( ";
            for (int curso = 0; curso < cursos.Length; ++curso)
            {
                consulta += " nombreCursoFK = '" + cursos[curso] + "'";
                if (curso + 1 < cursos.Length)
                {
                    consulta += " OR ";
                }

            }
            consulta += " ) AND emailEstudianteFK IN (SELECT emailFK FROM Habilidades"+
             " WHERE(habilidadPK = @habilidad))   GROUP BY nombreCursoFK ";
            SqlCommand comando = baseDeDatos.crearComandoParaConsulta(consulta);
            comando.Parameters.AddWithValue("@habilidad", habilidad);
            totalDeEstudiantesConCiertaHabilidad = 0;
            DataTable topHabilidades = baseDeDatos.crearTablaConsulta(comando);
            foreach (DataRow columnaCursosAprobados in topHabilidades.Rows)
            {
                totalDeEstudiantesConCiertaHabilidad += Convert.ToInt32(columnaCursosAprobados["Cantidad"]);
                habilidadPorCurso.Add(new Tuple<string, int>(Convert.ToString(columnaCursosAprobados["nombreCursoFK"]), Convert.ToInt32(columnaCursosAprobados["Cantidad"])));

            }
            return habilidadPorCurso;

        }
        public int obtenerTotalDeEstudiantesConCiertaHabilidad() {
            return totalDeEstudiantesConCiertaHabilidad;
        }
        public List<Tuple<string, int>> obtenerIdiomasDeEstudiantePorCurso(string[] cursos, string idioma)
        {

            List<Tuple<string, int>> habilidadPorCurso = new List<Tuple<string, int>>();
            string consulta = " SELECT COUNT( DISTINCT emailEstudianteFK) AS Cantidad,nombreCursoFK" +
            " FROM Certificado WHERE ( ";
            for (int curso = 0; curso < cursos.Length; ++curso)
            {
                consulta += " nombreCursoFK = '" + cursos[curso] + "'";
                if (curso + 1 < cursos.Length)
                {
                    consulta += " OR ";
                }

            }
            consulta += " ) AND emailEstudianteFK IN (SELECT emailFK FROM Idiomas" +
             " WHERE(idiomaPK = @idioma))   GROUP BY nombreCursoFK ";
            SqlCommand comando = baseDeDatos.crearComandoParaConsulta(consulta);
            comando.Parameters.AddWithValue("@idioma", idioma);
            totalDeEstudiantesConCiertoIdioma = 0;
            DataTable topHabilidades = baseDeDatos.crearTablaConsulta(comando);
            foreach (DataRow columnaCursosAprobados in topHabilidades.Rows)
            {
                totalDeEstudiantesConCiertoIdioma += Convert.ToInt32(columnaCursosAprobados["Cantidad"]);
                habilidadPorCurso.Add(new Tuple<string, int>(Convert.ToString(columnaCursosAprobados["nombreCursoFK"]), Convert.ToInt32(columnaCursosAprobados["Cantidad"])));

            }
            return habilidadPorCurso;

        }
        public int obtenerTotalDeEstudiantesConCiertoIdioma()
        {
            return totalDeEstudiantesConCiertoIdioma;
        }
        public List<Tuple<string, int>> obtenerDistribucionDePaisPorCurso(string[] cursos, string pais)
        {

            List<Tuple<string, int>> distribucionDeEstudiantes = new List<Tuple<string, int>>();
            string consulta = "SELECT     COUNT(  DISTINCT emailEstudianteFK) as Cantidad,nombreCursoFK  FROM Certificado     " +
            " WHERE (";
            for (int curso = 0; curso < cursos.Length; ++curso)
            {
                consulta += " nombreCursoFK = '" + cursos[curso] + "'";
                if (curso + 1 < cursos.Length)
                {
                    consulta += " OR ";
                }

            }
            consulta += ") AND emailEstudianteFK IN (SELECT emailPK FROM Usuario"+  
            " WHERE PAIS = @pais ) group by nombreCursoFK";
            SqlCommand comando = baseDeDatos.crearComandoParaConsulta(consulta);
            comando.Parameters.AddWithValue("@pais", pais);
            DataTable estudiantesPorCurso = baseDeDatos.crearTablaConsulta(comando);
            totalEstudiantesPorPaisEnCursos = 0;
            foreach (DataRow columnaCursosAprobados in estudiantesPorCurso.Rows)
            {
                totalEstudiantesPorPaisEnCursos += Convert.ToInt32(columnaCursosAprobados["Cantidad"]);
                distribucionDeEstudiantes.Add(new Tuple<string, int>(Convert.ToString(columnaCursosAprobados["nombreCursoFK"]), Convert.ToInt32(columnaCursosAprobados["Cantidad"])));

            }
            return distribucionDeEstudiantes;

        }
        public int obtenerTotalDeEstudiantesDeEsePais()
        {
            return totalEstudiantesPorPaisEnCursos;
        }


    }
}
