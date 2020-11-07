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

        public List<double> extraerPesoDeIdiomas(List<string> idiomas)
        {
            List<double> pesoDeIdiomas = new List<double>();

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

                pesoDeIdiomas.Add(1/(double)cantidadDeIdiomaEspecifica);
            }

            return pesoDeIdiomas;
        }

        public List<double> extraerPesoDeHabilidades(List<string> habilidades)
        {
            List<double> frecuenciaDeHabilidades = new List<double>();

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

                frecuenciaDeHabilidades.Add(1/(double)cantidadDeHabilidadEspecifica);
            }

            return frecuenciaDeHabilidades;
        }

        public double extraerPesoDePais(string pais) {
            
            string consulta = " SELECT COUNT(*)" +
                            " FROM  Usuario" +
                            " WHERE pais=@pais";
            SqlCommand comando = new SqlCommand(consulta, conexion);
            comando.Parameters.AddWithValue("@pais", pais);

            conexion.Open();
            double frecuenciaDePais = (Int32)comando.ExecuteScalar();
            conexion.Close();
           
            return (1/frecuenciaDePais);
        }

        public List<string> extraerPerfilesConAlMenosUnaSimilitud () {
            List<string> correos = new List<string>();

            string consulta = " SELECT COUNT(*)" +
                            " FROM  Usuario" +
                            " WHERE pais=@pais";

            /*SELECT emailFK
            FROM(
                (SELECT emailFK FROM Idiomas WHERE idiomaPK = 'Español' OR idiomaPK = 'Ingles' GROUP BY emailFK)
                UNION ALL
                (SELECT emailFK FROM Habilidades WHERE habilidadPK = 'Flexibilidad' OR habilidadPK = 'Liderazgo' GROUP BY emailFK)
                UNION ALL
                (SELECT emailPK FROM Usuario WHERE pais = 'Costa Rica' GROUP BY emailPK)
		        ) t
            GROUP BY emailFK*/

            SqlCommand comando = new SqlCommand(consulta, conexion);
            comando.Parameters.AddWithValue("@pais", pais);

            conexion.Open();
            frecuenciaDePais = (Int32)comando.ExecuteScalar();
            conexion.Close();

            return (1 / frecuenciaDePais);
        }

    }
}
