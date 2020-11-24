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

        public List<Certificados> obtenerCertificadosNoAprobados(){
            List<Certificados> listaDeCertificados = new List<Certificados>();
            string consulta = "SELECT c.version AS 'version', C.emailEstudianteFK AS 'emailEstudiante',C.nombreCursoFK AS 'nombreCurso', E.nombre+ ' '+E.primerApellido+ ' '+E.segundoApellido AS 'nombreEducador'"+ 
             ", S.nombre + ' ' + S.primerApellido + ' ' + S.segundoApellido AS 'nombreEstudiante' "+
             "FROM Certificado C " +
             "JOIN Curso Cur ON C.nombreCursoFK = Cur.nombrePK " +
             "JOIN Usuario S ON C.emailEstudianteFK = s.emailPK " +
             "JOIN Usuario E ON Cur.emailEducadorFK = E.emailPK " +
             "JOIN Educador Ed ON Ed.emailEducadorFK = Cur.emailEducadorFK " +
             "WHERE C.estado = 'Completado'; ";
            SqlCommand comando= baseDeDatos.crearComandoParaConsulta(consulta);
            DataTable tablaCurso = baseDeDatos.crearTablaConsulta(comando);
            Certificados certificadoTemporal;
            foreach (DataRow columna in tablaCurso.Rows)
            {
                certificadoTemporal = new Certificados
                {
                    emailEstudiante = Convert.ToString(columna["emailEstudiante"]),
                    nombreCurso = Convert.ToString(columna["nombreCurso"]),
                    nombreEducador = Convert.ToString(columna["nombreEducador"]),
                    nombreEstudiante = Convert.ToString(columna["nombreEstudiante"]),
                    version= Convert.ToInt32(columna["version"]),


                };
                listaDeCertificados.Add(certificadoTemporal);
            }
            return listaDeCertificados;
        }

        public List<Certificados> obtenerMisCertificados(string emailEstudiante)
        {
            List<Certificados> listaDeCertificados = new List<Certificados>();
            string consulta = "SELECT C.fechaEmitido AS 'fecha',C.emailEstudianteFK AS 'emailEstudiante',C.nombreCursoFK AS 'nombreCurso', E.nombre+ ' '+E.primerApellido+ ' '+E.segundoApellido AS 'nombreEducador'" +
             ", S.nombre + ' ' + S.primerApellido + ' ' + S.segundoApellido AS 'nombreEstudiante', Ed.emailEducadorFK as 'firmaEducador', C.emailCoordinadorFK as 'firmaCoordinador', C.version AS 'version' " +
             "FROM Certificado C " +
             "JOIN Curso Cur ON C.nombreCursoFK = Cur.nombrePK " +
             "JOIN Usuario S ON C.emailEstudianteFK = s.emailPK " +
             "JOIN Usuario E ON Cur.emailEducadorFK = E.emailPK " +
             "JOIN Educador Ed ON Ed.emailEducadorFK = Cur.emailEducadorFK " +
             "JOIN Coordinador Coord ON C.emailCoordinadorFK = Coord.emailCoordinadorFK " +
             "WHERE C.estado = 'Aprobado' AND emailEstudianteFK = @email; ";
            SqlCommand comando = baseDeDatos.crearComandoParaConsulta(consulta);
            comando.Parameters.AddWithValue("@email", emailEstudiante);
            DataTable tablaCurso = baseDeDatos.crearTablaConsulta(comando);
            Certificados certificadoTemporal;
            foreach (DataRow columna in tablaCurso.Rows)
            {
                certificadoTemporal = new Certificados
                {
                    emailEstudiante = Convert.ToString(columna["emailEstudiante"]),
                    nombreCurso = Convert.ToString(columna["nombreCurso"]),
                    nombreEducador = Convert.ToString(columna["nombreEducador"]),
                    nombreEstudiante = Convert.ToString(columna["nombreEstudiante"]),
                    emailEducador = Convert.ToString(columna["firmaEducador"])  ,
                    emailCoordinador = Convert.ToString(columna["firmaCoordinador"])  ,
                    fecha = Convert.ToString(columna["fecha"]),
                    version = Convert.ToInt32(columna["version"])

                };
                listaDeCertificados.Add(certificadoTemporal);
            }
            return listaDeCertificados;
        }

        public bool aprobarCertificado(string emailEstudiante, string nombreCurso, string emailCoordinador ,int version) {
            string consulta = "UPDATE Certificado SET estado= 'Aprobado'," +
                " emailCoordinadorFK = @emailCoordinador,fechaEmitido=GETDATE()" +
            " WHERE emailEstudianteFK = @emailEstudiante AND nombreCursoFK = @curso AND version =@version";
            SqlCommand comando = baseDeDatos.crearComandoParaConsulta(consulta);
            comando.Parameters.AddWithValue("@emailEstudiante", emailEstudiante);
            comando.Parameters.AddWithValue("@emailCoordinador", emailCoordinador);
            comando.Parameters.AddWithValue("@curso", nombreCurso);
         
            comando.Parameters.AddWithValue("@version", version);
            //agregarFirmaCoordinador(idCertificado, email);
            bool exito = baseDeDatos.ejecutarComandoParaConsulta(comando);
            return exito;
        }

        private void agregarFirmaCoordinador(int idCertificado,string email) {
            SqlCommand comandoParaInsertarEnTablaDeMiembros = baseDeDatos.crearComandoParaConsulta("SP_InsertarFirmaCoordinador");
            comandoParaInsertarEnTablaDeMiembros.CommandType = CommandType.StoredProcedure;
            comandoParaInsertarEnTablaDeMiembros.Parameters.AddWithValue("@email", email);
            bool exito = baseDeDatos.ejecutarComandoParaConsulta(comandoParaInsertarEnTablaDeMiembros);
        }

        public List<string> obtenerCursosAprobados(string emailEstudiante) {
            List<string> cursosAprobados = new List<string>();
            string consulta = "SELECT nombreCursoFK, version FROM Certificado WHERE emailEstudianteFK=@emailEstudiante AND estado='Aprobado'";
            SqlCommand comando = baseDeDatos.crearComandoParaConsulta(consulta);
            comando.Parameters.AddWithValue("@emailEstudiante", emailEstudiante);
            DataTable tablaCertificado = baseDeDatos.crearTablaConsulta(comando);
          
            foreach (DataRow columna in tablaCertificado.Rows)
            {
                cursosAprobados.Add(Convert.ToString(columna["nombreCursoFK"])+ " v."+ Convert.ToString(columna["version"]));
            }
            return cursosAprobados;
        }
    }
}
