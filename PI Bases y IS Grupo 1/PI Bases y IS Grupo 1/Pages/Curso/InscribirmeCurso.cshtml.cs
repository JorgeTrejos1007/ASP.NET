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
        public Miembro participanteExterno { get; set; }
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
            try {
                Miembro miembroDeLaComunidad = Sesion.obtenerDatosDeSesion(HttpContext.Session);
                if (miembroDeLaComunidad != null)
                {
                    var routeValues = new { estudiante = miembroDeLaComunidad, nombreCurso = curso.nombre, esMiembro = true };
                    return base.RedirectToPage("PagarCurso", routeValues);
                }
                else
                {
                    return RedirectToPage("PagarCurso", routeValues: new { estudiante = participanteExterno, nombreCurso = curso.nombre , esMiembro = true });
                }
            }
            catch {
            }
            return RedirectToAction("~/Curso/InscribirmeCurso");
        }
    }
}