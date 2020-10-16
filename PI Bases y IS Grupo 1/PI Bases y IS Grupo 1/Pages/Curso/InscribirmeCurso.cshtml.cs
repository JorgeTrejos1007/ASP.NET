using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PIBasesISGrupo1.Models;
using PIBasesISGrupo1.Handler;
using Microsoft.AspNetCore.Http;


namespace PIBasesISGrupo1.Pages.Curso
{

    public class InscribirmeCursoModel : PageModel
    {
        [BindProperty]
        public Cursos curso { get; set; }
        [BindProperty]
        public Miembro miembro { get; set; }
        public IActionResult OnGet(string nombreCurso)
        {
            IActionResult vista;
            try
            {
                vista = Page();
                ViewData["nombreCurso"] = nombreCurso;
                Miembro miembroDeLaComunidad = Sesion.obtenerDatosDeSesion(HttpContext.Session);
                if (miembroDeLaComunidad != null) {

                    ViewData["nombre"] = miembroDeLaComunidad.nombre;
                    ViewData["primerApellido"] = miembroDeLaComunidad.primerApellido;
                    ViewData["segundoApellido"] = miembroDeLaComunidad.segundoApellido;
                    ViewData["genero"] = miembroDeLaComunidad.genero;
                    ViewData["email"] = miembroDeLaComunidad.email;
                    ViewData["esMiembro"] = true;
                }
                else {
                    ViewData["esMiembro"] = false;
                }           
            }
            catch
            {
                vista = Redirect("~/Curso/InscribirmeCurso");
            }
            return vista;
        }

        public IActionResult OnPost()
        {

            try
            {
                Miembro miembroDeLaComunidad = Sesion.obtenerDatosDeSesion(HttpContext.Session);
                MiembroHandler accesoDatos = new MiembroHandler();
                if (miembroDeLaComunidad != null)
                {
                    if (accesoDatos.registrarEstudiante(miembroDeLaComunidad.email)) {
                        TempData["mensaje"] = "Se ha logrado inscribir con exito";
                        TempData["exitoAlEditar"] = true;
                        accesoDatos.inscribirEstudianteACurso(miembroDeLaComunidad.email, curso.nombre);
                    }
                    else
                    {
                        TempData["mensaje"] = "Se ha ocurrido un error en el registro";
                        TempData["exitoAlEditar"] = false;
                    }
                }
                else
                {
                    
                    if (accesoDatos.crearEstudianteComoParticipanteExterno(miembro))
                    {
                        string codigo = obtenerCodigoDeCurso();
                        TempData["mensaje"] = "Se ha logrado inscribir con exito, este es su codigo para el Curso: "+codigo;
                        TempData["exitoAlEditar"] = true;
                        accesoDatos.registrarEstudiante(miembro.email);
                        accesoDatos.inscribirEstudianteACurso(miembro.email, curso.nombre);
                    }
                    else
                    {
                        TempData["mensaje"] = "Se ha ocurrido un error en el registro";
                        TempData["exitoAlEditar"] = false;
                    }
                }
            }
            catch
            {

                TempData["mensaje"] = "Se ha ocurrido un error en el registro";
                TempData["exitoAlEditar"] = false;
            }
            return Redirect("~/Curso/InscribirmeCursos");
        }
        public string obtenerCodigoDeCurso()
        {
            string codigo = "";
            string []caracteres= {"a","b","c","d","e","f","g","h","i","j","k","l","m","n",".","_","-","?","!" };
            Random random = new Random();
            for (int caracter = 0; caracter < 4; ++caracter) {
                int caracterAleatorio = random.Next(caracteres.Length);
                codigo += caracteres[caracterAleatorio];
            }
            int randomNumber = random.Next(10000);
            codigo += randomNumber.ToString();
            return codigo;
        }
    }
}