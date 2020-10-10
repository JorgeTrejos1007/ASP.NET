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
            adaptadorParaTabla.Fill(consultaFormatoTabla);
            conexion.Close();
            return consultaFormatoTabla;
        }

    }
}
