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
        private readonly IConfiguration configuration;


        private SqlConnection conexion;
        private string rutaConexion;
        public MiembroHandler()
        {

            rutaConexion = configuration.GetConnectionString("ConexionBD");
            conexion = new SqlConnection(rutaConexion);


        }
        /*
        private DataTable crearTablaConsulta(string consulta)
        {
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);
            DataTable consultaFormatoTabla = new DataTable();
            conexion.Open();
            adaptadorParaTabla.Fill(consultaFormatoTabla);
            conexion.Close();
            return consultaFormatoTabla;
        }
         
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