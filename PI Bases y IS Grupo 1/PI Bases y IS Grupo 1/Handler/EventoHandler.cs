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
            string consulta = "INSERT INTO Evento(emailCoordinadorFK, nombreEventoPK, fechaYHoraPK, descripcion, imagen, tipoArchivoImagen) "
                + "VALUES (@emailCoordinador, @nombre, @fechaYHora, @descripcion, @archivoImagen, @tipoArchivoImagen) ";

            SqlCommand comandoParaConsulta = baseDeDatos.crearComandoParaConsulta(consulta);

            comandoParaConsulta.Parameters.AddWithValue("@emailCoordinador",evento.emailCoordinador);
            comandoParaConsulta.Parameters.AddWithValue("@nombre", evento.nombre);
            comandoParaConsulta.Parameters.AddWithValue("@fechaYHora",evento.fechaYHora);
            comandoParaConsulta.Parameters.AddWithValue("@descripcion",evento.descripcionDelEvento);
            comandoParaConsulta.Parameters.AddWithValue("@archivoImagen", obtenerBytes(archivoImagen));
            comandoParaConsulta.Parameters.AddWithValue("@tipoArchivoImagen", archivoImagen.ContentType);

            return baseDeDatos.ejecutarComandoParaConsulta(comandoParaConsulta);
        }

        public bool registrarEventoVirtual(Evento evento) {
            string consulta = "INSERT INTO Virtual "+ "VALUES(@emailCoordinador, @nombreEvento, @fechaYHora, @nombreCanal)";

            SqlCommand comandoParaConsulta = baseDeDatos.crearComandoParaConsulta(consulta);

            comandoParaConsulta.Parameters.AddWithValue("@emailCoordinador", evento.emailCoordinador);
            comandoParaConsulta.Parameters.AddWithValue("@nombreEvento", evento.nombre);
            comandoParaConsulta.Parameters.AddWithValue("@fechaYHora", evento.fechaYHora);
            comandoParaConsulta.Parameters.AddWithValue("@nombreCanal", evento.nombreCanalStream);

            return baseDeDatos.ejecutarComandoParaConsulta(comandoParaConsulta);
        }

        public bool registrarEventoPresencial(Evento evento)
        {
            string consulta = "INSERT INTO Presencial " + "VALUES(@emailCoordinador, @nombreEvento, @fechaYHora, @lugar)";

            SqlCommand comandoParaConsulta = baseDeDatos.crearComandoParaConsulta(consulta);

            comandoParaConsulta.Parameters.AddWithValue("@emailCoordinador", evento.emailCoordinador);
            comandoParaConsulta.Parameters.AddWithValue("@nombreEvento", evento.nombre);
            comandoParaConsulta.Parameters.AddWithValue("@fechaYHora", evento.fechaYHora);
            comandoParaConsulta.Parameters.AddWithValue("@lugar", evento.lugar);

            return baseDeDatos.ejecutarComandoParaConsulta(comandoParaConsulta);
        }

        public bool registrarSectores(Evento evento)
        {
            bool exito = false;
            string consulta = "INSERT INTO Sector " + "VALUES(@nombreDeSector, @emailCoordinador, @nombreDeEvento, @fechaYHora, @cantidadAsientos, @tipo)";

            for (int index = 0; index < evento.sectores.Count; index++) {
                SqlCommand comandoParaConsulta = baseDeDatos.crearComandoParaConsulta(consulta);

                comandoParaConsulta.Parameters.AddWithValue("@nombreDeSector", evento.sectores[index].nombreDeSector);
                comandoParaConsulta.Parameters.AddWithValue("@emailCoordinador", evento.emailCoordinador);
                comandoParaConsulta.Parameters.AddWithValue("@nombreDeEvento", evento.nombre);
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
