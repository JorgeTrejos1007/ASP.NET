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
            string consulta = "INSERT INTO Curso(nombre,emailUsuarioFK,documentoInformativo,tipoDocumentoInformativo)"
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
    }
}
