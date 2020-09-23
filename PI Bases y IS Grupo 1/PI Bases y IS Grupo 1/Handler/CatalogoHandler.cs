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
        public bool insertarTopico(Catalogo catalogo)
        {
            string consulta = " INSERT INTO Topico(nombreTopicoPK, nombreCategoriaFK) "
            + "VALUES (@nombreTopico,@nombreCategoria)";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);
            comandoParaConsulta.Parameters.AddWithValue("@nombreTopico", catalogo.topico);
            comandoParaConsulta.Parameters.AddWithValue("@nombreCategoria", catalogo.categoria);
            if (!String.IsNullOrEmpty(catalogo.topico) && !String.IsNullOrEmpty(catalogo.categoria))
            {
                comandoParaConsulta.Parameters.AddWithValue("@nombreTopico", catalogo.topico);
                comandoParaConsulta.Parameters.AddWithValue("@nombreCategoria", catalogo.categoria);
            }
            conexion.Open();
            bool exito = comandoParaConsulta.ExecuteNonQuery() >= 1;
            conexion.Close();
            return exito;
        }

        public bool insertarTopico(string categoria)
        {
            string consulta = " INSERT INTO Categoria(nombreCategoriaPK)"
            + "VALUES (@nombreCategoria)";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);

            comandoParaConsulta.Parameters.AddWithValue("@nombreCategoria", categoria);
            if (!string.IsNullOrEmpty(categoria))
            {
                comandoParaConsulta.Parameters.AddWithValue("@nombreCategoria", categoria);
            }
            conexion.Open();
            bool exito = comandoParaConsulta.ExecuteNonQuery() >= 1;
            conexion.Close();
            return exito;
        }

        private List<string> obtenerTopicosAsociadosACategorias(string categoria)
        {
            List<string> topicos = new List<string>();

            string consultaTopicosAsociados = "SELECT nombreTopicoPK FROM Topico WHERE nombreCategoriaFK=@categoria";
            SqlCommand comandoParaConsulta = new SqlCommand(consultaTopicosAsociados, conexion);
            comandoParaConsulta.Parameters.AddWithValue("@categoria", categoria);
            conexion.Open();
            SqlDataReader lectorColumna = comandoParaConsulta.ExecuteReader();
            while (lectorColumna.Read())
            {

                topicos.Add(lectorColumna["nombreTopicoPK"].ToString());
            }
            conexion.Close();

            return topicos;
        }


        private List<string> obtenerCategorias()
        {
            List<string> categorias = new List<string>();

            string consultaCategorias = "SELECT * FROM Categoria";
            SqlCommand comandoParaConsulta = new SqlCommand(consultaCategorias, conexion);
            
            conexion.Open();
            SqlDataReader lectorColumna = comandoParaConsulta.ExecuteReader();
            while (lectorColumna.Read())
            {

                categorias.Add(lectorColumna["nombreCategoriaPK"].ToString());
            }
            conexion.Close();

            return categorias;
        }





    }
}
