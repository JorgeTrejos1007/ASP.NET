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

            comandoParaConsulta.Parameters.AddWithValue("@emailCoordinador", evento.emailCoordinador);
            comandoParaConsulta.Parameters.AddWithValue("@nombre", evento.nombre);
            comandoParaConsulta.Parameters.AddWithValue("@fechaYHora", evento.fechaYHora);
            comandoParaConsulta.Parameters.AddWithValue("@descripcion", evento.descripcionDelEvento);
            comandoParaConsulta.Parameters.AddWithValue("@archivoImagen", obtenerBytes(archivoImagen));
            comandoParaConsulta.Parameters.AddWithValue("@tipoArchivoImagen", archivoImagen.ContentType);

            return baseDeDatos.ejecutarComandoParaConsulta(comandoParaConsulta);
        }

        public bool registrarEventoVirtual(Evento evento) {
            string consulta = "INSERT INTO Virtual " + "VALUES(@emailCoordinador, @nombreEvento, @fechaYHora, @nombreCanal)";

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

        public List<Evento> obtenerTodosLosEventosVirtuales()
        {
            List<Evento> eventos = new List<Evento>();
            string consulta = "SELECT Evento.*, Virtual.nombreCanal FROM Evento JOIN Virtual " +
                              "ON Evento.emailCoordinadorFK = Virtual.emailCoordinadorFK AND Evento.nombreEventoPK = Virtual.nombreEventoFK AND Evento.fechaYHoraPK = Virtual.fechaYHoraFK " +
                              "WHERE fechaYHoraPK > GETDATE()";

            SqlCommand comandoParaConsulta = baseDeDatos.crearComandoParaConsulta(consulta);
            DataTable consultaFormatoTabla = baseDeDatos.crearTablaConsulta(comandoParaConsulta);

            foreach (DataRow columna in consultaFormatoTabla.Rows)
            {
                eventos.Add(
                new Evento
                {
                    emailCoordinador = Convert.ToString(columna["emailCoordinadorFK"]),
                    nombre = Convert.ToString(columna["nombreEventoPK"]),
                    fechaYHora = Convert.ToDateTime(columna["fechaYHoraPK"]),
                    descripcionDelEvento = Convert.ToString(columna["descripcion"]),
                    arrayArchivoImagen = (byte[])columna["imagen"],
                    tipoArchivoImagen = Convert.ToString(columna["TipoArchivoImagen"]),
                    nombreCanalStream = Convert.ToString(columna["nombreCanal"]),
                    tipo = "Virtual"
                });
            }

            return eventos;
        }

        public List<Evento> obtenerTodosLosEventosPresenciales()
        {
            List<Evento> eventos = new List<Evento>();
            string consulta = "SELECT Evento.*, Presencial.lugar FROM Evento JOIN Presencial " +
                              "ON Evento.emailCoordinadorFK = Presencial.emailCoordinadorFK AND Evento.nombreEventoPK = Presencial.nombreEventoFK AND Evento.fechaYHoraPK = Presencial.fechaYHoraFK " +
                              "WHERE fechaYHoraPK > GETDATE()";

            SqlCommand comandoParaConsulta = baseDeDatos.crearComandoParaConsulta(consulta);
            DataTable consultaFormatoTabla = baseDeDatos.crearTablaConsulta(comandoParaConsulta);

            foreach (DataRow columna in consultaFormatoTabla.Rows)
            {
                eventos.Add(
                new Evento
                {
                    emailCoordinador = Convert.ToString(columna["emailCoordinadorFK"]),
                    nombre = Convert.ToString(columna["nombreEventoPK"]),
                    fechaYHora = Convert.ToDateTime(columna["fechaYHoraPK"]),
                    descripcionDelEvento = Convert.ToString(columna["descripcion"]),
                    arrayArchivoImagen = (byte[])columna["imagen"],
                    tipoArchivoImagen = Convert.ToString(columna["TipoArchivoImagen"]),
                    lugar = Convert.ToString(columna["lugar"]),
                    tipo = "Presencial"
                });
            }

            return eventos;
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

