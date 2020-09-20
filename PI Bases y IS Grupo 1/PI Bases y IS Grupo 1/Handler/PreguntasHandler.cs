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
    public class PreguntasHandler
    {
        private ConexionModel conexionBD;
        private SqlConnection conexion;
        public PreguntasHandler()
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
        public bool crearPregunta(PreguntaModel pregunta)
        {
            string consulta = "INSERT INTO Preguntas(idEncuestaFK, pregunta, opcion1, opcion2, opcion3, opcion4) "
            + "VALUES (@idEncuestaFK,@pregunta,@opcion1,@opcion2, @opcion3, @opcion4) ";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);

           // pregunta.encuestaID = idEncuesta;
            comandoParaConsulta.Parameters.AddWithValue("@idEncuestaFK", pregunta.encuestaID);
            //comandoParaConsulta.Parameters.AddWithValue("@idPregunta", pregunta.preguntaID);
            comandoParaConsulta.Parameters.AddWithValue("@pregunta", pregunta.pregunta);
            comandoParaConsulta.Parameters.AddWithValue("@opcion1", pregunta.opcion1);
            comandoParaConsulta.Parameters.AddWithValue("@opcion2", pregunta.opcion2);
            comandoParaConsulta.Parameters.AddWithValue("@opcion3", pregunta.opcion3);
            comandoParaConsulta.Parameters.AddWithValue("@opcion4", pregunta.opcion4);

            bool exito = comandoParaConsulta.ExecuteNonQuery() >= 1;
            conexion.Close();
            return exito;
        }
    }
}
