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
    public class CursoHandler
    {
        private ConexionModel conexionBD;
        private SqlConnection conexion;
        public CursoHandler()
        {
            conexionBD = new ConexionModel();
            conexion = conexionBD.Connection();
        }

        private byte[] obtenerBytes(IFormFile archivo)
        {
            byte[] bytes;
            MemoryStream ms = new MemoryStream();
            archivo.OpenReadStream().CopyTo(ms);
            bytes = ms.ToArray();
            return bytes;
        }

        public bool proponerCurso(Cursos curso, IFormFile archivo) {
            string consulta = "INSERT INTO Curso(nombre,emailEducadorFK,documentoInformativo,tipoDocumentoInformativo)"
          + "VALUES (@nombreCurso,@emailEducador,@documentoInformativo,@tipoDocumentoInformativo)";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);
            comandoParaConsulta.Parameters.AddWithValue("@nombreCurso", curso.nombre);
            comandoParaConsulta.Parameters.AddWithValue("@emailEducador", curso.emailDelQueLoPropone);
            comandoParaConsulta.Parameters.AddWithValue("@documentoInformativo", obtenerBytes(archivo));
            comandoParaConsulta.Parameters.AddWithValue("@tipoDocumentoInformativo", archivo.ContentType);
            conexion.Open();
            bool exito = comandoParaConsulta.ExecuteNonQuery() >= 1;
            if (curso.topicos.Length > 0)
            {
               exito= insertarRelacionConTopico(curso); 
            }
            conexion.Close();
            return exito;
        }
        public bool insertarRelacionConTopico(Cursos curso) {
            bool exito = false;
            string consultaATablaContiene = "INSERT INTO Contiene(nombreCursoFK,nombreTopicoFK)"
            + "VALUES (@nombreCurso,@nombreTopico)";
            for (int topico = 0; topico < curso.topicos.Length; ++topico) {
                SqlCommand comandoParaConsulta = new SqlCommand(consultaATablaContiene, conexion);
                SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);
                comandoParaConsulta.Parameters.AddWithValue("@nombreCurso", curso.nombre);
                comandoParaConsulta.Parameters.AddWithValue("@nombreTopico", curso.topicos[topico]);
                exito = comandoParaConsulta.ExecuteNonQuery() >= 1;
            }
            return exito;

        }
        public List<Tuple<Cursos,Miembro>> obtenerCursosPropuestos()
        {
            List<Tuple<Cursos, Miembro>> cursos = new List<Tuple<Cursos, Miembro>>();
            string consultaCategorias = "SELECT C.nombre AS nombreCurso,C.estado AS estado,C.precio AS precio," +
                " C.emailEducadorFK AS emailEducador,C.tipoDocumentoInformativo AS tipoDocumento,C.documentoInformativo AS documento,E.nombre AS nombreEducador,E.primerApellido AS primerApellido,E.segundoApellido AS segundoApellido " +
            "FROM Curso C JOIN Usuario E ON C.emailEducadorFK = E.email " + "WHERE estado='No aprobado'";
            DataTable tablaCurso = crearTablaConsulta(consultaCategorias);
            Cursos cursoTemporal;
            Miembro educadorTemporal;
            foreach (DataRow columna in tablaCurso.Rows)
            {
                cursoTemporal= new Cursos
                {
                    nombre = Convert.ToString(columna["nombreCurso"]),
                    estado = Convert.ToString(columna["estado"]),
                    precio=Convert.ToDouble(columna["precio"]),
                    emailDelEducador = Convert.ToString(columna["emailEducador"]),
                    byteArrayDocument = (byte [])columna["documento"],
                    tipoDocInformativo = Convert.ToString(columna["tipoDocumento"])
                };
                educadorTemporal = new Miembro
                {
                    nombre = Convert.ToString(columna["nombreEducador"]), 
                    primerApellido = Convert.ToString(columna["primerApellido"]),
                    segundoApellido = Convert.ToString(columna["segundoApellido"])

                };
                cursos.Add(new Tuple<Cursos, Miembro>(cursoTemporal, educadorTemporal));

            
            }
            return cursos;
        }

        public List<Tuple<Cursos, Miembro>> obtenerCursosDisponibles()
        {
            List<Tuple<Cursos, Miembro>> cursos = new List<Tuple<Cursos, Miembro>>();
            string consultaCategorias = "SELECT C.nombre AS nombreCurso,C.estado AS estado,C.precio AS precio," +
                " C.emailEducadorFK AS emailEducador,C.tipoDocumentoInformativo AS tipoDocumento,C.documentoInformativo AS documento,E.nombre AS nombreEducador,E.primerApellido AS primerApellido,E.segundoApellido AS segundoApellido " +
            "FROM Curso C JOIN Usuario E ON C.emailEducadorFK = E.email " + "WHERE estado='Aprobado'";
            DataTable tablaCurso = crearTablaConsulta(consultaCategorias);
            Cursos cursoTemporal;
            Miembro educadorTemporal;
            foreach (DataRow columna in tablaCurso.Rows)
            {
                cursoTemporal = new Cursos
                {
                    nombre = Convert.ToString(columna["nombreCurso"]),
                    estado = Convert.ToString(columna["estado"]),
                    precio = Convert.ToDouble(columna["precio"]),
                    emailDelEducador = Convert.ToString(columna["emailEducador"]),
                    byteArrayDocument = (byte[])columna["documento"],
                    tipoDocInformativo = Convert.ToString(columna["tipoDocumento"])
                };
                educadorTemporal = new Miembro
                {
                    nombre = Convert.ToString(columna["nombreEducador"]),
                    primerApellido = Convert.ToString(columna["primerApellido"]),
                    segundoApellido = Convert.ToString(columna["segundoApellido"])

                };
                cursos.Add(new Tuple<Cursos, Miembro>(cursoTemporal, educadorTemporal));


            }
            return cursos;
        }



        private DataTable crearTablaConsulta(string consulta)
        {
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);
            DataTable consultaFormatoTabla = new DataTable();

            adaptadorParaTabla.Fill(consultaFormatoTabla);
            conexion.Close();
            return consultaFormatoTabla;
        }
        public bool aprobarCurso(string nombreCurso)
        {
            
            string consulta = "UPDATE Curso " + "SET estado='Aprobado' " + "WHERE nombre=@nombreCurso";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);
            comandoParaConsulta.Parameters.AddWithValue("@nombreCurso", nombreCurso);
            conexion.Open();
            bool exito = comandoParaConsulta.ExecuteNonQuery() >= 1;
            conexion.Close();
            return exito;
        }

    }
}
