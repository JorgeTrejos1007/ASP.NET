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
    public class CursoHandler
    {
        private ConexionModel conexionBD;
        private SqlConnection conexion;
        private BaseDeDatosHandler baseDeDatos;
        public CursoHandler()
        {
            conexionBD = new ConexionModel();
            conexion = conexionBD.Connection();
            baseDeDatos = new BaseDeDatosHandler();
        }

        private byte[] obtenerBytes(IFormFile archivo)
        {
            byte[] bytes;
            MemoryStream ms = new MemoryStream();
            archivo.OpenReadStream().CopyTo(ms);
            bytes = ms.ToArray();
            return bytes;
        }

        public bool proponerCurso(Cursos curso, IFormFile archivo) {
            string consulta = "INSERT INTO Curso(nombre,emailEducadorFK,documentoInformativo,tipoDocumentoInformativo,precio)"
          + "VALUES (@nombreCurso,@emailEducador,@documentoInformativo,@tipoDocumentoInformativo,@precio)";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);
            comandoParaConsulta.Parameters.AddWithValue("@nombreCurso", curso.nombre);
            comandoParaConsulta.Parameters.AddWithValue("@emailEducador", curso.emailDelQueLoPropone);
            comandoParaConsulta.Parameters.AddWithValue("@documentoInformativo", obtenerBytes(archivo));
            comandoParaConsulta.Parameters.AddWithValue("@tipoDocumentoInformativo", archivo.ContentType);
            comandoParaConsulta.Parameters.AddWithValue("@precio", curso.precio);
            conexion.Open();
            bool exito = comandoParaConsulta.ExecuteNonQuery() >= 1;
            if (curso.topicos.Length > 0)
            {
                exito = insertarRelacionConTopico(curso);
            }
            conexion.Close();
            return exito;
        }
        public bool insertarRelacionConTopico(Cursos curso) {
            bool exito = false;
            string consultaATablaContiene = "INSERT INTO Contiene(nombreCursoFK,nombreTopicoFK)"
            + "VALUES (@nombreCurso,@nombreTopico)";
            for (int topico = 0; topico < curso.topicos.Length; ++topico) {
                SqlCommand comandoParaConsulta = new SqlCommand(consultaATablaContiene, conexion);
                SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);
                comandoParaConsulta.Parameters.AddWithValue("@nombreCurso", curso.nombre);
                comandoParaConsulta.Parameters.AddWithValue("@nombreTopico", curso.topicos[topico]);
                exito = comandoParaConsulta.ExecuteNonQuery() >= 1;
            }
            return exito;

        }
        public List<Tuple<Cursos, Miembro>> obtenerCursosPropuestos()
        {
            List<Tuple<Cursos, Miembro>> cursos = new List<Tuple<Cursos, Miembro>>();
            string consultaCategorias = "SELECT C.nombre AS nombreCurso,C.estado AS estado,C.precio AS precio," +
                " C.emailEducadorFK AS emailEducador,C.tipoDocumentoInformativo AS tipoDocumento,C.documentoInformativo AS documento,E.nombre AS nombreEducador,E.primerApellido AS primerApellido,E.segundoApellido AS segundoApellido " +
            "FROM Curso C JOIN Usuario E ON C.emailEducadorFK = E.email " + "WHERE estado='No aprobado'";
            DataTable tablaCurso = crearTablaConsulta(consultaCategorias);
            Cursos cursoTemporal;
            Miembro educadorTemporal;
            foreach (DataRow columna in tablaCurso.Rows)
            {
                cursoTemporal = new Cursos
                {
                    nombre = Convert.ToString(columna["nombreCurso"]),
                    estado = Convert.ToString(columna["estado"]),
                    precio = Convert.ToDouble(columna["precio"]),
                    emailDelEducador = Convert.ToString(columna["emailEducador"]),
                    byteArrayDocument = (byte[])columna["documento"],
                    tipoDocInformativo = Convert.ToString(columna["tipoDocumento"])
                };
                educadorTemporal = new Miembro
                {
                    nombre = Convert.ToString(columna["nombreEducador"]),
                    primerApellido = Convert.ToString(columna["primerApellido"]),
                    segundoApellido = Convert.ToString(columna["segundoApellido"])

                };
                cursos.Add(new Tuple<Cursos, Miembro>(cursoTemporal, educadorTemporal));


            }
            return cursos;
        }

        public List<Cursos>obtenerMisCursosPropuestos(string correoMiembro){
            List<Cursos> misCursosPropuestos = new List<Cursos>();
            string consulta = "SELECT C.nombre AS nombreCurso,C.estado AS estado,C.precio AS precio" +
            " FROM Curso C " + "WHERE emailEducadorFK=@correoMiembro AND estado!='Creado'";

            SqlCommand ComandoParaConsulta = baseDeDatos.crearComandoParaConsulta(consulta);
            ComandoParaConsulta.Parameters.AddWithValue("@correoMiembro", correoMiembro);
            DataTable tablaCurso = baseDeDatos.crearTablaConsulta(ComandoParaConsulta);

            Cursos cursoTemporal;
            foreach (DataRow columna in tablaCurso.Rows)
            {
                cursoTemporal = new Cursos
                {
                    nombre = Convert.ToString(columna["nombreCurso"]),
                    estado = Convert.ToString(columna["estado"]),
                    precio = Convert.ToDouble(columna["precio"])
                  
                };
                misCursosPropuestos.Add(cursoTemporal);
            }
            return misCursosPropuestos;
        }

        public List<Tuple<Cursos, Miembro,List<Tuple<string,string>>>> obtenerCursosDisponibles()
        {
            List<Tuple<Cursos, Miembro, List<Tuple<string, string>>>> cursos = new List<Tuple<Cursos, Miembro, List<Tuple<string, string>>>>();
            string consultaCategorias = "SELECT C.nombre AS nombreCurso,C.estado AS estado,C.precio AS precio," +
                " C.emailEducadorFK AS emailEducador,C.tipoDocumentoInformativo AS tipoDocumento,C.documentoInformativo AS documento,E.nombre AS nombreEducador,E.primerApellido AS primerApellido,E.segundoApellido AS segundoApellido " +
            "FROM Curso C JOIN Usuario E ON C.emailEducadorFK = E.email " + "WHERE estado='Creado'";
            string consultaTopicos = "SELECT c.nombreCursoFK AS nombreCurso,T.nombreTopicoPK AS topico,Cat.nombreCategoriaPK AS category" +
            " FROM Curso Cu Join  Contiene C ON Cu.nombre = C.nombreCursoFK" +
            " JOIN Topico T ON C.nombreTopicoFK = T.nombreTopicoPK JOIN Categoria Cat ON Cat.nombreCategoriaPK = T.nombreCategoriaFK" +
            " WHERE Cu.estado = 'Creado'; ";
            DataTable tablaCurso = crearTablaConsulta(consultaCategorias);
            DataTable tableTopicos = crearTablaConsulta(consultaTopicos);
            Cursos cursoTemporal;
            Miembro educadorTemporal;
            List<Tuple<string, string>> catalogo;
            foreach (DataRow columna in tablaCurso.Rows)
            {
                cursoTemporal = new Cursos
                {
                    nombre = Convert.ToString(columna["nombreCurso"]),
                    estado = Convert.ToString(columna["estado"]),
                    precio = Convert.ToDouble(columna["precio"]),
                    emailDelEducador = Convert.ToString(columna["emailEducador"]),
                    byteArrayDocument = (byte[])columna["documento"],
                    tipoDocInformativo = Convert.ToString(columna["tipoDocumento"])
                };
                catalogo = new List <Tuple<string, string>>();
                foreach (DataRow columnaTopicos in tableTopicos.Rows)
                {
                    if (String.Equals(cursoTemporal.nombre, Convert.ToString(columnaTopicos["nombreCurso"]))) {
                        catalogo.Add(new Tuple <string, string>(Convert.ToString(columnaTopicos["topico"]), Convert.ToString(columnaTopicos["category"])));
                    }

                }
                    educadorTemporal = new Miembro
                {
                    nombre = Convert.ToString(columna["nombreEducador"]),
                    primerApellido = Convert.ToString(columna["primerApellido"]),
                    segundoApellido = Convert.ToString(columna["segundoApellido"])

                };
               
                cursos.Add(new Tuple<Cursos, Miembro, List<Tuple<string, string>>>(cursoTemporal, educadorTemporal,catalogo));


            }
            return cursos;
        }
       
        public List<Tuple<Cursos, Miembro, List<Tuple<string, string>>>> buscarCursos(bool opcionDeBusqueda,string busqueda)
        {
            List<Tuple<Cursos, Miembro, List<Tuple<string, string>>>> cursos = new List<Tuple<Cursos, Miembro, List<Tuple<string, string>>>>();
            Tuple<SqlCommand, SqlCommand> comandosParaLaBusqueda;
            if (opcionDeBusqueda == true) {
                comandosParaLaBusqueda = crearConsultaParaBusquedaDeCurso(true, busqueda);
            }
            else{
                comandosParaLaBusqueda= crearConsultaParaBusquedaDeCurso(false, busqueda);
            }

            DataTable tablaCurso = baseDeDatos.crearTablaConsulta(comandosParaLaBusqueda.Item1);
            DataTable tableTopicos = baseDeDatos.crearTablaConsulta(comandosParaLaBusqueda.Item2);
            Cursos cursoTemporal;
            Miembro educadorTemporal;
            List<Tuple<string, string>> catalogo;
            foreach (DataRow columna in tablaCurso.Rows)
            {
                cursoTemporal = new Cursos
                {
                    nombre = Convert.ToString(columna["nombreCurso"]),
                    estado = Convert.ToString(columna["estado"]),
                    precio = Convert.ToDouble(columna["precio"]),
                    emailDelEducador = Convert.ToString(columna["emailEducador"]),
                    byteArrayDocument = (byte[])columna["documento"],
                    tipoDocInformativo = Convert.ToString(columna["tipoDocumento"])
                };
                catalogo = new List<Tuple<string, string>>();
                foreach (DataRow columnaTopicos in tableTopicos.Rows)
                {
                    if (String.Equals(cursoTemporal.nombre, Convert.ToString(columnaTopicos["nombreCurso"])))
                    {
                        catalogo.Add(new Tuple<string, string>(Convert.ToString(columnaTopicos["topico"]), Convert.ToString(columnaTopicos["category"])));
                    }

                }
                educadorTemporal = new Miembro
                {
                    nombre = Convert.ToString(columna["nombreEducador"]),
                    primerApellido = Convert.ToString(columna["primerApellido"]),
                    segundoApellido = Convert.ToString(columna["segundoApellido"])

                };

                cursos.Add(new Tuple<Cursos, Miembro, List<Tuple<string, string>>>(cursoTemporal, educadorTemporal, catalogo));


            }
            return cursos;
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

        public Tuple<SqlCommand, SqlCommand> crearConsultaParaBusquedaDeCurso(bool opcionDeBusqueda, string busqueda)
        {
            string consultaCategorias;
            string consultaTopicos;
            //si se va a buscar por precio
            if (opcionDeBusqueda == true)
            {
                consultaCategorias = "SELECT C.nombre AS nombreCurso,C.estado AS estado,C.precio AS precio," +
                " C.emailEducadorFK AS emailEducador,C.tipoDocumentoInformativo AS tipoDocumento,C.documentoInformativo AS documento,E.nombre AS nombreEducador,E.primerApellido AS primerApellido,E.segundoApellido AS segundoApellido " +
                "FROM Curso C JOIN Usuario E ON C.emailEducadorFK = E.email " + "WHERE estado='Creado' and precio like @busqueda; ";
                consultaTopicos = "SELECT c.nombreCursoFK AS nombreCurso,T.nombreTopicoPK AS topico,Cat.nombreCategoriaPK AS category" +
               " FROM Curso Cu Join  Contiene C ON Cu.nombre = C.nombreCursoFK" +
               " JOIN Topico T ON C.nombreTopicoFK = T.nombreTopicoPK JOIN Categoria Cat ON Cat.nombreCategoriaPK = T.nombreCategoriaFK" +
               " WHERE Cu.estado = 'Creado' and Cu.precio like @busqueda; ";

            }
            //si se va a buscar por nombre o instructor
            else
            {
                consultaCategorias = "SELECT C.nombre AS nombreCurso,C.estado AS estado,C.precio AS precio," +
                " C.emailEducadorFK AS emailEducador,C.tipoDocumentoInformativo AS tipoDocumento," +
               "C.documentoInformativo AS documento,E.nombre AS nombreEducador,E.primerApellido AS " +
               "primerApellido,E.segundoApellido AS segundoApellido " +
               "FROM Curso C JOIN Usuario E ON C.emailEducadorFK = E.email " +
               "WHERE estado='Creado' and (C.nombre like @busqueda or E.nombre like @busqueda); ";
                consultaTopicos = "SELECT c.nombreCursoFK AS nombreCurso,T.nombreTopicoPK AS topico,Cat.nombreCategoriaPK AS category" +
                 " FROM Curso Cu JOIN Usuario  E ON Cu.emailEducadorFK = E.email JOIN Contiene C ON Cu.nombre = C.nombreCursoFK" +
                 " JOIN Topico T ON C.nombreTopicoFK = T.nombreTopicoPK JOIN Categoria Cat ON Cat.nombreCategoriaPK = T.nombreCategoriaFK" +
                 " WHERE Cu.estado = 'Creado'   and (Cu.nombre like @busqueda or E.nombre like @busqueda); ";

            }
            SqlCommand consultaCategoriasParametrizada = baseDeDatos.crearComandoParaConsulta(consultaCategorias);
            consultaCategoriasParametrizada.Parameters.AddWithValue("@busqueda","%"+busqueda+"%");
            SqlCommand consultaTopicosParametrizada = baseDeDatos.crearComandoParaConsulta(consultaTopicos);
            consultaTopicosParametrizada.Parameters.AddWithValue("@busqueda", "%"+ busqueda+"%");



            return new Tuple<SqlCommand, SqlCommand>(consultaCategoriasParametrizada, consultaTopicosParametrizada);
        }
        public bool aprobarCurso(string nombreCurso)
        {
            
            string consulta = "UPDATE Curso " + "SET estado='Aprobado' " + "WHERE nombre=@nombreCurso";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);
            comandoParaConsulta.Parameters.AddWithValue("@nombreCurso", nombreCurso);
            conexion.Open();
            bool exito = comandoParaConsulta.ExecuteNonQuery() >= 1;
            conexion.Close();
            return exito;
        }

        public List<Tuple<string, Miembro>> obtenerMisCursosMatriculados(string emailDelUsuario)
        {
            List<Tuple<string, Miembro>>cursos = new List<Tuple<string, Miembro>>();
            
            string consulta = "SELECT I.nombreCursoFK,E.tipoArchivo AS tipo,E.archivoImagen AS imagen " +
            ", E.nombre AS nombre FROM Inscribirse I JOIN Curso C ON I.nombreCursoFK = C.nombre JOIN Usuario E " +
            "ON C.emailEducadorFK = E.email"
            +" WHERE I.emailEstudianteFK =@emailDelUsuario;";
            SqlCommand comandoParaConsulta = baseDeDatos.crearComandoParaConsulta(consulta);
            comandoParaConsulta.Parameters.AddWithValue("@emailDelUsuario", emailDelUsuario);
            DataTable tablaCursoInscrito = baseDeDatos.crearTablaConsulta(comandoParaConsulta);
            Miembro educador;
            foreach (DataRow columna in tablaCursoInscrito.Rows)
            {
                string nombreDelCurso = Convert.ToString(columna["nombreCursoFK"]);
                educador = new Miembro { nombre = Convert.ToString(columna["nombre"]),
                   tipoArchivo= Convert.ToString(columna["tipo"]),
                   byteArrayImage= (byte[])columna["imagen"]
                };
                cursos.Add(new Tuple<string, Miembro>(nombreDelCurso,educador));
            }


           return cursos;

        }



    }
}
