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
    public class VistaCursoEstudianteModel : PageModel
    {
        [BindProperty]
        public List<SeccionModel> Secciones { get; set; }
        public IActionResult OnGet(String nombreCurso)
        {
            try
            {
                CursoHandler accesoDatos = new CursoHandler();
                var miembroEnSesion = Sesion.obtenerDatosDeSesion(HttpContext.Session, "Miembro");
                if (miembroEnSesion==null)
                {
                    miembroEnSesion = Sesion.obtenerDatosDeSesion(HttpContext.Session, "Estudiante");
                }
                var comprobarCurso = accesoDatos.obtenerMisCursosMatriculados(miembroEnSesion.email);
                bool nombreCursoValido = false;
                foreach (var item in (List<Tuple<string, int>>)comprobarCurso)
                {
                    if (item.Item1.Equals(nombreCurso) == true)
                    {
                        nombreCursoValido = true;
                        break;
                    }
                }
                if (nombreCursoValido)
                {
                    Secciones = accesoDatos.obtenerSecciones(nombreCurso);
                    ViewData["nombreCurso"] = nombreCurso;

                    foreach (var item in Secciones)
                    {
                        item.listaMateriales = accesoDatos.obtenerMaterialDeUnaSeccion(item.nombreSeccion, nombreCurso);
                    }
                    return Page();
                }
                else
                {
                    return RedirectToPage("MisCursosInscritos");
                }
            }
            catch
            {
                return RedirectToPage("/Index");
            }

        }
    }
}