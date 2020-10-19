﻿using System;
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

            conexion.Open();
            adaptadorParaTabla.Fill(consultaFormatoTabla);
            conexion.Close();
            return consultaFormatoTabla;
        }
        public bool crearPregunta(PreguntaModel pregunta)
        {
            string consulta = "INSERT INTO Pregunta(idEncuestaFK, pregunta, opcion1, opcion2, opcion3, opcion4) "
            + "VALUES (@idEncuestaFK,@pregunta,@opcion1,@opcion2, @opcion3, @opcion4) ";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);

        
            comandoParaConsulta.Parameters.AddWithValue("@idEncuestaFK", pregunta.encuestaID);
            comandoParaConsulta.Parameters.AddWithValue("@pregunta", pregunta.pregunta);
            comandoParaConsulta.Parameters.AddWithValue("@opcion1", pregunta.opcion1);
            comandoParaConsulta.Parameters.AddWithValue("@opcion2", pregunta.opcion2);
            if (pregunta.opcion3  != null && pregunta.opcion4 != null)
            {
                comandoParaConsulta.Parameters.AddWithValue("@opcion3", pregunta.opcion3);
                comandoParaConsulta.Parameters.AddWithValue("@opcion4", pregunta.opcion4);
            }
            else if(pregunta.opcion3 != null)
            {
                comandoParaConsulta.Parameters.AddWithValue("@opcion3", pregunta.opcion3);
                comandoParaConsulta.Parameters.AddWithValue("@opcion4", DBNull.Value);
            }
            else
            {
                comandoParaConsulta.Parameters.AddWithValue("@opcion3", DBNull.Value);
                comandoParaConsulta.Parameters.AddWithValue("@opcion4", DBNull.Value);
            }

            conexion.Open();
            bool exito = comandoParaConsulta.ExecuteNonQuery() >= 1;
            conexion.Close();
            return exito;
        }

        public List<PreguntaModel> obtenerPreguntas(int encuestaID)
        {
            List<PreguntaModel> preguntas = new List<PreguntaModel>();
            string consulta = "SELECT * FROM Pregunta WHERE idEncuestaFK=@encuestaID";
          
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);
            comandoParaConsulta.Parameters.AddWithValue("@encuestaID", encuestaID);

            DataTable consultaFormatoTabla = new DataTable();

            adaptadorParaTabla.Fill(consultaFormatoTabla);
            
          

            foreach (DataRow columna in consultaFormatoTabla.Rows)
            {
                preguntas.Add(
                new PreguntaModel
                {
                    encuestaID = Convert.ToInt32(columna["idEncuestaFK"]),
                    preguntaID = Convert.ToInt32(columna["idPreguntaPK"]),
                    pregunta = Convert.ToString(columna["pregunta"]),
                    opcion1 = Convert.ToString(columna["opcion1"]),
                    opcion2 = Convert.ToString(columna["opcion2"]),
                    opcion3 = Convert.ToString(columna["opcion3"]),
                    opcion4 = Convert.ToString(columna["opcion4"]),
                });

            }
            return preguntas;
        }

        public bool borrarPregunta(int idEncuesta, int idPregunta)
        {
            string consulta = "DELETE FROM Pregunta WHERE idPreguntaPK=@idPreg AND idEncuestaFK=@idEnc";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);
            comandoParaConsulta.Parameters.AddWithValue("@idPreg", idPregunta);
            comandoParaConsulta.Parameters.AddWithValue("@idEnc", idEncuesta);

            conexion.Open();
            bool exito = comandoParaConsulta.ExecuteNonQuery() >= 1;
            conexion.Close();
            return exito;
        }

        public PreguntaModel obtenerTuplaPregunta(int idEncuesta, int idPregunta)
        {
            PreguntaModel pregunta = new PreguntaModel();
            string consulta = "SELECT * FROM Pregunta WHERE idEncuestaFK=@idEncuesta AND idPreguntaPK=@idPregunta ";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);
            comandoParaConsulta.Parameters.AddWithValue("@idEncuesta", idEncuesta);
            comandoParaConsulta.Parameters.AddWithValue("@idPregunta", idPregunta);

            conexion.Open();
            SqlDataReader lectorDeDatos = comandoParaConsulta.ExecuteReader();
            lectorDeDatos.Read();
            pregunta.encuestaID = (int)lectorDeDatos["idEncuestaFK"];
            pregunta.preguntaID = (int)lectorDeDatos["idPreguntaPK"];
            pregunta.pregunta = (string)lectorDeDatos["pregunta"];
            pregunta.opcion1 = (string)lectorDeDatos["opcion1"];
            pregunta.opcion2 = (string)lectorDeDatos["opcion2"];
            if (lectorDeDatos["opcion3"] != DBNull.Value && lectorDeDatos["opcion4"] != DBNull.Value)
            {
                pregunta.opcion3 = (string)lectorDeDatos["opcion3"];
                pregunta.opcion4 = (string)lectorDeDatos["opcion4"];
            }
            else if (lectorDeDatos["opcion3"] != DBNull.Value)
            {
                pregunta.opcion3 = (string)lectorDeDatos["opcion3"];
                pregunta.opcion4 = null;
            }
            else {
                pregunta.opcion3 = null;
                pregunta.opcion4 = null;
            }
           
            conexion.Close();
            return pregunta;
        }

        public bool modificarPregunta(PreguntaModel pregunta)
        {
            string consulta = "UPDATE Pregunta SET pregunta=@pregunta, opcion1=@opcion1, opcion2=@opcion2, opcion3=@opcion3, opcion4=@opcion4 WHERE idEncuestaFK=@idEncuesta AND idPreguntaPK=@idPregunta";

            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);

            comandoParaConsulta.Parameters.AddWithValue("@idEncuesta", pregunta.encuestaID);
            comandoParaConsulta.Parameters.AddWithValue("@idPregunta", pregunta.preguntaID);
            comandoParaConsulta.Parameters.AddWithValue("@pregunta", pregunta.pregunta);
            comandoParaConsulta.Parameters.AddWithValue("@opcion1", pregunta.opcion1);
            comandoParaConsulta.Parameters.AddWithValue("@opcion2", pregunta.opcion2);
            if (pregunta.opcion3 != null && pregunta.opcion4 != null)
            {
                comandoParaConsulta.Parameters.AddWithValue("@opcion3", pregunta.opcion3);
                comandoParaConsulta.Parameters.AddWithValue("@opcion4", pregunta.opcion4);
            }
            else if (pregunta.opcion3 != null)
            {
                comandoParaConsulta.Parameters.AddWithValue("@opcion3", pregunta.opcion3);
                comandoParaConsulta.Parameters.AddWithValue("@opcion4", DBNull.Value);
            }
            else
            {
                comandoParaConsulta.Parameters.AddWithValue("@opcion3", DBNull.Value);
                comandoParaConsulta.Parameters.AddWithValue("@opcion4", DBNull.Value);
            }

            conexion.Open();
            bool exito = comandoParaConsulta.ExecuteNonQuery() >= 1;
            conexion.Close();
            return exito;
        }
    }
}
