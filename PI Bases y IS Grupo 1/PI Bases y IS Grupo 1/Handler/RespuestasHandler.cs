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
            string consulta = "INSERT INTO Respuesta(idEncuestaFK, idPreguntaFK, correoEncuestado, respuesta) "
            + "VALUES (@encuestaID,@preguntaID,@correoEncuestado,@respuesta) ";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);

            comandoParaConsulta.Parameters.AddWithValue("@correoEncuestado", respuesta.correoEncuestado);
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
            string consulta = " SELECT Pregunta.pregunta, Respuesta.idPreguntaFK, Respuesta.correoEncuestado, Respuesta.respuesta " +
                "FROM Pregunta JOIN Respuesta ON Pregunta.idEncuestaFK = Respuesta.idEncuestaFK AND Pregunta.idPreguntaPK = Respuesta.idPreguntaFK " +
                "WHERE Respuesta.idEncuestaFK=@encuestaID ORDER BY Respuesta.idPreguntaFK";

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
                    correoEncuestado = Convert.ToString(columna["correoEncuestado"]),
                    respuesta = Convert.ToString(columna["respuesta"]),
                    pregunta = Convert.ToString(columna["pregunta"]),
                   
                });

            }
            return respuestas;
        }

        public int cantidadVecesElegidaUnaOpcion(int encuestaID, int preguntaID, string opcion)
        {
            if (opcion !="") {
                int cantidadRespuestaOpcion = 0;
                string consulta = "SELECT COUNT(respuesta) AS [cantidad] FROM Respuesta WHERE respuesta = @opcion AND idPreguntaFK = @preguntaID AND idEncuestaFK = @encuestaID";
                SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
                SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);

                comandoParaConsulta.Parameters.AddWithValue("@encuestaID", encuestaID);
                comandoParaConsulta.Parameters.AddWithValue("@preguntaID", preguntaID);
                comandoParaConsulta.Parameters.AddWithValue("@opcion", opcion);


                conexion.Open();
                SqlDataReader lectorDeDatos = comandoParaConsulta.ExecuteReader();
                lectorDeDatos.Read();
                cantidadRespuestaOpcion = (int)lectorDeDatos["cantidad"];
                conexion.Close();
                return cantidadRespuestaOpcion;
            }
            else
            {
                return -1;
            }
        }
    }
}
