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
            string consulta = "SELECT  C.idCertificadoPK AS 'idCertificado', E.nombre+ ' '+E.primerApellido+ ' '+E.segundoApellido AS 'nombreEducador'" +
                "  ,Cu.nombrePK AS 'nombreCurso', S.nombre+ ' '+S.primerApellido+ ' '+S.segundoApellido AS 'nombreEstudiante' " +
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
                    nombreEducador = Convert.ToString(columna["nombreEducador"]),
                    nombreEstudiante = Convert.ToString(columna["nombreEstudiante"]),
                    idCertificado= Convert.ToInt32(columna["idCertificado"])

                };
                listaDeCertificados.Add(certificadoTemporal);
            }
            return listaDeCertificados;
        }

        public List<Certificado> obtenerMisCertificados(string email)
        {
            List<Certificado> listaDeCertificados = new List<Certificado>();
            string consulta = "SELECT  C.idCertificadoPK AS 'idCertificado', E.nombre+ ' '+E.primerApellido+ ' '+E.segundoApellido AS 'nombreEducador'" +
                "  ,Cu.nombrePK AS 'nombreCurso', S.nombre+ ' '+S.primerApellido+ ' '+S.segundoApellido AS 'nombreEstudiante' " +
                ",c.firmaEducador AS 'firmaEducador',c.firmaCoordinador AS 'firmaCoordinador' "+
                "FROM Certifica Certi " +
                "JOIN Usuario E ON E.emailPK = Certi.emailEducadorFK " +
                "JOIN Curso Cu ON Cu.nombrePK = Certi.nombreCursoFK " +
                "JOIN Certificado C ON C.idCertificadoPK = Certi.idCertificadoFK " +
                "JOIN Usuario S ON Certi.emailEstudianteFK = S.emailPK " +
                "WHERE c.estadoDelCertificado = 'Aprobado'  AND Certi.emailEstudianteFK= @email; ";
            SqlCommand comando = baseDeDatos.crearComandoParaConsulta(consulta);
            comando.Parameters.AddWithValue("@email", email);
            DataTable tablaCurso = baseDeDatos.crearTablaConsulta(comando);
            Certificado certificadoTemporal;
            foreach (DataRow columna in tablaCurso.Rows)
            {
                certificadoTemporal = new Certificado
                {
                    nombreCurso = Convert.ToString(columna["nombreCurso"]),
                    nombreEducador = Convert.ToString(columna["nombreEducador"]),
                    nombreEstudiante = Convert.ToString(columna["nombreEstudiante"]),
                    idCertificado = Convert.ToInt32(columna["idCertificado"]),
                    firmaEducador = Convert.ToString(columna["firmaEducador"]),
                    firmaCoordinador = Convert.ToString(columna["firmaCoordinador"]),

                };
                listaDeCertificados.Add(certificadoTemporal);
            }
            return listaDeCertificados;
        }
        public bool aprobarCertificado(int idCertificado,string email) {
            string consulta = "UPDATE Certificado SET estadoDelCertificado= 'Aprobado',fechaEmitido=GETDATE()" +
            " WHERE idCertificadoPK = @id;";
            SqlCommand comando = baseDeDatos.crearComandoParaConsulta(consulta);
            comando.Parameters.AddWithValue("@id", idCertificado);
            agregarFirmaCoordinador(idCertificado, email);
            bool exito = baseDeDatos.ejecutarComandoParaConsulta(comando);
            return exito;
        }

        private void agregarFirmaCoordinador(int idCertificado,string email) {
            SqlCommand comandoParaInsertarEnTablaDeMiembros = baseDeDatos.crearComandoParaConsulta("SP_InsertarFirmaCoordinador");
            comandoParaInsertarEnTablaDeMiembros.CommandType = CommandType.StoredProcedure;
            comandoParaInsertarEnTablaDeMiembros.Parameters.AddWithValue("@email", email);
            bool exito = baseDeDatos.ejecutarComandoParaConsulta(comandoParaInsertarEnTablaDeMiembros);
       }

    }
}
