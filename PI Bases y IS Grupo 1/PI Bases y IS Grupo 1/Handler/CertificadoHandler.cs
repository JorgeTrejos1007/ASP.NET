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
            Certificado certificadoTemporal;
            foreach (DataRow columna in tablaCurso.Rows)
            {
                certificadoTemporal = new Certificado
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

        public List<Certificado> obtenerMisCertificados(string email)
        {
            List<Certificado> listaDeCertificados = new List<Certificado>();
            string consulta = "SELECT imagenCertificado "+
                "FROM Certificado " +
                "WHERE  estado  = 'Aprobado'  AND  emailEstudianteFK= @email; ";
            SqlCommand comando = baseDeDatos.crearComandoParaConsulta(consulta);
            comando.Parameters.AddWithValue("@email", email);
            DataTable tablaCurso = baseDeDatos.crearTablaConsulta(comando);
            Certificado certificadoTemporal;
            foreach (DataRow columna in tablaCurso.Rows)
            {
                certificadoTemporal = new Certificado
                {
                     
                    imagenCertificado = Convert.IsDBNull(columna["imagenCertificado"]) ? null : (string)(columna["imagenCertificado"]) 
                    

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

    }
}
