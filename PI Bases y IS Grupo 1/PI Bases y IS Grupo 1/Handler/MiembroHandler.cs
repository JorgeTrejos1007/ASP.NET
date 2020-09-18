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
    public class MiembroHandler
    {
        private ConexionModel conexionBD;
        private SqlConnection conexion;
        public MiembroHandler()
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

        public List<Miembro> obtenerTodosLosMiembros()
        {
            byte[] byteArray;
            List<Miembro> miembros = new List<Miembro>();
            string consulta = "SELECT * FROM Usuario";
            DataTable tablaResultado = crearTablaConsulta(consulta);
            
            foreach (DataRow columna in tablaResultado.Rows)
            {
               
                miembros.Add(
                new Miembro
                {
                    genero = Convert.ToString(columna["genero"]),
                    nombre = Convert.ToString(columna["nombre"]),
                    primerApellido = Convert.ToString(columna["primerApellido"]),
                    segundoApellido = Convert.ToString(columna["segundoApellido"]),
                    email = Convert.ToString(columna["email"]),
                    password = Convert.ToString(columna["password"]),
                    pais = Convert.ToString(columna["pais"]),
                    hobbies = Convert.ToString(columna["hobbies"]),
                    tipoArchivo = Convert.ToString(columna["tipoArchivo"]),
                    byteArrayImage = Convert.IsDBNull(columna["archivoImagen"]) ? null : (byte[])columna["archivoImagen"]

                });

            }
            return miembros;
        }

        private byte[] obtenerBytes(IFormFile archivo)
        {
            byte[] bytes;
            MemoryStream ms = new MemoryStream();
            archivo.OpenReadStream().CopyTo(ms);
            bytes = ms.ToArray();
            return bytes;
        }

        public bool crearMiembro(Miembro miembro)
        {
            string consulta = "INSERT INTO Usuario(genero, nombre, primerApellido, segundoApellido, email, password, pais, hobbies) "
                + "VALUES (@genero,@nombre,@primerApellido,@segundoApellido,@email,@password,@pais,@hobbies) ";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);
     
            comandoParaConsulta.Parameters.AddWithValue("@genero", miembro.genero);
            comandoParaConsulta.Parameters.AddWithValue("@nombre", miembro.nombre);
            comandoParaConsulta.Parameters.AddWithValue("@primerApellido", miembro.primerApellido);
            if (String.IsNullOrEmpty(miembro.segundoApellido))
            {
                comandoParaConsulta.Parameters.AddWithValue("@segundoApellido", DBNull.Value);
            }
            else
            {
                comandoParaConsulta.Parameters.AddWithValue("@segundoApellido", miembro.segundoApellido);
            }
            comandoParaConsulta.Parameters.AddWithValue("@email", miembro.email);
            comandoParaConsulta.Parameters.AddWithValue("@password", miembro.password);
            comandoParaConsulta.Parameters.AddWithValue("@pais", miembro.pais);
            comandoParaConsulta.Parameters.AddWithValue("@hobbies", miembro.hobbies);
            conexion.Open();
            bool exito = comandoParaConsulta.ExecuteNonQuery() >= 1; 
            conexion.Close();
            return exito;
        }



        public bool modificarMiembro(Miembro miembro)
        {
            string consulta = "UPDATE Usuario SET genero=@genero, nombre=@nombre, primerApellido=@primerApellido, " +
                "segundoApellido=@segundoApellido, password=@password, pais=@pais, hobbies=@hobbies " 
                 + "WHERE email=@email";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);
            comandoParaConsulta.Parameters.AddWithValue("@genero", miembro.genero);
            comandoParaConsulta.Parameters.AddWithValue("@nombre", miembro.nombre);
            comandoParaConsulta.Parameters.AddWithValue("@primerApellido", miembro.primerApellido);
            if (String.IsNullOrEmpty(miembro.segundoApellido))
            {
                comandoParaConsulta.Parameters.AddWithValue("@segundoApellido", DBNull.Value);
            }
            else {
                comandoParaConsulta.Parameters.AddWithValue("@segundoApellido", miembro.segundoApellido);
            }
            comandoParaConsulta.Parameters.AddWithValue("@email", miembro.email);
            comandoParaConsulta.Parameters.AddWithValue("@password", miembro.password);
            comandoParaConsulta.Parameters.AddWithValue("@pais", miembro.pais);
            comandoParaConsulta.Parameters.AddWithValue("@hobbies", miembro.hobbies);
            conexion.Open();
            bool exito = comandoParaConsulta.ExecuteNonQuery() >= 1; // indica que se agregO una tupla (cuando es mayor o igual que 1)
            conexion.Close();
            return exito;
        }

        public bool actualizarImagen(string email, IFormFile archivoImagen) {
            string consulta = "UPDATE Usuario SET archivoImagen=@archivoImagen, tipoArchivo=@tipoArchivo " + "WHERE email=@email";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);
            comandoParaConsulta.Parameters.AddWithValue("@archivoImagen", obtenerBytes(archivoImagen));
            comandoParaConsulta.Parameters.AddWithValue("@tipoArchivo", archivoImagen.ContentType);
            comandoParaConsulta.Parameters.AddWithValue("@email", email);
            conexion.Open();
            bool exito = comandoParaConsulta.ExecuteNonQuery() >= 1; // indica que se agregO una tupla (cuando es mayor o igual que 1)
            conexion.Close();
            return exito;
        }




        


    }
}