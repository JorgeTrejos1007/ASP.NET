using System;
using System.Collections.Generic;


using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using PIBasesISGrupo1.Models;
using Microsoft.Extensions.Configuration;


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
            DataTable tablaResultado = crearTablaConsulta(consulta);
            foreach (DataRow columna in tablaResultado.Rows)
            {
                miembros.Add(
                new Miembro
                {
                    Genero = Convert.ToString(columna["Genero"]),
                    Nombre = Convert.ToString(columna["Nombre"]),
                    PrimerApellido = Convert.ToString(columna["PrimerApellido"]),
                    SegundoApellido = Convert.ToString(columna["SegundoApellido"]),
                    Email = Convert.ToString(columna["Email"]),
                    Password = Convert.ToString(columna["Password"]),
                    Pais = Convert.ToString(columna["Pais"]),
                    Hobbies = Convert.ToString(columna["Hobbies"])
                });

            }
            return miembros;
        }

        public bool crearMiembro(Miembro miembro)
        {
            string consulta = "INSERT INTO Usuario(Genero, Nombre, PrimeApellido, SegundoApellido, Email, Password, Nacionalidad, Hobbies) "
                + "VALUES (@Genero,@Nombre,@PrimeApellido,@SegundoApellido,@Email,@Password,@Nacionalidad,@Hobbies) ";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);
     
            comandoParaConsulta.Parameters.AddWithValue("@Genero", miembro.Genero);
            comandoParaConsulta.Parameters.AddWithValue("@Nombre", miembro.Nombre);
            comandoParaConsulta.Parameters.AddWithValue("@PrimeApellido", miembro.PrimerApellido);
            comandoParaConsulta.Parameters.AddWithValue("@SegundoApellido", miembro.SegundoApellido);
            comandoParaConsulta.Parameters.AddWithValue("@Email", miembro.Email);
            comandoParaConsulta.Parameters.AddWithValue("@Password", miembro.Password);
            comandoParaConsulta.Parameters.AddWithValue("@Hobbies", miembro.Hobbies);

            bool exito = comandoParaConsulta.ExecuteNonQuery() >= 1; // indica que se agregO una tupla (cuando es mayor o igual que 1)
            conexion.Close();
            return exito;
        }



        public bool modificarMiembro(Miembro miembro)
        {
            string consulta = "UPDATE Usuario SET Genero=@Genero, Nombre=@Nombre, PrimerApellido=@PrimerApellido, " +
                "SegundoApellido=@SegundoApellido, Password=@Password, Pais=@Pais, Hobbies=@Hobbies " +
                "WHERE Email=@Email";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);
            comandoParaConsulta.Parameters.AddWithValue("@Genero", miembro.Genero);
            comandoParaConsulta.Parameters.AddWithValue("@Nombre", miembro.Nombre);
            comandoParaConsulta.Parameters.AddWithValue("@PrimerApellido", miembro.PrimerApellido);
            comandoParaConsulta.Parameters.AddWithValue("@SegundoApellido", miembro.SegundoApellido);
            comandoParaConsulta.Parameters.AddWithValue("@Email", miembro.Email);
            comandoParaConsulta.Parameters.AddWithValue("@Password", miembro.Password);
            comandoParaConsulta.Parameters.AddWithValue("@Pais", miembro.Pais);
            comandoParaConsulta.Parameters.AddWithValue("@Hobbies", miembro.Hobbies);

            bool exito = comandoParaConsulta.ExecuteNonQuery() >= 1; // indica que se agregO una tupla (cuando es mayor o igual que 1)
            conexion.Close();
            return exito;
        }





       
    }
}