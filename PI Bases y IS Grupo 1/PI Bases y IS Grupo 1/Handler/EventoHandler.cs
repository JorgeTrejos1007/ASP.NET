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
using PIBasesISGrupo1.Handler;

namespace PIBasesISGrupo1.Handler
{
    public class EventoHandler
    {

        private ConexionModel conexionBD;
        private SqlConnection conexion;
        private BaseDeDatosHandler baseDeDatos;

        public EventoHandler()
        {
            conexionBD = new ConexionModel();
            conexion = conexionBD.Connection();
            baseDeDatos = new BaseDeDatosHandler();
        }

        public bool registrarEvento(Evento evento, IFormFile archivoImagen)
        {
            bool exito = false;

            string consulta = "INSERT INTO Evento(emailCoordinador, nombrePK, fechaYHora, descripcion, imagen, tipoArchivoImagen) "
                + "VALUES (@emailCoordinador, @nombre, @fechaYHora, @descripcion, @archivoImagen, @tipoArchivoImagen) ";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);

            comandoParaConsulta.Parameters.AddWithValue("@emailCoordinador",evento.emailCoordinador);
            comandoParaConsulta.Parameters.AddWithValue("@nombre", evento.nombre);
            comandoParaConsulta.Parameters.AddWithValue("@fechaYHora",evento.fechaYHora);
            comandoParaConsulta.Parameters.AddWithValue("@descripcion",evento.descripcionDelEvento);
            comandoParaConsulta.Parameters.AddWithValue("@archivoImagen", obtenerBytes(archivoImagen));
            comandoParaConsulta.Parameters.AddWithValue("@tipoArchivoImagen", archivoImagen.ContentType);

            exito= baseDeDatos.ejecutarComandoParaConsulta(comandoParaConsulta);

            if (evento.tipo == "Virtual")
            {
                registrarEventoVirtual(evento);
            }
            else {
                registrarEventoPresencial(evento);
            }

            return exito;
        }

        public bool registrarEventoVirtual(Evento evento) {
            bool exito = false;
            string consulta = "INSERT INTO VIRTUAL "+"VALUES(@emailCoordinador, @nombre, @fechaYHora, @nombreCanal)";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);
            comandoParaConsulta.Parameters.AddWithValue("@emailCoordinador", evento.emailCoordinador);
            comandoParaConsulta.Parameters.AddWithValue("@nombre", evento.nombre);
            comandoParaConsulta.Parameters.AddWithValue("@fechaYHora", evento.fechaYHora);
            comandoParaConsulta.Parameters.AddWithValue("@nombreCanal", evento.nombreCanalStream);
            exito = baseDeDatos.ejecutarComandoParaConsulta(comandoParaConsulta);

            return exito;
        }

        public bool registrarEventoPresencial(Evento evento)
        {
            bool exito = false;
            string consulta = "INSERT INTO VIRTUAL " + "VALUES(@emailCoordinador, @nombre, @fechaYHora, @lugar)";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);
            comandoParaConsulta.Parameters.AddWithValue("@emailCoordinador", evento.emailCoordinador);
            comandoParaConsulta.Parameters.AddWithValue("@nombre", evento.nombre);
            comandoParaConsulta.Parameters.AddWithValue("@fechaYHora", evento.fechaYHora);
            comandoParaConsulta.Parameters.AddWithValue("@lugar", evento.lugar);
            exito = baseDeDatos.ejecutarComandoParaConsulta(comandoParaConsulta);

            bool seRegistroSector = registrarSectores(evento);

            return (exito && seRegistroSector);
        }

        public bool registrarSectores(Evento evento)
        {
            bool exito = false;
            string consulta = "INSERT INTO Sector " + "VALUES(@nombreDeSector, @emailCoordinador, @nombreDeEvento, @fechaYHora, @cantidadAsientos, @tipo)";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);

            for (int index=0; index < evento.sectores.Count; index++) {
                comandoParaConsulta.Parameters.AddWithValue("@nombreDeSector", evento.sectores[index].nombreDeSector);
                comandoParaConsulta.Parameters.AddWithValue("@emailCoordinador", evento.emailCoordinador);
                comandoParaConsulta.Parameters.AddWithValue(" @nombreDeEvento", evento.nombre);
                comandoParaConsulta.Parameters.AddWithValue("@fechaYHora", evento.fechaYHora);
                comandoParaConsulta.Parameters.AddWithValue("@cantidadAsientos", evento.sectores[index].cantidadAsientos);
                comandoParaConsulta.Parameters.AddWithValue("@tipo", evento.sectores[index].tipo);
                exito = baseDeDatos.ejecutarComandoParaConsulta(comandoParaConsulta);

                if (!exito) {
                    break;
                }
            }
            return exito;
        }

        private byte[] obtenerBytes(IFormFile archivo)
        {
            byte[] bytes;
            MemoryStream stream = new MemoryStream();
            archivo.OpenReadStream().CopyTo(stream);
            bytes = stream.ToArray();
            return bytes;
        }
    }
}
