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
    public class EncuestasHandler
    {
        private ConexionModel conexionBD;
        private SqlConnection conexion;
        public EncuestasHandler()
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
        public bool crearEncuesta(EncuestaModel encuesta)
        {
            string consulta = "INSERT INTO Encuesta(topicoFK, nombreEncuesta, autor, vigencia) "
            + "VALUES (@topico,@nombreEncuesta,@autor,@vigencia) ";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);

            
            comandoParaConsulta.Parameters.AddWithValue("@topico", encuesta.topico);
            comandoParaConsulta.Parameters.AddWithValue("@nombreEncuesta", encuesta.nombreEncuesta);
            comandoParaConsulta.Parameters.AddWithValue("@autor", encuesta.autor);
            comandoParaConsulta.Parameters.AddWithValue("@vigencia", encuesta.vigencia);

            conexion.Open();
            bool exito = comandoParaConsulta.ExecuteNonQuery() >= 1;
            conexion.Close();
            return exito;
        }
        public List<EncuestaModel> obtenerEncuestas()
        {
            List<EncuestaModel> encuestas = new List<EncuestaModel>();
            string consulta = "SELECT * FROM Encuesta";
            DataTable tablaResultado = crearTablaConsulta(consulta);
            foreach (DataRow columna in tablaResultado.Rows)
            {
                encuestas.Add(
                new EncuestaModel
                {
                    id = Convert.ToInt32(columna["idEncuesta"]),
                    topico = Convert.ToString(columna["topicoFK"]),
                    nombreEncuesta = Convert.ToString(columna["nombreEncuesta"]),
                    autor = Convert.ToString(columna["autor"]),
                    vigencia = Convert.ToInt32(columna["vigencia"])
                });

            }
            return encuestas;
        }
        public bool modificarEncuesta(EncuestaModel encuesta)
        {
            string consulta = "UPDATE Encuesta SET topicoFK=@topico, nombreEncuesta=@nombreEncuesta, autor=@autor, vigencia=@vigencia WHERE idEncuesta=@id"; 
            
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);

            comandoParaConsulta.Parameters.AddWithValue("@id", encuesta.id);
            comandoParaConsulta.Parameters.AddWithValue("@topico", encuesta.topico);
            comandoParaConsulta.Parameters.AddWithValue("@nombreEncuesta", encuesta.nombreEncuesta);
            comandoParaConsulta.Parameters.AddWithValue("@autor", encuesta.autor);
            comandoParaConsulta.Parameters.AddWithValue("@vigencia", encuesta.vigencia);

            conexion.Open();
            bool exito = comandoParaConsulta.ExecuteNonQuery() >= 1;
            conexion.Close();
            return exito;
        }

        public EncuestaModel obtenerTuplaEncuesta(int id)
        {
            EncuestaModel encuesta =  new EncuestaModel();
            string consulta = "SELECT * FROM Encuesta WHERE idEncuesta=@id";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);
            comandoParaConsulta.Parameters.AddWithValue("@id", id);

            conexion.Open();
            SqlDataReader lectorDeDatos = comandoParaConsulta.ExecuteReader();
            lectorDeDatos.Read();
            encuesta.id = id;
            encuesta.topico = (string)lectorDeDatos["topicoFK"];
            encuesta.nombreEncuesta = (string)lectorDeDatos["nombreEncuesta"];
            encuesta.autor = (string)lectorDeDatos["autor"];
            encuesta.vigencia = (int)lectorDeDatos["vigencia"];
            conexion.Close();
            return encuesta;
        }

        public bool borrarEncuesta(int id) {
            string consulta = "DELETE FROM Encuesta WHERE idEncuesta=@id";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);
            comandoParaConsulta.Parameters.AddWithValue("@id", id);

            conexion.Open();
            bool exito = comandoParaConsulta.ExecuteNonQuery() >= 1;
            conexion.Close();
            return exito;
        }
        public List<string> obtenerTodosLosEmails()        {            List<string> miembrosEmail = new List<string>();            string consulta = "SELECT email FROM Usuario";            DataTable tablaResultado = crearTablaConsulta(consulta);            foreach (DataRow columna in tablaResultado.Rows)            {                miembrosEmail.Add(Convert.ToString(columna["email"]));            }            return miembrosEmail;        }

    }
}
