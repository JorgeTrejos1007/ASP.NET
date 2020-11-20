using System;
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
        public GraficoHandler()
        {

            baseDeDatos = new BaseDeDatosHandler();

        }


        public List<string> obtenerEstudiantesCertificadosDeUnCurso(string nombreCurso) {
            string consulta = "SELECT DISTINCT emailEstudianteFK FROM Certificado WHERE nombreCursoFK=@nombreCurso AND estado='Aprobado';";
            SqlCommand comandoParaConsulta = baseDeDatos.crearComandoParaConsulta(consulta);
            comandoParaConsulta.Parameters.AddWithValue("@nombreCurso", nombreCurso);
            List<string> estudiantesConCertificado = baseDeDatos.obtenerDatosDeColumna(comandoParaConsulta, "emailEstudianteFK");
            return estudiantesConCertificado;
        }

        public List<string> obtenerEstudiantesQueEstanCursandoUnCurso(string nombreCurso)
        {
            string consulta = "SELECT DISTINCT emailEstudianteFK FROM Certificado WHERE nombreCursoFK=@nombreCurso AND estado<>'Aprobado';";
            SqlCommand comandoParaConsulta = baseDeDatos.crearComandoParaConsulta(consulta);
            comandoParaConsulta.Parameters.AddWithValue("@nombreCurso", nombreCurso);
            List<string> estudiantes = baseDeDatos.obtenerDatosDeColumna(comandoParaConsulta, "emailEstudianteFK");
            return estudiantes;
        }
        public List<Tuple<string, int>> obtenerLasHabilidadesMasFrecuentes(string [] cursos){

            List<Tuple<string, int>> habilidades = new List<Tuple<string, int>>();  
            string consulta = " SELECT TOP 5 COUNT(habilidadPK) AS Cantidad,habilidadPK" +            " FROM Habilidades WHERE emailFK IN (SELECT emailEstudianteFK FROM Certificado " +            " WHERE";
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
            string consulta = " SELECT TOP 5 COUNT(pais) AS Cantidad,pais" +            " FROM Usuario  WHERE emailPK IN (SELECT emailEstudianteFK FROM Certificado " +            " WHERE";
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
            string consulta = " SELECT DISTINCT COUNT(rolUsuarioPK) AS cantidad, rolUsuarioPK" +                             " FROM Rol WHERE emailUsuarioFK IN(SELECT emailPK FROM Usuario WHERE pais=@pais)" +
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


    }
}
