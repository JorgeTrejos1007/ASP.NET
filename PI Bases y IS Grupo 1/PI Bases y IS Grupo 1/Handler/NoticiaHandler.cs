﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using PIBasesISGrupo1.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

namespace PIBasesISGrupo1.Handler
{
    using System.IO;
    using System.Web;
    public class NoticiaHandler
    {
        private ConexionModel conexionBD;
        private SqlConnection conexion;
        public NoticiaHandler()
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

        public List<Noticia> obtenerTodasLasNoticias()
        {
            List<Noticia> noticias = new List<Noticia>();
            string consulta = "SELECT * FROM Noticia ORDER BY fecha";
            DataTable tablaResultado = crearTablaConsulta(consulta);
            foreach (DataRow columna in tablaResultado.Rows)
            {
                noticias.Add(
                new Noticia
                {
                    titulo = Convert.ToString(columna["titulo"]),
                    fecha = Convert.ToDateTime(columna["fecha"]),
                    arrayArchivoNoticia = (byte[])columna["archivoNoticia"],
                    tipoArchivoNoticia = Convert.ToString(columna["tipoArchivoNoticia"]),
                    arrayArchivoImagen = (byte[])columna["archivoImagen"],
                    tipoArchivoImagen = Convert.ToString(columna["tipoArchivoImagen"]),
                });

            }
            return noticias;
        }

        private byte[] obtenerBytes(IFormFile archivo)
        {
            byte[] bytes;
            MemoryStream stream = new MemoryStream();
            archivo.OpenReadStream().CopyTo(stream);
            bytes = stream.ToArray();
            return bytes;
        }

        public bool crearNoticia(Noticia noticia, IFormFile archivoNoticia, IFormFile archivoImagen)
        {
            string consulta = "INSERT INTO Noticia(titulo, fecha, archivoNoticia, tipoArchivoNoticia, archivoImagen, tipoArchivoImagen) "
                + "VALUES (@titulo, @fecha, @archivoNoticia, @tipoArchivoNoticia, @archivoImagen, @tipoArchivoImagen) ";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);

            comandoParaConsulta.Parameters.AddWithValue("@titulo", noticia.titulo);
            comandoParaConsulta.Parameters.AddWithValue("@fecha", noticia.fecha);
            comandoParaConsulta.Parameters.AddWithValue("@archivoNoticia", obtenerBytes(archivoNoticia));
            comandoParaConsulta.Parameters.AddWithValue("@tipoArchivoNoticia", archivoNoticia.ContentType);
            comandoParaConsulta.Parameters.AddWithValue("@archivoImagen", obtenerBytes(archivoImagen));
            comandoParaConsulta.Parameters.AddWithValue("@tipoArchivoImagen", archivoImagen.ContentType);

            conexion.Open();
            bool exito = comandoParaConsulta.ExecuteNonQuery() >= 1;
            conexion.Close();
            return exito;
        }
    }
}

