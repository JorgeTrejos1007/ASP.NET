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
using PIBasesISGrupo1.Handler;

namespace PIBasesISGrupo1.Handler
{
    public class SimilitudPerfilHandler
    {

        private ConexionModel conexionBD;
        private SqlConnection conexion;
        private BaseDeDatosHandler baseDeDatos;
        public SimilitudPerfilHandler()
        {
            conexionBD = new ConexionModel();
            conexion = conexionBD.Connection();
            baseDeDatos = new BaseDeDatosHandler();   
        }

        public List<int> extraerFrecuenciaDeIdiomas(List<string> idiomas)
        {
            List<int> frecuenciaDeIdiomas = new List<int>();

            foreach (string idioma in idiomas)
            {
                Int32 cantidadDeIdiomaEspecifica = 0;
                string consulta = " SELECT COUNT(*)" +
                                " FROM  Idiomas" +
                                " WHERE idiomaPK=@idioma";
                SqlCommand comando = new SqlCommand(consulta, conexion);
                comando.Parameters.AddWithValue("@idioma", idioma);

                conexion.Open();
                cantidadDeIdiomaEspecifica = (Int32)comando.ExecuteScalar();
                conexion.Close();

                frecuenciaDeIdiomas.Add((int)cantidadDeIdiomaEspecifica);
            }

            return frecuenciaDeIdiomas;
        }

        public List<int> extraerFrecuenciaDeHabilidades(List<string> habilidades)
        {
            List<int> frecuenciaDeHabilidades = new List<int>();

            foreach (string habilidad in habilidades)
            {
                Int32 cantidadDeHabilidadEspecifica = 0;
                string consulta = " SELECT COUNT(*)" +
                                " FROM  Habilidades" +
                                " WHERE habilidadPK=@habilidad";
                SqlCommand comando = new SqlCommand(consulta, conexion);
                comando.Parameters.AddWithValue("@habilidad", habilidad);

                conexion.Open();
                cantidadDeHabilidadEspecifica = (Int32)comando.ExecuteScalar();
                conexion.Close();

                frecuenciaDeHabilidades.Add((int)cantidadDeHabilidadEspecifica);
            }

            return frecuenciaDeHabilidades;
        }

        public int extraerFrecuenciaDePais(string pais) {
            int frecuenciaDePais = 0;
        
            string consulta = " SELECT COUNT(*)" +
                            " FROM  Usuario" +
                            " WHERE pais=@pais";
            SqlCommand comando = new SqlCommand(consulta, conexion);
            comando.Parameters.AddWithValue("@pais", pais);

            conexion.Open();
            frecuenciaDePais = (Int32)comando.ExecuteScalar();
            conexion.Close();
           
            return frecuenciaDePais;
        }

    }
}
