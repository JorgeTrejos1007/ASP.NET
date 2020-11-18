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




    }
}
