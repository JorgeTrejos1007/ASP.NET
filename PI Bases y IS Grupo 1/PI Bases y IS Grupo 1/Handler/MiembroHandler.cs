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

        public List<Miembro> obtenerTodoslosMiembros()
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
                    Nacionalidad = Convert.ToString(columna["Nacionalidad"]),
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

        
        
        
        
        
        
        
        
        /*
        
        public List<Miembro> obtenerTodoslosPlanetas()
        {
            List<Miembro> planetas = new List<Miembro>();
            string consulta = "SELECT * FROM Planeta ";
            DataTable tablaResultado = crearTablaConsulta(consulta);
            foreach (DataRow columna in tablaResultado.Rows)
            {
                planetas.Add(
                new Miembro
                {
                    nombre = Convert.ToString(columna["nombrePlaneta"]),
                    tipo = Convert.ToString(columna["tipoPlaneta"]),
                    id = Convert.ToInt32(columna["planetaId"]),
                    numeroAnillos = Convert.ToInt32(columna["numeroAnillos"])
                });
            }
            return planetas;
        }
        private byte[] obtenerBytes(HttpPostedFileBase archivo)
        {
            byte[] bytes;
            BinaryReader lector = new BinaryReader(archivo.InputStream);
            bytes = lector.ReadBytes(archivo.ContentLength);
            return bytes;
        }
        public bool crearPlaneta(Miembro planeta)
        {
            string consulta = "INSERT INTO Planeta (archivoPlaneta, tipoArchivo, nombrePlaneta, numeroAnillos, tipoPlaneta) " + "VALUES (@archivo,@tipoArchivo,@nombre,@numeroAnillos,@tipoPlaneta) ";

            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion); SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);
            comandoParaConsulta.Parameters.AddWithValue("@archivo", obtenerBytes(planeta.archivo));
            comandoParaConsulta.Parameters.AddWithValue("@tipoArchivo", planeta.archivo.ContentType);
            comandoParaConsulta.Parameters.AddWithValue("@nombre", planeta.nombre);
            comandoParaConsulta.Parameters.AddWithValue("@numeroAnillos", planeta.numeroAnillos);
            comandoParaConsulta.Parameters.AddWithValue("@tipoPlaneta", planeta.tipo);

            conexion.Open();
            bool exito = comandoParaConsulta.ExecuteNonQuery() >= 1; // indica que se agregO una tupla (cuando es mayor o igual que 1)             
            conexion.Close();
            return exito;

        }
        */
    }
}