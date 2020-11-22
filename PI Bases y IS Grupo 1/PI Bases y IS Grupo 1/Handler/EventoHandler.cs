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

        public bool registrarSector(Sector sector, string emailCoordinador, string nombreDeEvento, DateTime fechaYHora)
        {
            string consulta = "INSERT INTO Sector " + "VALUES(@nombreDeSector, @emailCoordinador, @nombreDeEvento, @fechaYHora, @cantidadAsientos, @tipo)";       
                SqlCommand comandoParaConsulta = baseDeDatos.crearComandoParaConsulta(consulta);

                comandoParaConsulta.Parameters.AddWithValue("@nombreDeSector", sector.nombreDeSector);
                comandoParaConsulta.Parameters.AddWithValue("@emailCoordinador", emailCoordinador);
                comandoParaConsulta.Parameters.AddWithValue("@nombreDeEvento", nombreDeEvento);
                comandoParaConsulta.Parameters.AddWithValue("@fechaYHora", fechaYHora);
                comandoParaConsulta.Parameters.AddWithValue("@cantidadAsientos", sector.cantidadAsientos);
                comandoParaConsulta.Parameters.AddWithValue("@tipo", sector.tipo);

            return baseDeDatos.ejecutarComandoParaConsulta(comandoParaConsulta);
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

        public Evento obtenerUnEventoVirtual(string emailCoordinador, string nombreEvento, DateTime fechaYHora)
        {
            Evento evento = new Evento();
            string consulta = "SELECT Evento.*, Virtual.nombreCanal FROM Evento JOIN Virtual " +
                              "ON Evento.emailCoordinadorFK = Virtual.emailCoordinadorFK AND Evento.nombreEventoPK = Virtual.nombreEventoFK AND Evento.fechaYHoraPK = Virtual.fechaYHoraFK " +
                              "WHERE Evento.emailCoordinadorFK = @email AND Evento.nombreEventoPK = @nombreEvento AND Evento.fechaYHoraPK = @fechaYHora";

            SqlCommand comandoParaConsulta = baseDeDatos.crearComandoParaConsulta(consulta);

            comandoParaConsulta.Parameters.AddWithValue("@email", emailCoordinador);
            comandoParaConsulta.Parameters.AddWithValue("@nombreEvento", nombreEvento);
            comandoParaConsulta.Parameters.AddWithValue("@fechaYHora", fechaYHora);

            DataTable consultaFormatoTabla = baseDeDatos.crearTablaConsulta(comandoParaConsulta);

            foreach (DataRow columna in consultaFormatoTabla.Rows)
            {
                evento.emailCoordinador = Convert.ToString(columna["emailCoordinadorFK"]);
                evento.nombre = Convert.ToString(columna["nombreEventoPK"]);
                evento.fechaYHora = Convert.ToDateTime(columna["fechaYHoraPK"]);
                evento.descripcionDelEvento = Convert.ToString(columna["descripcion"]);
                evento.arrayArchivoImagen = (byte[])columna["imagen"];
                evento.tipoArchivoImagen = Convert.ToString(columna["TipoArchivoImagen"]);
                evento.lugar = Convert.ToString(columna["nombreCanal"]);
                evento.tipo = "Virtual";
            }

            return evento;
        }

        public Evento obtenerUnEventoPresencial(string emailCoordinador, string nombreEvento, DateTime fechaYHora)
        {
            Evento evento = new Evento();
            string consulta = "SELECT Evento.*, Presencial.lugar FROM Evento JOIN Presencial " +
                              "ON Evento.emailCoordinadorFK = Presencial.emailCoordinadorFK AND Evento.nombreEventoPK = Presencial.nombreEventoFK AND Evento.fechaYHoraPK = Presencial.fechaYHoraFK " +
                              "WHERE Evento.emailCoordinadorFK = @email AND Evento.nombreEventoPK = @nombreEvento AND Evento.fechaYHoraPK = @fechaYHora";

            SqlCommand comandoParaConsulta = baseDeDatos.crearComandoParaConsulta(consulta);

            comandoParaConsulta.Parameters.AddWithValue("@email", emailCoordinador);
            comandoParaConsulta.Parameters.AddWithValue("@nombreEvento", nombreEvento);
            comandoParaConsulta.Parameters.AddWithValue("@fechaYHora", fechaYHora);

            DataTable consultaFormatoTabla = baseDeDatos.crearTablaConsulta(comandoParaConsulta);

            foreach (DataRow columna in consultaFormatoTabla.Rows)
            {
                evento.emailCoordinador = Convert.ToString(columna["emailCoordinadorFK"]);
                evento.nombre = Convert.ToString(columna["nombreEventoPK"]);
                evento.fechaYHora = Convert.ToDateTime(columna["fechaYHoraPK"]);
                evento.descripcionDelEvento = Convert.ToString(columna["descripcion"]);
                evento.arrayArchivoImagen = (byte[])columna["imagen"];
                evento.tipoArchivoImagen = Convert.ToString(columna["TipoArchivoImagen"]);
                evento.lugar = Convert.ToString(columna["lugar"]);
                evento.tipo = "Presencial";
            }
            return evento;
        }

        public List<Sector> obtenerSectoresEventoPresencial(string emailCoordinador, string nombreEvento, DateTime fechaYHora)
        {
            List<Sector> listaSectores = new List<Sector>();

            string consulta = "SELECT nombreDeSectorPK, tipo, numeroDeAsientos FROM Sector " +
                              "WHERE emailCoordinadorFK = @email AND nombreEventoFK = @nombreEvento AND fechaYHoraFK = @fechaYHora";

            SqlCommand comandoParaConsulta = baseDeDatos.crearComandoParaConsulta(consulta);

            comandoParaConsulta.Parameters.AddWithValue("@email", emailCoordinador);
            comandoParaConsulta.Parameters.AddWithValue("@nombreEvento", nombreEvento);
            comandoParaConsulta.Parameters.AddWithValue("@fechaYHora", fechaYHora);

            DataTable consultaFormatoTabla = baseDeDatos.crearTablaConsulta(comandoParaConsulta);

            foreach (DataRow columna in consultaFormatoTabla.Rows)
            {
                listaSectores.Add(
                new Sector
                {
                    nombreDeSector = Convert.ToString(columna["nombreDeSectorPK"]),
                    tipo = Convert.ToString(columna["tipo"]),
                    cantidadAsientos = Convert.ToInt32(columna["numeroDeAsientos"])
                });
            }

            return listaSectores;
        }

        public List<int> asientosDisponiblesEnSector(string emailCoordinador, string nombreEvento, DateTime fechaYHora, string nombreSector)
        {
            List<int> asientosDisponibles = new List<int> ();

            string consulta = "SELECT numero FROM AsientoNumerado " +
                              "WHERE estado = 'No reservado' AND nombreDeSectorFK = @nombreSector AND emailCoordinadorFK = @email " +
                              "AND nombreEventoFK = @nombreEvento AND fechaYHoraFK = @fechaYHora";

            SqlCommand comandoParaConsulta = baseDeDatos.crearComandoParaConsulta(consulta);

            comandoParaConsulta.Parameters.AddWithValue("@nombreSector", nombreSector);
            comandoParaConsulta.Parameters.AddWithValue("@email", emailCoordinador);
            comandoParaConsulta.Parameters.AddWithValue("@nombreEvento", nombreEvento);
            comandoParaConsulta.Parameters.AddWithValue("@fechaYHora", fechaYHora);

            DataTable consultaFormatoTabla = baseDeDatos.crearTablaConsulta(comandoParaConsulta);

            foreach (DataRow columna in consultaFormatoTabla.Rows)
            {
                asientosDisponibles.Add(Convert.ToInt32(columna["numero"]));
            }

            return asientosDisponibles;
        }

        public bool transaccionReservarAsientosNoNumerados(InformacionDeRegistroEnEvento cantidadAsientosDeseados)
        {
            bool exito = false;

            string consulta = "SET IMPLICIT_TRANSACTIONS ON \n" +
                              "SET TRANSACTION ISOLATION LEVEL REPEATABLE READ \n" +
                              "BEGIN TRAN \n" +
                              "UPDATE Sector SET numeroDeAsientos = numeroDeAsientos - @numeroDeAsientos WHERE nombreDeSectorPK = @nombreDeSector \n" +
                              "AND emailCoordinadorFK = @emailCoordinador AND nombreEventoFK = @nombreEvento AND fechaYHoraFK = @fechaYHora \n" +
                              "COMMIT TRAN \n" +
                              "SET IMPLICIT_TRANSACTIONS OFF \n";

            SqlCommand comando = baseDeDatos.crearComandoParaConsulta(consulta);

            comando.Parameters.AddWithValue("@numeroDeAsientos", cantidadAsientosDeseados.cantidadAsientos);
            comando.Parameters.AddWithValue("@nombreDeSector", cantidadAsientosDeseados.nombreSector);
            comando.Parameters.AddWithValue("@emailCoordinador", cantidadAsientosDeseados.emailCoordinador);
            comando.Parameters.AddWithValue("@nombreEvento", cantidadAsientosDeseados.nombreEvento);
            comando.Parameters.AddWithValue("@fechaYHora", cantidadAsientosDeseados.fechaYHora);

            try
            {
                exito = baseDeDatos.ejecutarComandoParaConsulta(comando);
            }
            catch
            {
                return exito;
            }

            return exito;
        }

        public bool transaccionReservarAsientosNumerados (InformacionDeRegistroEnEvento asientos, string emailComprador)
        {
            bool exito = false;

            string consulta = "SET IMPLICIT_TRANSACTIONS ON \n" +
                              "SET TRANSACTION ISOLATION LEVEL REPEATABLE READ \n" +
                              "BEGIN TRAN \n";      
            SqlCommand comando = baseDeDatos.crearComandoParaConsulta(consulta);

            try
            {
                exito = baseDeDatos.ejecutarComandoParaConsulta(comando);
            }
            catch
            {
                return exito;
            }


            consulta = crearConsultaDinamicaVerificarAsientosDeseadosEstanDisponibles(asientos);
            comando = baseDeDatos.crearComandoParaConsulta(consulta);

            comando.Parameters.AddWithValue("@nombreDeSector", asientos.nombreSector);
            comando.Parameters.AddWithValue("@emailCoordinador", asientos.emailCoordinador);
            comando.Parameters.AddWithValue("@nombreEvento", asientos.nombreEvento);
            comando.Parameters.AddWithValue("@fechaYHora", asientos.fechaYHora.ToString());

            string parametro = "parametro";
            int numeroDeParametro = 1;
            for (int i = 0; i < asientos.asientosDeseados.Count; i++)
            {
                comando.Parameters.AddWithValue("@" + parametro + numeroDeParametro.ToString(), asientos.asientosDeseados[i]);
                numeroDeParametro = numeroDeParametro + 1;
            }

            Int32 cantidadDeAsientosDeseadosDisponibles = 0;
            try
            {
                conexion.Open();
                cantidadDeAsientosDeseadosDisponibles = (Int32)comando.ExecuteScalar();
                conexion.Close();
            }
            catch {
                return exito;
            }

            /*if (cantidadDeAsientosDeseadosDisponibles != asientos.asientosDeseados.Count) {
                consulta = "ROLLBACK";
                comando = baseDeDatos.crearComandoParaConsulta(consulta);
                try
                {
                    exito = baseDeDatos.ejecutarComandoParaConsulta(comando);
                    exito = false;
                    return exito;
                }
                catch
                {
                    return exito;
                }
            }

            consulta = crearConsultaDinamicaParaReservarAsientosDisponibles(asientos, emailComprador);
            consulta += " COMMIT TRAN " + "SET IMPLICIT_TRANSACTIONS OFF";


            comando = baseDeDatos.crearComandoParaConsulta(consulta);

            comando.Parameters.AddWithValue("@nombreDeSector", asientos.nombreSector);
            comando.Parameters.AddWithValue("@emailCoordinador", asientos.emailCoordinador);
            comando.Parameters.AddWithValue("@nombreEvento", asientos.nombreEvento);
            comando.Parameters.AddWithValue("@fechaYHora", asientos.fechaYHora);

            parametro = "parametro";
            numeroDeParametro = 1;
            for (int i = 0; i < asientos.asientosDeseados.Count; i++)
            {
                comando.Parameters.AddWithValue("@" + parametro + numeroDeParametro.ToString(), asientos.asientosDeseados[i]);
                numeroDeParametro = numeroDeParametro + 1;
            }

            exito = false;
            try
            {
                exito = baseDeDatos.ejecutarComandoParaConsulta(comando);
            }
            catch
            {
                return exito;
            }*/

            return exito;
        }

        private string crearConsultaDinamicaParaReservarAsientosDisponibles(InformacionDeRegistroEnEvento asientos, string emailComprador)
        {
            string consulta = "UPDATE AsientoNumerado SET estado = 'Reservado', emailCompradorFK = " + emailComprador +
                              "WHERE(nombreDeSectorFK = @nombreDeSector AND emailCoordinadorFK = @emailCoordinador " +
                              "AND nombreEventoFK = @nombreEvento AND fechaYHoraFK = @fechaYHora) " +
                              "AND( ";

            string parametro = "parametro";
            int numeroDeParametro = 1;

            for (int iterador = 0; iterador < asientos.asientosDeseados.Count; iterador++)
            {
                if (iterador < asientos.asientosDeseados.Count - 1)
                {
                    consulta += "numero = @" + parametro + numeroDeParametro.ToString() + " OR ";
                    numeroDeParametro = numeroDeParametro + 1;
                }
                else
                {
                    consulta += "numero = @" + parametro + numeroDeParametro.ToString() + " )";
                }
            }

            return consulta;
        }

        private string crearConsultaDinamicaVerificarAsientosDeseadosEstanDisponibles(InformacionDeRegistroEnEvento asientos) {
            string consulta = "SELECT COUNT(numero) FROM AsientoNumerado WHERE(nombreDeSectorFK = @nombreDeSector AND emailCoordinadorFK = @emailCoordinador " +
                              "AND nombreEventoFK = @nombreEvento AND fechaYHoraFK = @fechaYHora AND estado = 'No reservado') " +
                              "AND ( ";

            string parametro = "parametro";
            int numeroDeParametro = 1;

            for (int iterador = 0; iterador < asientos.asientosDeseados.Count; iterador++) {
                if (iterador < asientos.asientosDeseados.Count-1)
                {
                    consulta += "numero = @" + parametro + numeroDeParametro.ToString() + " OR ";
                    numeroDeParametro = numeroDeParametro + 1;
                }
                else
                {
                    consulta += "numero = @" + parametro + numeroDeParametro.ToString() + " )";
                }
            }

            return consulta;
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

