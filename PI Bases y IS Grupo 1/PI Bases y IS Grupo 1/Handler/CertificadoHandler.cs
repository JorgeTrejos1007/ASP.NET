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

namespace PIBasesISGrupo1.Handler
{
    public class CertificadoHandler
    {
        private BaseDeDatosHandler baseDeDatos;
        public CertificadoHandler()
        {
           baseDeDatos = new BaseDeDatosHandler();
        }
        public List<Certificado> obtenerCertificadosNoAprobados(){
            List<Certificado> listaDeCertificados = new List<Certificado>();
            string consulta = "SELECT E.nombre AS 'nombreEducador'  ,Cu.nombrePK AS 'nombreCurso', S.nombre AS 'nombreEstudiante' " +
                "FROM Certifica Certi " +
                "JOIN Usuario E ON E.emailPK = Certi.emailEducadorFK " +
                "JOIN Curso Cu ON Cu.nombrePK = Certi.nombreCursoFK " +
                "JOIN Certificado C ON C.idCertificadoPK = Certi.idCertificadoFK " +
                "JOIN Usuario S ON Certi.emailEstudianteFK = S.emailPK " +
                "WHERE c.estadoDelCertificado = 'No Aprobado'; ";
            SqlCommand comando= baseDeDatos.crearComandoParaConsulta(consulta);
            DataTable tablaCurso = baseDeDatos.crearTablaConsulta(comando);
            Certificado certificadoTemporal;
            foreach (DataRow columna in tablaCurso.Rows)
            {
                certificadoTemporal = new Certificado
                {
                    nombreCurso = Convert.ToString(columna["nombreCurso"]),
                    nombreEducador = Convert.ToString(columna["emailEducador"]),
                    nombreEstudiante = Convert.ToString(columna["emailEstudiante"]),

                };
                listaDeCertificados.Add(certificadoTemporal);
            }
            return listaDeCertificados;
        }

    }
}
