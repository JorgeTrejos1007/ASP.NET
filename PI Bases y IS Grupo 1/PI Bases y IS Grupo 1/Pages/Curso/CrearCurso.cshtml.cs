using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PIBasesISGrupo1.Handler;
using PIBasesISGrupo1.Models;
using Microsoft.AspNetCore.Http;
namespace PIBasesISGrupo1.Pages.Curso
{
    public class CrearCursoModel : PageModel
    {
        [BindProperty]
        public List<SeccionModel> Secciones { get; set; }
        public IActionResult OnGet(string nombreCurso)
        {
            try
            {
                CursoHandler accesoDatos = new CursoHandler();
                var miembroEnSesion = Sesion.obtenerDatosDeSesion(HttpContext.Session, "Miembro");
                var comprobarCurso = accesoDatos.obtenerMisCursosPropuestos(miembroEnSesion.email);
                bool nombreCursoValido = false;
                foreach (var item in comprobarCurso)
                {
                    if (item.nombre.Equals(nombreCurso) == true)
                    {
                        nombreCursoValido = true;
                        break;
                    }
                }
                if (nombreCursoValido) { 
                Secciones = accesoDatos.obtenerSecciones(nombreCurso);
                ViewData["nombreCurso"] = nombreCurso;
                foreach (var item in Secciones)
                {
                    item.listaMateriales = accesoDatos.obtenerMaterialDeUnaSeccion(item.nombreSeccion,nombreCurso);
                }
                return Page();
                }
                else
                {
                    return RedirectToPage("MisCursosPropuestos");
                }
            }
            catch
            {
                return RedirectToPage("CursosAprobados");
            }
      
        }

        public IActionResult OnPost(string nombreCurso)
        {
            CursoHandler accesoDatos = new CursoHandler();
            bool exito = accesoDatos.crearCurso(nombreCurso);

            return RedirectToPage("CursosAprobados");
        }

    }
}