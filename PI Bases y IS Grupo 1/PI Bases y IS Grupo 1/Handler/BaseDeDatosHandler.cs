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

namespace PIBasesISGrupo1.Handler
{

    public class BaseDeDatosHandler
    {
        private ConexionModel conexionBD;
        private SqlConnection conexion;

        public BaseDeDatosHandler()
        {
            conexionBD = new ConexionModel();
            conexion = conexionBD.Connection();
        }

        public DataTable crearTablaConsulta(string consulta)
        {

            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);
            DataTable consultaFormatoTabla = new DataTable();
            conexion.Open();
            adaptadorParaTabla.Fill(consultaFormatoTabla);
            conexion.Close();
            return consultaFormatoTabla;
        }

        public SqlCommand crearComandoParaConsulta(string consulta)
        {
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            return comandoParaConsulta;
        }

        public List<string> obtenerDatosDeColumna(SqlCommand comandoParaConsulta,string columnaAconsultar) {
            List<string> datosDeColumna = new List<string>();
            conexion.Open();
            SqlDataReader lectorColumna = comandoParaConsulta.ExecuteReader();
            while (lectorColumna.Read())
            {
                datosDeColumna.Add(lectorColumna[columnaAconsultar].ToString());
            }
            conexion.Close();

            return datosDeColumna;
        }

        public bool ejecutarComandoParaConsulta(SqlCommand ComandoParaConsulta) {
            conexion.Open();
            bool exito = true;
            try
            {
                exito = ComandoParaConsulta.ExecuteNonQuery() >= 1;
            }
            catch {
                exito = false;
            }
            conexion.Close();
            return exito;
        }
        


    }
}
