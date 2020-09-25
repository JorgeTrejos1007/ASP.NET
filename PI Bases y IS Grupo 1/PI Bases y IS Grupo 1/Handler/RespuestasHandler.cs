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
    public class RespuestasHandler
    {
        private ConexionModel conexionBD;
        private SqlConnection conexion;
        public RespuestasHandler()
        {
            conexionBD = new ConexionModel();
            conexion = conexionBD.Connection();
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
        public bool crearRespuesta(RespuestaModel respuesta)
        {
            string consulta = "INSERT INTO Respuestas(nombre, idEncuestaFK, idPreguntaFK, respuesta) "
            + "VALUES (@nombre,@encuestaID,@preguntaID,@respuesta) ";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);


            //comandoParaConsulta.Parameters.AddWithValue("@id", respuesta.id);
            comandoParaConsulta.Parameters.AddWithValue("@nombre", respuesta.nombre);
            comandoParaConsulta.Parameters.AddWithValue("@encuestaID", respuesta.encuestaID);
            comandoParaConsulta.Parameters.AddWithValue("@preguntaID", respuesta.preguntaID);
            comandoParaConsulta.Parameters.AddWithValue("@respuesta", respuesta.respuesta);

            bool exito = comandoParaConsulta.ExecuteNonQuery() >= 1;
            conexion.Close();
            return exito;
        }
    }
}
