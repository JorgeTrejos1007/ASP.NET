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

        public List<double> extraerPesoDeIdiomas(string[] idiomas)
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

        public List<double> extraerPesoDeHabilidades(string[] habilidades)
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

        public double extraerPesoDePaises(string pais) {
            
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
        
        public List<string> extraerCorreosConAlMenosUnaSimilitud(string[] atributos, string consulta)
        {
            List<string> correos = new List<string>();

            SqlCommand comando = new SqlCommand(consulta, conexion);

            for (int i = 0; i < atributos.Length; i++) {
                comando.Parameters.AddWithValue("@"+ atributos[i], atributos[i]);
            }

            DataTable tablaCorreos = baseDeDatos.crearTablaConsulta(comando);

            foreach (DataRow columna in tablaCorreos.Rows)
            {
                correos.Add(Convert.ToString(columna["emailFK"]));
            }
            return correos;
        }

        public string crearConsultaTamañoDinamicoHabilidades(string[] habilidades)
        {
            string consulta = "SELECT DISTINCT emailFK" + " FROM Habilidades" + " WHERE ";
            for (int index = 0; index < habilidades.Length; index++)
            {
                if (index < habilidades.Length - 1)
                {
                    consulta += "habilidadPK = @" + habilidades[index] + " OR ";
                }
                else
                {
                    consulta += "habilidadPK = @" + habilidades[index];
                }
            }
            return consulta;
        }

        public string crearConsultaTamanoDinamicoIdiomas(string[] idiomas)
        {
            string consulta = "SELECT DISTINCT emailFK" + " FROM Idiomas" + " WHERE ";
            for (int index = 0; index < idiomas.Length; index++)
            {
                if (index < idiomas.Length - 1)
                {
                    consulta += "idiomaPK = @" + idiomas[index] + " OR ";
                }
                else
                {
                    consulta += "idiomaPK = @" + idiomas[index];
                }
            }
            return consulta;
        }

        public List<string> extraerCorreosConSimiltudEnPais(string pais)
        {
            List<string> correos = new List<string>();

            string consulta = " SELECT emailPK" +
                            " FROM  Usuario" +
                            " WHERE pais=@pais";

            SqlCommand comando = new SqlCommand(consulta, conexion);
            comando.Parameters.AddWithValue("@pais",pais);
            
            DataTable tablaCorreos = baseDeDatos.crearTablaConsulta(comando);

            foreach (DataRow columna in tablaCorreos.Rows)
            {
                correos.Add(Convert.ToString(columna["emailPK"]));
            }
            return correos;
        }

        public int revisarSiTieneElAtributoSegunCorreo(string nombreTabla,string atributo, string columnaDeBusqueda, string correo)
        {       
            string consulta = "SELECT COUNT(*) " + "FROM " + nombreTabla + " WHERE " + columnaDeBusqueda + "=@atributo AND emailFK=@correo";
            SqlCommand comando = new SqlCommand(consulta, conexion);
            comando.Parameters.AddWithValue("@atributo", atributo);
            comando.Parameters.AddWithValue("@correo", correo);

            conexion.Open();
            Int32 estaAtributo = (Int32)comando.ExecuteScalar();
            conexion.Close();

            return (int)estaAtributo;                }
    }
}
