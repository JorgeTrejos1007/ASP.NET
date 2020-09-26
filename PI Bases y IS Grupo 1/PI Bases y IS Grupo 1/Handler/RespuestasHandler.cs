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

            conexion.Open();
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

            conexion.Open();
            bool exito = comandoParaConsulta.ExecuteNonQuery() >= 1;
            conexion.Close();
            return exito;
        }

        public List<MostarRespuestaModel> obtenerRespuestas(int encuestaID)
        {
            List<MostarRespuestaModel> respuestas = new List<MostarRespuestaModel>();
            string consulta = " SELECT Preguntas.pregunta, Respuestas.idPreguntaFK, Respuestas.nombre, Respuestas.respuesta " +
                "FROM Preguntas JOIN Respuestas ON Preguntas.idEncuestaFK = Respuestas.idEncuestaFK AND Preguntas.idPregunta = Respuestas.idPreguntaFK " +
                "WHERE Respuestas.idEncuestaFK=@encuestaID ORDER BY Respuestas.idPreguntaFK";

            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);
            comandoParaConsulta.Parameters.AddWithValue("@encuestaID", encuestaID);

            DataTable consultaFormatoTabla = new DataTable();

            adaptadorParaTabla.Fill(consultaFormatoTabla);



            foreach (DataRow columna in consultaFormatoTabla.Rows)
            {
                respuestas.Add(
                new MostarRespuestaModel
                {
                   
                    preguntaID = Convert.ToInt32(columna["idPreguntaFK"]),
                    nombre = Convert.ToString(columna["nombre"]),
                    respuesta = Convert.ToString(columna["respuesta"]),
                    pregunta = Convert.ToString(columna["pregunta"]),
                   
                });

            }
            return respuestas;
        }
    }
}
