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
               exito= insertarRelacionConTopico(curso); 
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
        public List<Tuple<Cursos,Miembro>> obtenerCursosPropuestos()
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
                cursoTemporal= new Cursos
                {
                    nombre = Convert.ToString(columna["nombreCurso"]),
                    estado = Convert.ToString(columna["estado"]),
                    precio=Convert.ToDouble(columna["precio"]),
                    emailDelEducador = Convert.ToString(columna["emailEducador"]),
                    byteArrayDocument = (byte [])columna["documento"],
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

        public List<Tuple<Cursos, Miembro,List<Tuple<string,string>>>> obtenerCursosDisponibles()
        {
            List<Tuple<Cursos, Miembro, List<Tuple<string, string>>>> cursos = new List<Tuple<Cursos, Miembro, List<Tuple<string, string>>>>();
            string consultaCategorias = "SELECT C.nombre AS nombreCurso,C.estado AS estado,C.precio AS precio," +
                " C.emailEducadorFK AS emailEducador,C.tipoDocumentoInformativo AS tipoDocumento,C.documentoInformativo AS documento,E.nombre AS nombreEducador,E.primerApellido AS primerApellido,E.segundoApellido AS segundoApellido " +
            "FROM Curso C JOIN Usuario E ON C.emailEducadorFK = E.email " + "WHERE estado='Aprobado'";
            string consultaTopicos = "SELECT c.nombreCursoFK AS nombreCurso,T.nombreTopicoPK AS topico,Cat.nombreCategoriaPK AS category" +
            " FROM Curso Cu Join  Contiene C ON Cu.nombre = C.nombreCursoFK" +
            " JOIN Topico T ON C.nombreTopicoFK = T.nombreTopicoPK JOIN Categoria Cat ON Cat.nombreCategoriaPK = T.nombreCategoriaFK" +
            " WHERE Cu.estado = 'Aprobado'; ";
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
            Tuple<string, string> comandosParaLaBusqueda;
            if (opcionDeBusqueda == true) {
                comandosParaLaBusqueda = crearConsultaParaBusquedaDeCurso(true, busqueda);
            }
            else{
                comandosParaLaBusqueda= crearConsultaParaBusquedaDeCurso(false, busqueda);
            }
            DataTable tablaCurso = crearTablaConsulta(comandosParaLaBusqueda.Item1);
            DataTable tableTopicos = crearTablaConsulta(comandosParaLaBusqueda.Item2);
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

        public Tuple<string, string> crearConsultaParaBusquedaDeCurso(bool opcionDeBusqueda, string busqueda)
        {
            string consultaCategorias;
            string consultaTopicos;
            //si se va a buscar por precio
            if (opcionDeBusqueda == true)
            {
                consultaCategorias = "SELECT C.nombre AS nombreCurso,C.estado AS estado,C.precio AS precio," +
                " C.emailEducadorFK AS emailEducador,C.tipoDocumentoInformativo AS tipoDocumento,C.documentoInformativo AS documento,E.nombre AS nombreEducador,E.primerApellido AS primerApellido,E.segundoApellido AS segundoApellido " +
                "FROM Curso C JOIN Usuario E ON C.emailEducadorFK = E.email " + "WHERE estado='Aprobado' and precio = " + busqueda + ";";
                consultaTopicos = "SELECT c.nombreCursoFK AS nombreCurso,T.nombreTopicoPK AS topico,Cat.nombreCategoriaPK AS category" +
               " FROM Curso Cu Join  Contiene C ON Cu.nombre = C.nombreCursoFK" +
               " JOIN Topico T ON C.nombreTopicoFK = T.nombreTopicoPK JOIN Categoria Cat ON Cat.nombreCategoriaPK = T.nombreCategoriaFK" +
               " WHERE Cu.estado = 'Aprobado' and Cu.precio = " + busqueda + "; ";

            }
            //si se va a buscar por nombre o instructor
            else
            {
                consultaCategorias = "SELECT C.nombre AS nombreCurso,C.estado AS estado,C.precio AS precio," +
                " C.emailEducadorFK AS emailEducador,C.tipoDocumentoInformativo AS tipoDocumento," +
               "C.documentoInformativo AS documento,E.nombre AS nombreEducador,E.primerApellido AS " +
               "primerApellido,E.segundoApellido AS segundoApellido " +
               "FROM Curso C JOIN Usuario E ON C.emailEducadorFK = E.email " +
               "WHERE estado='Aprobado' and (C.nombre = '" + busqueda + "' or E.nombre = '" + busqueda + "'); ";
                consultaTopicos = "SELECT c.nombreCursoFK AS nombreCurso,T.nombreTopicoPK AS topico,Cat.nombreCategoriaPK AS category" +
                 " FROM Curso Cu JOIN Usuario  E ON Cu.emailEducadorFK = E.email JOIN Contiene C ON Cu.nombre = C.nombreCursoFK" +
                 " JOIN Topico T ON C.nombreTopicoFK = T.nombreTopicoPK JOIN Categoria Cat ON Cat.nombreCategoriaPK = T.nombreCategoriaFK" +
                 " WHERE Cu.estado = 'Aprobado'   and (Cu.nombre = '" + busqueda + "' or E.nombre = '" + busqueda + "'); ";

            }
            return new Tuple<string, string>(consultaCategorias, consultaTopicos);
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
        
        public bool crearSeccion(SeccionModel seccion){
            string consulta = "INSERT INTO Seccion(nombreSeccionPK,nombreCursoFK)"
            + "VALUES (@nombreSeccion,@nombreCurso)";
            SqlCommand comandoParaConsulta = baseDeDatos.crearComandoParaConsulta(consulta);
            comandoParaConsulta.Parameters.AddWithValue("@nombreCurso", seccion.nombreCurso);
            comandoParaConsulta.Parameters.AddWithValue("@nombreSeccion", seccion.nombreSeccion);
            return baseDeDatos.ejecutarComandoParaConsulta(comandoParaConsulta); ;          
        }
        public bool agregarMaterial(MaterialModel material, IFormFile archivo)
        {
                string consulta = "INSERT INTO Material(nombreMaterialPK,nombreSeccionFK,nombreCursoFK, material, tipoArchivo)"
                + "VALUES (@nombreMaterial,@nombreSeccion,@nombreCurso,@material,@tipoMaterial)";
                SqlCommand comandoParaConsulta = baseDeDatos.crearComandoParaConsulta(consulta);
                comandoParaConsulta.Parameters.AddWithValue("@nombreCurso", material.nombreDeCurso);
                comandoParaConsulta.Parameters.AddWithValue("@nombreSeccion", material.nombreDeSeccion);
                comandoParaConsulta.Parameters.AddWithValue("@nombreMaterial", material.nombreMaterial);
                comandoParaConsulta.Parameters.AddWithValue("@material", obtenerBytes(archivo));
                comandoParaConsulta.Parameters.AddWithValue("@tipoMaterial", archivo.ContentType);

            return baseDeDatos.ejecutarComandoParaConsulta(comandoParaConsulta);
            
        }

        public List<SeccionModel> obtenerSecciones(string nombreCurso)
        {
            List<SeccionModel> secciones = new List<SeccionModel>();
            string consulta = "SELECT * FROM Seccion WHERE nombreCursoFK = @nombreCurso";
            SqlCommand comandoParaConsulta = baseDeDatos.crearComandoParaConsulta(consulta);
            comandoParaConsulta.Parameters.AddWithValue("@nombreCurso", nombreCurso);
            DataTable tablaResultado = baseDeDatos.crearTablaConsulta(comandoParaConsulta);
            foreach (DataRow columna in tablaResultado.Rows)
            {
                secciones.Add(
                new SeccionModel
                {
                    nombreCurso = Convert.ToString(columna["nombreCursoFK"]),
                    nombreSeccion = Convert.ToString(columna["nombreSeccionPK"]),
                    
                });

            }
            return secciones;
        }
        public List<MaterialModel> obtenerMaterialDeUnaSeccion(string nombreSeccion, string nombreCurso)
        {
            List<MaterialModel> materiales = new List<MaterialModel>();
            string consulta = "SELECT * FROM Material WHERE nombreSeccionFK = @nombreSeccion AND nombreCursoFK = @nombreCurso";
            SqlCommand comandoParaConsulta = baseDeDatos.crearComandoParaConsulta(consulta);
            comandoParaConsulta.Parameters.AddWithValue("@nombreSeccion", nombreSeccion);
            comandoParaConsulta.Parameters.AddWithValue("@nombreCurso", nombreCurso);

            DataTable tablaResultado = baseDeDatos.crearTablaConsulta(comandoParaConsulta);
            foreach (DataRow columna in tablaResultado.Rows)
            {
                materiales.Add(
                new MaterialModel
                {
                    archivo = (byte[])columna["material"],
                    nombreDeCurso = Convert.ToString(columna["nombreCursoFK"]),
                    nombreDeSeccion = Convert.ToString(columna["nombreSeccionFK"]),
                    nombreMaterial =Convert.ToString(columna["nombreMaterialPK"]),
                    tipoArchivo = Convert.ToString(columna["tipoArchivo"]),
                });

            }
            return materiales;
        }

        public bool borrarMaterial(MaterialModel material )
        {

            string consulta = "DELETE FROM Material " + "WHERE nombreCursoFK=@nombreCurso AND nombreSeccionFK=@nombreSeccion AND nombreMaterialPK=@nombreMaterial";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);
            comandoParaConsulta.Parameters.AddWithValue("@nombreCurso", material.nombreDeCurso);
            comandoParaConsulta.Parameters.AddWithValue("@nombreSeccion", material.nombreDeSeccion);
            comandoParaConsulta.Parameters.AddWithValue("@nombreMaterial", material.nombreMaterial);
            conexion.Open();
            bool exito = comandoParaConsulta.ExecuteNonQuery() >= 1;
            conexion.Close();
            return exito;
        }

        public bool crearCurso(string nombreCurso)
        {

            string consulta = "UPDATE Curso " + "SET estado='Creado' " + "WHERE nombre=@nombreCurso";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);
            comandoParaConsulta.Parameters.AddWithValue("@nombreCurso", nombreCurso);
            conexion.Open();
            bool exito = comandoParaConsulta.ExecuteNonQuery() >= 1;
            conexion.Close();
            return exito;
        }

        public List<Tuple<Cursos, Miembro, List<Tuple<string, string>>>> obtenerCursosCreados()
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
    } 
}
