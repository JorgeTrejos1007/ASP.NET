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
            
            List<Miembro> miembros = new List<Miembro>();
            string consulta = "SELECT * FROM Usuario";
            DataTable tablaMiembro = crearTablaConsulta(consulta);
            
            foreach (DataRow columna in tablaMiembro.Rows)
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
                    byteArrayImage = Convert.IsDBNull(columna["archivoImagen"]) ? null : (byte[])columna["archivoImagen"],
                    idiomas = obtenerIdiomas(Convert.ToString(columna["email"])),
                    habilidades = obtenerHabilidades(Convert.ToString(columna["email"]))
                });

            }
            return miembros;
        }

        private string[] obtenerIdiomas(string email) {
            List<string> idiomas= new List<string>();
         
            string consultaIdioma = "SELECT idioma FROM Idiomas " + "WHERE email=@email";
            SqlCommand comandoParaConsulta = new SqlCommand(consultaIdioma, conexion);
            comandoParaConsulta.Parameters.AddWithValue("@email", email);
            conexion.Open();
            SqlDataReader lectorColumna = comandoParaConsulta.ExecuteReader();
            while (lectorColumna.Read())
            {

                idiomas.Add(lectorColumna["idioma"].ToString());
            }
            conexion.Close();

            return idiomas.ToArray();
        }



        private string[] obtenerHabilidades(string email)
        {
            List<string> habilidades = new List<string>();

            string consultaIdioma = "SELECT habilidad FROM Habilidades " + "WHERE email=@email";
            SqlCommand comandoParaConsulta = new SqlCommand(consultaIdioma, conexion);
            comandoParaConsulta.Parameters.AddWithValue("@email", email);
            conexion.Open();
            SqlDataReader lectorColumna = comandoParaConsulta.ExecuteReader();
            while (lectorColumna.Read())
            {

                habilidades.Add(lectorColumna["habilidad"].ToString());
            }
            conexion.Close();

            return habilidades.ToArray();
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

            if (String.IsNullOrEmpty(miembro.hobbies))
            {
                comandoParaConsulta.Parameters.AddWithValue("@hobbies", DBNull.Value);
            }
            else
            {
                comandoParaConsulta.Parameters.AddWithValue("@hobbies", miembro.hobbies);
            }

           
            conexion.Open();
            bool exito = comandoParaConsulta.ExecuteNonQuery() >= 1;
            
            if (miembro.habilidades.Length>0) {
                exito = insertatHabilidadesMiembro(miembro);
            }

            if (miembro.idiomas.Length > 0)
            {
                exito = insertatIdiomasMiembro(miembro);
            }

            conexion.Close();
            return exito;
        }

        private bool insertatHabilidadesMiembro(Miembro miembro) {
            string consultaHabilidades = "insert INTO Habilidades(email, habilidad) Values(@email,@habilidad) ";
            
            bool exito=false;
            for (int habilidad = 0; habilidad < miembro.habilidades.Length; habilidad++)
            {
                SqlCommand comandoParaConsultaHabilidades = new SqlCommand(consultaHabilidades, conexion);
                SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsultaHabilidades);
                comandoParaConsultaHabilidades.Parameters.AddWithValue("@email", miembro.email);
                comandoParaConsultaHabilidades.Parameters.AddWithValue("@habilidad", miembro.habilidades[habilidad]);
                exito = comandoParaConsultaHabilidades.ExecuteNonQuery() >= 1;
            }
            return exito;
        }

        private bool insertatIdiomasMiembro(Miembro miembro)
        {
            string consultaIdiomas = "insert INTO Idiomas(email, idioma) Values(@email,@idioma) ";
          
            bool exito = false;
            for (int idioma = 0; idioma < miembro.idiomas.Length; idioma++)
            {
                SqlCommand comandoParaConsultaIdiomas = new SqlCommand(consultaIdiomas, conexion);
                SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsultaIdiomas);
                comandoParaConsultaIdiomas.Parameters.AddWithValue("@email", miembro.email);
                comandoParaConsultaIdiomas.Parameters.AddWithValue("@idioma", miembro.idiomas[idioma]);
                exito = comandoParaConsultaIdiomas.ExecuteNonQuery() >= 1;
            }

          
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
            if (String.IsNullOrEmpty(miembro.hobbies))
            {
                comandoParaConsulta.Parameters.AddWithValue("@hobbies", DBNull.Value);
            }
            else
            {
                comandoParaConsulta.Parameters.AddWithValue("@hobbies", miembro.hobbies);
            }
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