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
        private BaseDeDatosHandler baseDeDatos;


        public MiembroHandler()
        {
            baseDeDatos = new BaseDeDatosHandler();
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
            string consulta = "SELECT * FROM Usuario U " +
            " WHERE EXISTS(SELECT emailMiembroFK FROM Miembro M WHERE M.emailMiembroFK= U.emailPK); ";
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
                    email = Convert.ToString(columna["emailPK"]),
                    password = Convert.ToString(columna["password"]),
                    pais = Convert.ToString(columna["pais"]),
                    hobbies = Convert.ToString(columna["hobbies"]),
                    tipoArchivo = Convert.ToString(columna["tipoArchivo"]),
                    byteArrayImage = Convert.IsDBNull(columna["archivoImagen"]) ? null : (byte[])columna["archivoImagen"],
                    idiomas = obtenerIdiomas(Convert.ToString(columna["emailPK"])),
                    habilidades = obtenerHabilidades(Convert.ToString(columna["emailPK"])),
                    
                });

            }
            return miembros;
        }

        public Miembro obtenerDatosDeUnMiembro(string email)
        {
            Miembro miembro = new Miembro();
            string consulta = "SELECT * FROM Usuario WHERE emailPK=@email";

            SqlCommand comandoParaConsulta = baseDeDatos.crearComandoParaConsulta(consulta);
            comandoParaConsulta.Parameters.AddWithValue("@email", email);
            DataTable tablaMiembro = baseDeDatos.crearTablaConsulta(comandoParaConsulta);

            foreach (DataRow columna in tablaMiembro.Rows)
            {
                miembro.genero = Convert.ToString(columna["genero"]);
                miembro.nombre = Convert.ToString(columna["nombre"]);
                miembro.primerApellido = Convert.ToString(columna["primerApellido"]);
                miembro.segundoApellido = Convert.ToString(columna["segundoApellido"]);
                miembro.email = Convert.ToString(columna["emailPK"]);
                miembro.password = Convert.ToString(columna["password"]);
                miembro.pais = Convert.ToString(columna["pais"]);
                miembro.hobbies = Convert.ToString(columna["hobbies"]);
                miembro.tipoArchivo = Convert.ToString(columna["tipoArchivo"]);
                miembro.byteArrayImage = Convert.IsDBNull(columna["archivoImagen"]) ? null : (byte[])columna["archivoImagen"];
                miembro.idiomas = obtenerIdiomas(Convert.ToString(columna["emailPK"]));
                miembro.habilidades = obtenerHabilidades(Convert.ToString(columna["emailPK"]));

            }
            return miembro;
        }
        private string[] obtenerIdiomas(string email) {
            List<string> idiomas= new List<string>();
         
            string consultaIdioma = "SELECT idiomaPK FROM Idiomas " + "WHERE emailFK=@email";
            SqlCommand comandoParaConsulta = new SqlCommand(consultaIdioma, conexion);
            comandoParaConsulta.Parameters.AddWithValue("@email", email);
            conexion.Open();
            SqlDataReader lectorColumna = comandoParaConsulta.ExecuteReader();
            while (lectorColumna.Read())
            {

                idiomas.Add(lectorColumna["idiomaPK"].ToString());
            }
            conexion.Close();

            return idiomas.ToArray();
        }
        
        private string[] obtenerHabilidades(string email)
        {
            List<string> habilidades = new List<string>();

            string consultaIdioma = "SELECT habilidadPK FROM Habilidades " + "WHERE emailFK=@email";
            SqlCommand comandoParaConsulta = new SqlCommand(consultaIdioma, conexion);
            comandoParaConsulta.Parameters.AddWithValue("@email", email);
            conexion.Open();
            SqlDataReader lectorColumna = comandoParaConsulta.ExecuteReader();
            while (lectorColumna.Read())
            {

                habilidades.Add(lectorColumna["habilidadPK"].ToString());
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
            string consulta = "INSERT INTO Usuario(genero, nombre, primerApellido, segundoApellido, emailPK, password, pais, hobbies) "
                + "VALUES (@genero,@nombre,@primerApellido,@segundoApellido,@email,@password,@pais,@hobbies) ;";
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
                exito = insertarHabilidadesMiembro(miembro);
            }

            if (miembro.idiomas.Length > 0)
            {
                exito = insertarIdiomasMiembro(miembro);
            }
            conexion.Close();
            exito = registrarEnLaTablaDeMiembros(miembro.email);
            return exito;
        }

        public bool resgistrarEstudianteALaComunidad(Miembro estudianteNoRegistrado){
            bool exitoAlregistrarme = registrarEnLaTablaDeMiembros(estudianteNoRegistrado.email);
            if (exitoAlregistrarme==true)
            {
                string consulta = "UPDATE Usuario SET password=@password, pais=@pais, hobbies=@hobbies"
                 + " WHERE emailPK=@email";
                SqlCommand comandoParaConsulta = baseDeDatos.crearComandoParaConsulta(consulta);
                comandoParaConsulta.Parameters.AddWithValue("@password", estudianteNoRegistrado.password);
                comandoParaConsulta.Parameters.AddWithValue("@pais", estudianteNoRegistrado.pais);
                comandoParaConsulta.Parameters.AddWithValue("@email", estudianteNoRegistrado.email);

                if (String.IsNullOrEmpty(estudianteNoRegistrado.hobbies))
                {
                    comandoParaConsulta.Parameters.AddWithValue("@hobbies", DBNull.Value);
                }
                else
                {
                    comandoParaConsulta.Parameters.AddWithValue("@hobbies", estudianteNoRegistrado.hobbies);
                }
                bool exito = baseDeDatos.ejecutarComandoParaConsulta(comandoParaConsulta);

                if (estudianteNoRegistrado.habilidades != null)
                {
                    exito = insertarHabilidadesMiembro(estudianteNoRegistrado);
                }

                if (estudianteNoRegistrado.idiomas != null)
                {
                    exito = insertarIdiomasMiembro(estudianteNoRegistrado);
                }

            }

            return exitoAlregistrarme;
        }

        private bool registrarEnLaTablaDeMiembros(string email) {
            SqlCommand comandoParaInsertarEnTablaDeMiembros = baseDeDatos.crearComandoParaConsulta("SP_InsertarEnMiembros");
            comandoParaInsertarEnTablaDeMiembros.CommandType = CommandType.StoredProcedure;
            comandoParaInsertarEnTablaDeMiembros.Parameters.AddWithValue("@email", email);
            bool exito = baseDeDatos.ejecutarComandoParaConsulta(comandoParaInsertarEnTablaDeMiembros);
            return exito;
        }
        public bool crearEstudianteComoParticipanteExterno(Miembro participanteExterno) {
            bool exito= false;
            string consulta = "INSERT INTO Usuario(genero, nombre, primerApellido, segundoApellido, emailPK, password) "
               + "VALUES (@genero,@nombre,@primerApellido,@segundoApellido,@email,@password)";
            SqlCommand comandoParaConsulta = baseDeDatos.crearComandoParaConsulta(consulta);
            if (String.IsNullOrEmpty(participanteExterno.segundoApellido))
            {
                comandoParaConsulta.Parameters.AddWithValue("@segundoApellido", DBNull.Value);
            }
            else
            {
                comandoParaConsulta.Parameters.AddWithValue("@segundoApellido", participanteExterno.segundoApellido);
            }
            comandoParaConsulta.Parameters.AddWithValue("@genero", participanteExterno.genero);
            comandoParaConsulta.Parameters.AddWithValue("@nombre", participanteExterno.nombre);
            comandoParaConsulta.Parameters.AddWithValue("@primerApellido", participanteExterno.primerApellido);
            comandoParaConsulta.Parameters.AddWithValue("@email", participanteExterno.email);
            comandoParaConsulta.Parameters.AddWithValue("@password", participanteExterno.password);
            exito= baseDeDatos.ejecutarComandoParaConsulta(comandoParaConsulta);
            return exito;

        }

        public bool registrarEstudiante(string correoEstudiante) {
            string consulta = "INSERT INTO Estudiante(emailEstudianteFK) "
                + "VALUES (@email) ";
            SqlCommand comandoParaConsulta = baseDeDatos.crearComandoParaConsulta(consulta);
            comandoParaConsulta.Parameters.AddWithValue("@email", correoEstudiante);
            bool exito = baseDeDatos.ejecutarComandoParaConsulta(comandoParaConsulta);

            return exito;
        }

        public bool inscribirEstudianteACurso(string emailEstudiante, string nombreDeCurso) {
            string consulta = "INSERT INTO Inscribirse(emailEstudianteFK,nombreCursoFK) "
                + "VALUES (@emailEstudiante,@nombreDeCurso)";
            SqlCommand comandoParaConsulta = baseDeDatos.crearComandoParaConsulta(consulta);
            comandoParaConsulta.Parameters.AddWithValue("@emailEstudiante", emailEstudiante);
            comandoParaConsulta.Parameters.AddWithValue("@nombreDeCurso", nombreDeCurso);
            bool exito = baseDeDatos.ejecutarComandoParaConsulta(comandoParaConsulta);
            return exito;
        }

        private bool insertarHabilidadesMiembro(Miembro miembro) {
            string consultaHabilidades = "INSERT INTO Habilidades(emailFK, habilidadPK) Values(@email,@habilidad) ";
            
            bool exito=false;
            for (int habilidad = 0; habilidad < miembro.habilidades.Length; habilidad++)
            {
                SqlCommand comandoParaConsultaHabilidades = baseDeDatos.crearComandoParaConsulta(consultaHabilidades);
                SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsultaHabilidades);
                comandoParaConsultaHabilidades.Parameters.AddWithValue("@email", miembro.email);
                comandoParaConsultaHabilidades.Parameters.AddWithValue("@habilidad", miembro.habilidades[habilidad]);
                exito =     baseDeDatos.ejecutarComandoParaConsulta(comandoParaConsultaHabilidades);
            }
            return exito;
        }

        private bool insertarIdiomasMiembro(Miembro miembro)
        {
            string consultaIdiomas = "INSERT INTO Idiomas(emailFK, idiomaPK) Values(@email,@idioma) ";
          
            bool exito = false;
            for (int idioma = 0; idioma < miembro.idiomas.Length; idioma++)
            {
                SqlCommand comandoParaConsultaIdiomas = baseDeDatos.crearComandoParaConsulta(consultaIdiomas);
                SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsultaIdiomas);
                comandoParaConsultaIdiomas.Parameters.AddWithValue("@email", miembro.email);
                comandoParaConsultaIdiomas.Parameters.AddWithValue("@idioma", miembro.idiomas[idioma]);
                exito = baseDeDatos.ejecutarComandoParaConsulta(comandoParaConsultaIdiomas );
            }
            return exito;
        }

        public bool eliminarIdiomasMiembro(string email, string[]idiomasBorrar)
        {
            string consultaIdiomas = "DELETE FROM Idiomas WHERE emailFK=@email and idioma=@idioma";

            bool exito = false;
            conexion.Open();
            for (int idioma = 0; idioma < idiomasBorrar.Length; idioma++)
            {
                SqlCommand comandoParaConsultaIdiomas = new SqlCommand(consultaIdiomas, conexion);
                SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsultaIdiomas);
                comandoParaConsultaIdiomas.Parameters.AddWithValue("@email", email);
                comandoParaConsultaIdiomas.Parameters.AddWithValue("@idioma", idiomasBorrar[idioma]);
                exito = comandoParaConsultaIdiomas.ExecuteNonQuery() >= 1;
            }
            conexion.Close();
            return exito;
        }

        public bool eliminarHabilidesMiembro(string email, string[] habilidadesBorrar )
        {
            
            string consultaHabilidades = "DELETE FROM Habilidades WHERE emailFK=@email and habilidad=@habilidad";
            conexion.Open();
            bool exito = false;
            for (int habilidad = 0; habilidad < habilidadesBorrar.Length; habilidad++)
            {
                SqlCommand comandoParaConsultaHabilidades = new SqlCommand(consultaHabilidades, conexion);
                SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsultaHabilidades);
                comandoParaConsultaHabilidades.Parameters.AddWithValue("@email", email);
                comandoParaConsultaHabilidades.Parameters.AddWithValue("@habilidad", habilidadesBorrar[habilidad]);
                exito = comandoParaConsultaHabilidades.ExecuteNonQuery() >= 1;
            }
            conexion.Close();
            return exito;
        }



        public bool modificarMiembro(Miembro miembro)
        {
            string consulta = "UPDATE Usuario SET genero=@genero, nombre=@nombre, primerApellido=@primerApellido, " +
                "segundoApellido=@segundoApellido, password=@password, pais=@pais, hobbies=@hobbies " 
                 + "WHERE emailPK=@email";
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
            bool exito = comandoParaConsulta.ExecuteNonQuery() >= 1; 

            if (miembro.habilidades!=null)
            {
                exito = insertarHabilidadesMiembro(miembro);
            }

            if (miembro.idiomas!=null)
            {
                exito = insertarIdiomasMiembro(miembro);
            }

            conexion.Close();
            return exito;
        }

        public bool actualizarImagen(string email, IFormFile archivoImagen) {
            string consulta = "UPDATE Usuario SET archivoImagen=@archivoImagen, tipoArchivo=@tipoArchivo " + "WHERE emailPK=@email";
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

        public int obtenerNumeroDeMiembros()
        {
            Int32 numeroDeMiembros = 0;
            string consulta = "SELECT COUNT(*) FROM Usuario";
            SqlCommand comando = new SqlCommand(consulta, conexion);
            conexion.Open();
            numeroDeMiembros = (Int32)comando.ExecuteScalar();
            conexion.Close();
            return (int)numeroDeMiembros;
        }
        public List<string> obtenerPaisesMiembro()
        {
            List<string> paisesMiembros = new List<string>();
            string consulta = "SELECT DISTINCT Pais FROM Usuario";
            DataTable tablaResultado = crearTablaConsulta(consulta);
            foreach (DataRow columna in tablaResultado.Rows)
            {
                paisesMiembros.Add(Convert.ToString(columna["Pais"]));
            }
            conexion.Open();
            return paisesMiembros;
            conexion.Close();
        }
         


    }
}
 