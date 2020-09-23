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
    public class CatalogoHandler
    {
        private ConexionModel conexionBD;
        private SqlConnection conexion;
        public CatalogoHandler()
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
        public bool crearTopico(Catalogo topico)
        {
            string consulta = " INSERT INTO Topico(nombreTopicoPK, nombreCategoriaFK) "
            + "VALUES (@nombreTopico,@nombreCategoria)";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);
            comandoParaConsulta.Parameters.AddWithValue("@nombreTopico", topico.nombre);
            comandoParaConsulta.Parameters.AddWithValue("@nombreCategoria", topico.categoria);
            if (!String.IsNullOrEmpty(miembro.nombre) && !String.IsNullOrEmpty(miembro.categoria))
            {
                comandoParaConsulta.Parameters.AddWithValue("@nombreTopico", topico.nombre);
                comandoParaConsulta.Parameters.AddWithValue("@nombreCategoria", topico.categoria);
            }

            conexion.Open();
            bool exito = comandoParaConsulta.ExecuteNonQuery() >= 1;
            conexion.Close();
            return exito;
        }

    }
}
