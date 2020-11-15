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



    }
}
