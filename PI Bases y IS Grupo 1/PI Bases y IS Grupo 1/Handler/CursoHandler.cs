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
        private MiembroHandler miembrosHandler;
        public CursoHandler()
        {
            conexionBD = new ConexionModel();
            conexion = conexionBD.Connection();
            baseDeDatos = new BaseDeDatosHandler();
            miembrosHandler = new MiembroHandler();
        }

        private byte[] obtenerBytes(IFormFile archivo)
        {
            byte[] bytes;
            MemoryStream ms = new MemoryStream();
            archivo.OpenReadStream().CopyTo(ms);
            bytes = ms.ToArray();
            return bytes;
        }

        public bool proponerCurso(Cursos curso, IFormFile archivo)
        {
            string consulta = "INSERT INTO Curso(nombrePK,emailEducadorFK,documentoInformativo,tipoDocumentoInformativo,precio)"
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
        public bool insertarRelacionConTopico(Cursos curso)
        {
            bool exito = false;
            string consultaATablaContiene = "INSERT INTO Contiene(nombreCursoFK,nombreTopicoFK)"
            + "VALUES (@nombreCurso,@nombreTopico)";
            for (int topico = 0; topico < curso.topicos.Length; ++topico)
            {
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
            string consultaCategorias = "SELECT C.nombrePK AS nombreCurso,C.estado AS estado,C.precio AS precio," +
                " C.emailEducadorFK AS emailEducador,C.tipoDocumentoInformativo AS tipoDocumento,C.documentoInformativo AS documento,E.nombre AS nombreEducador,E.primerApellido AS primerApellido,E.segundoApellido AS segundoApellido " +
            "FROM Curso C JOIN Usuario E ON C.emailEducadorFK = E.emailPK " + "WHERE estado='No aprobado'";
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
            string consulta = "SELECT C.nombrePK AS nombreCurso,C.estado AS estado,C.precio AS precio" +
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
            string consultaCategorias = "SELECT C.nombrePK AS nombreCurso,C.estado AS estado,C.precio AS precio," +
                " C.emailEducadorFK AS emailEducador,C.tipoDocumentoInformativo AS tipoDocumento,C.documentoInformativo AS documento,E.nombre AS nombreEducador,E.primerApellido AS primerApellido,E.segundoApellido AS segundoApellido " +
            "FROM Curso C JOIN Usuario E ON C.emailEducadorFK = E.emailPK " + "WHERE estado='Creado'";
            string consultaTopicos = "SELECT c.nombreCursoFK AS nombreCurso,T.nombreTopicoPK AS topico,Cat.nombreCategoriaPK AS category" +
            " FROM Curso Cu Join  Contiene C ON Cu.nombrePK = C.nombreCursoFK" +
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

        public List<Tuple<Cursos, Miembro, List<Tuple<string, string>>>> buscarCursos(bool opcionDeBusqueda, string busqueda)
        {
            List<Tuple<Cursos, Miembro, List<Tuple<string, string>>>> cursos = new List<Tuple<Cursos, Miembro, List<Tuple<string, string>>>>();
            Tuple<SqlCommand, SqlCommand> comandosParaLaBusqueda;
            if (opcionDeBusqueda == true) {
                comandosParaLaBusqueda = crearConsultaParaBusquedaDeCurso(true, busqueda);
            }
            else
            {
                comandosParaLaBusqueda = crearConsultaParaBusquedaDeCurso(false, busqueda);
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
                consultaCategorias = "SELECT C.nombrePK AS nombreCurso,C.estado AS estado,C.precio AS precio," +
                " C.emailEducadorFK AS emailEducador,C.tipoDocumentoInformativo AS tipoDocumento,C.documentoInformativo AS documento,E.nombre AS nombreEducador,E.primerApellido AS primerApellido,E.segundoApellido AS segundoApellido " +
                "FROM Curso C JOIN Usuario E ON C.emailEducadorFK = E.emailPK " + "WHERE estado='Creado' and precio like @busqueda; ";
                consultaTopicos = "SELECT c.nombreCursoFK AS nombreCurso,T.nombreTopicoPK AS topico,Cat.nombreCategoriaPK AS category" +
               " FROM Curso Cu Join  Contiene C ON Cu.nombrePK = C.nombreCursoFK" +
               " JOIN Topico T ON C.nombreTopicoFK = T.nombreTopicoPK JOIN Categoria Cat ON Cat.nombreCategoriaPK = T.nombreCategoriaFK" +
               " WHERE Cu.estado = 'Creado' and Cu.precio like @busqueda; ";

            }
            //si se va a buscar por nombre o instructor
            else
            {
                consultaCategorias = "SELECT C.nombrePK AS nombreCurso,C.estado AS estado,C.precio AS precio," +
                " C.emailEducadorFK AS emailEducador,C.tipoDocumentoInformativo AS tipoDocumento," +
               "C.documentoInformativo AS documento,E.nombre AS nombreEducador,E.primerApellido AS " +
               "primerApellido,E.segundoApellido AS segundoApellido " +
               "FROM Curso C JOIN Usuario E ON C.emailEducadorFK = E.emailPK " +
               "WHERE estado='Creado' and (C.nombrePK like @busqueda or E.nombre like @busqueda); ";
                consultaTopicos = "SELECT c.nombreCursoFK AS nombreCurso,T.nombreTopicoPK AS topico,Cat.nombreCategoriaPK AS category" +
                 " FROM Curso Cu JOIN Usuario  E ON Cu.emailEducadorFK = E.emailPK JOIN Contiene C ON Cu.nombrePK = C.nombreCursoFK" +
                 " JOIN Topico T ON C.nombreTopicoFK = T.nombreTopicoPK JOIN Categoria Cat ON Cat.nombreCategoriaPK = T.nombreCategoriaFK" +
                 " WHERE Cu.estado = 'Creado'   and (Cu.nombrePK like @busqueda or E.nombre like @busqueda); ";

            }
            SqlCommand consultaCategoriasParametrizada = baseDeDatos.crearComandoParaConsulta(consultaCategorias);
            consultaCategoriasParametrizada.Parameters.AddWithValue("@busqueda","%"+busqueda+"%");
            SqlCommand consultaTopicosParametrizada = baseDeDatos.crearComandoParaConsulta(consultaTopicos);
            consultaTopicosParametrizada.Parameters.AddWithValue("@busqueda", "%"+ busqueda+"%");



            return new Tuple<SqlCommand, SqlCommand>(consultaCategoriasParametrizada, consultaTopicosParametrizada);
        }
        public bool aprobarCurso(string nombreCurso,string emailDelQueLoPropuso)
        {

            string consulta = "UPDATE Curso " + "SET estado='Aprobado' " + "WHERE nombrePK=@nombreCurso";
            SqlCommand comandoParaConsulta = baseDeDatos.crearComandoParaConsulta(consulta);
            comandoParaConsulta.Parameters.AddWithValue("@nombreCurso", nombreCurso);
            bool exito = baseDeDatos.ejecutarComandoParaConsulta(comandoParaConsulta);
            bool exitoAlInsertarEducador = miembrosHandler.crearEducador(emailDelQueLoPropuso);
            return exito;
        }

        public bool crearSeccion(SeccionModel seccion)
        {
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
                    nombreMaterial = Convert.ToString(columna["nombreMaterialPK"]),
                    tipoArchivo = Convert.ToString(columna["tipoArchivo"]),
                });

            }
            return materiales;
        }

        public List<string> obtenerMisCursosDisponibles(string emailDelUsuario)        {            List<string> cursos = new List<string>();            string consulta = "SELECT nombreCursoFK FROM Inscribirse WHERE emailEstudianteFK=@emailDelUsuario;";            SqlCommand comandoParaConsulta = baseDeDatos.crearComandoParaConsulta(consulta);            comandoParaConsulta.Parameters.AddWithValue("@emailDelUsuario", emailDelUsuario);            cursos = baseDeDatos.obtenerDatosDeColumna(comandoParaConsulta, "nombreCursoFK");            return cursos;        }

        public bool borrarMaterial(MaterialModel material)
        {
            string consulta = "DELETE FROM Material " + "WHERE nombreCursoFK=@nombreCurso AND nombreSeccionFK=@nombreSeccion AND nombreMaterialPK=@nombreMaterial";
            SqlCommand comandoParaConsulta = baseDeDatos.crearComandoParaConsulta(consulta);
            comandoParaConsulta.Parameters.AddWithValue("@nombreCurso", material.nombreDeCurso);
            comandoParaConsulta.Parameters.AddWithValue("@nombreSeccion", material.nombreDeSeccion);
            comandoParaConsulta.Parameters.AddWithValue("@nombreMaterial", material.nombreMaterial);

            return baseDeDatos.ejecutarComandoParaConsulta(comandoParaConsulta);
        }

        public bool borrarSeccion(SeccionModel seccion)
        {
            string consulta = "DELETE FROM Seccion " + "WHERE nombreCursoFK=@nombreCurso AND nombreSeccionPK=@nombreSeccion";
            SqlCommand comandoParaConsulta = baseDeDatos.crearComandoParaConsulta(consulta);
            comandoParaConsulta.Parameters.AddWithValue("@nombreCurso", seccion.nombreCurso);
            comandoParaConsulta.Parameters.AddWithValue("@nombreSeccion", seccion.nombreSeccion);
            return baseDeDatos.ejecutarComandoParaConsulta(comandoParaConsulta);
        }

        public bool modificarSeccion(SeccionModel seccionNueva, string nombreSeccionAntiguo)
        {
            string consulta = "UPDATE Seccion SET nombreSeccionPK = @nombreSeccion " + "WHERE nombreCursoFK=@nombreCurso AND nombreSeccionPK=@nombreSeccionAntiguo";
            SqlCommand comandoParaConsulta = baseDeDatos.crearComandoParaConsulta(consulta);
            comandoParaConsulta.Parameters.AddWithValue("@nombreCurso", seccionNueva.nombreCurso);
            comandoParaConsulta.Parameters.AddWithValue("@nombreSeccion", seccionNueva.nombreSeccion);
            comandoParaConsulta.Parameters.AddWithValue("@nombreSeccionAntiguo", nombreSeccionAntiguo);
            return baseDeDatos.ejecutarComandoParaConsulta(comandoParaConsulta);
        }

        public bool crearCurso(string nombreCurso)
        {

            string consulta = "UPDATE Curso " + "SET estado='Creado' " + "WHERE nombrePK=@nombreCurso";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);
            comandoParaConsulta.Parameters.AddWithValue("@nombreCurso", nombreCurso);
            conexion.Open();
            bool exito = comandoParaConsulta.ExecuteNonQuery() >= 1;
            conexion.Close();
            return exito;
        }

        public List<Tuple<string,int>> obtenerCursosCreados(string emailEducador)
        {
            List<Tuple<string, int>> cursos;
            string consulta = "SELECT nombrePK,version FROM Curso WHERE emailEducadorFK=@emailEducador AND estado='Creado';";

            SqlCommand comandoParaConsulta = baseDeDatos.crearComandoParaConsulta(consulta);
            comandoParaConsulta.Parameters.AddWithValue("@emailEducador", emailEducador);
            DataTable tablaCursosCreados = baseDeDatos.crearTablaConsulta(comandoParaConsulta);
            cursos = new List<Tuple<string, int>>();
            foreach (DataRow columnaCursosCreados in tablaCursosCreados.Rows)
            {
                    cursos.Add(new Tuple<string, int>(Convert.ToString(columnaCursosCreados["nombrePK"]), Convert.ToInt32(columnaCursosCreados["version"])));
            
            }

            return cursos;
        }

        public Tuple<Cursos, Miembro, List<Tuple<string, string>>> obtenerInformacionCurso(string nombreCurso)
        {
            Cursos curso = new Cursos();
            Miembro educador = new Miembro();
            List<Tuple<string, string>> catalogo;
            string consultaCursos = "SELECT C.nombrePK AS nombreCurso,C.estado AS estado,C.precio AS precio, C.version AS version," +
                " C.emailEducadorFK AS emailEducador,C.tipoDocumentoInformativo AS tipoDocumento,C.documentoInformativo AS documento,E.nombre AS nombreEducador,E.primerApellido AS primerApellido,E.segundoApellido AS segundoApellido " +
            "FROM Curso C JOIN Usuario E ON C.emailEducadorFK = E.emailPK " + "WHERE estado='Creado' AND C.nombrePK = @nombreCurso";
            string consultaTopicos = "SELECT c.nombreCursoFK AS nombreCurso,T.nombreTopicoPK AS topico,Cat.nombreCategoriaPK AS category" +
            " FROM Curso Cu Join  Contiene C ON Cu.nombrePK = C.nombreCursoFK" +
            " JOIN Topico T ON C.nombreTopicoFK = T.nombreTopicoPK JOIN Categoria Cat ON Cat.nombreCategoriaPK = T.nombreCategoriaFK" +
            " WHERE Cu.estado = 'Creado' AND Cu.nombrePK = @nombreCurso; ";

            SqlCommand comandoParaConsultaCursos = new SqlCommand(consultaCursos, conexion);
            SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsultaCursos);
            comandoParaConsultaCursos.Parameters.AddWithValue("@nombreCurso", nombreCurso);

            conexion.Open();
            SqlDataReader lectorDeDatosCurso = comandoParaConsultaCursos.ExecuteReader();
            lectorDeDatosCurso.Read();
            curso.nombre = Convert.ToString(lectorDeDatosCurso["nombreCurso"]);
            curso.estado = Convert.ToString(lectorDeDatosCurso["estado"]);
            curso.version = Convert.ToInt32(lectorDeDatosCurso["version"]);
            curso.precio = Convert.ToDouble(lectorDeDatosCurso["precio"]);
            curso.emailDelEducador = Convert.ToString(lectorDeDatosCurso["emailEducador"]);
            curso.byteArrayDocument = (byte[])lectorDeDatosCurso["documento"];
            curso.tipoDocInformativo = Convert.ToString(lectorDeDatosCurso["tipoDocumento"]);
            conexion.Close();

            conexion.Open();
            SqlDataReader lectorDeDatosEducador = comandoParaConsultaCursos.ExecuteReader();
            lectorDeDatosEducador.Read();
            educador.nombre = Convert.ToString(lectorDeDatosEducador["nombreEducador"]);
            educador.primerApellido = Convert.ToString(lectorDeDatosEducador["primerApellido"]);
            educador.segundoApellido = Convert.ToString(lectorDeDatosEducador["segundoApellido"]);
            conexion.Close();

            SqlCommand comandoParaConsultaTopicos = baseDeDatos.crearComandoParaConsulta(consultaTopicos);
            comandoParaConsultaTopicos.Parameters.AddWithValue("@nombreCurso", nombreCurso);
            DataTable tableTopicos = baseDeDatos.crearTablaConsulta(comandoParaConsultaTopicos);
            catalogo = new List<Tuple<string, string>>();
            foreach (DataRow columnaTopicos in tableTopicos.Rows)
            {
                if (String.Equals(curso.nombre, Convert.ToString(columnaTopicos["nombreCurso"])))
                {
                    catalogo.Add(new Tuple<string, string>(Convert.ToString(columnaTopicos["topico"]), Convert.ToString(columnaTopicos["category"])));
                }
            }
            Tuple<Cursos, Miembro, List<Tuple<string, string>>> cursos = new Tuple<Cursos, Miembro, List<Tuple<string, string>>>(curso, educador, catalogo);
            return cursos;
        }

        public List<Tuple<string,int>> obtenerMisCursosMatriculados(string emailDelUsuario)
        {
            List<Tuple<string, int>> cursos;
            string consulta = "SELECT I.nombreCursoFK, C.version FROM Inscribirse I JOIN Curso C ON I.nombreCursoFK = C.nombrePK WHERE emailEstudianteFK=@emailDelUsuario;";

            SqlCommand comandoParaConsulta = baseDeDatos.crearComandoParaConsulta(consulta);
            comandoParaConsulta.Parameters.AddWithValue("@emailDelUsuario", emailDelUsuario);            
            DataTable tablaCursosMatriculados = baseDeDatos.crearTablaConsulta(comandoParaConsulta);
            cursos = new List<Tuple<string, int>>();
            foreach (DataRow columnaCursosMatriculados in tablaCursosMatriculados.Rows)
            {
                cursos.Add(new Tuple<string, int>(Convert.ToString(columnaCursosMatriculados["nombreCursoFK"]), Convert.ToInt32(columnaCursosMatriculados["version"])));

            }

            return cursos;
        }
        public bool actualizarInfoCurso(Cursos curso ,string antiguaNombreCurso)
        {
            string consulta = "UPDATE Curso SET nombrePK=@nuevoNombreCurso, precio=@nuevoPrecio, version=@version WHERE nombrePK=@antiguaNombreCurso";


            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);
            comandoParaConsulta.Parameters.AddWithValue("@nuevoNombreCurso", curso.nombre);
            comandoParaConsulta.Parameters.AddWithValue("@nuevoPrecio", curso.precio);
            comandoParaConsulta.Parameters.AddWithValue("@antiguaNombreCurso", antiguaNombreCurso);
            comandoParaConsulta.Parameters.AddWithValue("@version", curso.version);
            conexion.Open();
            bool exito = comandoParaConsulta.ExecuteNonQuery() >= 1;
            if (curso.topicos.Length > 0)
            {
                exito = borrarTopicos(curso.nombre);
                exito = insertarRelacionConTopico(curso);
            }
            conexion.Close();
            return exito;
        }

        public bool actualizarVersion(string nombreCurso)
        {
            string consulta = "UPDATE Curso SET version+=1 WHERE nombrePK=@nombreCurso";


            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);
            comandoParaConsulta.Parameters.AddWithValue("@nombreCurso", nombreCurso);

            conexion.Open();
            bool exito = comandoParaConsulta.ExecuteNonQuery() >= 1;
            conexion.Close();
            return exito;
        }

        public bool borrarTopicos(string nombreCurso)
        {
            bool exito = false;
            string consultaATablaContiene = "DELETE FROM Contiene WHERE nombreCursoFK=@nombreCurso ";
            
            SqlCommand comandoParaConsulta = new SqlCommand(consultaATablaContiene, conexion);
            SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);
            comandoParaConsulta.Parameters.AddWithValue("@nombreCurso", nombreCurso);
               
            exito = comandoParaConsulta.ExecuteNonQuery() >= 1;
            
            return exito;

        }

    }
}
