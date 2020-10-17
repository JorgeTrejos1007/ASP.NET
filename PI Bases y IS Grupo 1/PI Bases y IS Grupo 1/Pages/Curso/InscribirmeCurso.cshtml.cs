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
using Newtonsoft.Json;

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

                    ViewData["miembroDeLaComunidad"] = miembroDeLaComunidad;
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

        public virtual Microsoft.AspNetCore.Mvc.RedirectToPageResult OnPost()
        {
            try {
                Miembro miembroDeLaComunidad = Sesion.obtenerDatosDeSesion(HttpContext.Session);
                if (miembroDeLaComunidad != null)
                {
                    TempData["estudiante"] = miembroDeLaComunidad;
                    var routeValues = new { estudiante = miembroDeLaComunidad, nombreCurso = curso.nombre, esMiembro = true };
                    return base.RedirectToPage("PagarCurso", routeValues);
                }
                else {
                    TempData["nombreCurso"] = curso.nombre;
                    TempData["nombre"]= participanteExterno.nombre;
                    TempData["primerApellido"] = participanteExterno.primerApellido;
                    TempData["segundoApellido"] = participanteExterno.segundoApellido;
                    TempData["email"] = participanteExterno.email;
                    TempData["genero"] = participanteExterno.genero;
                     
                }
               
            }
            catch {
            }
            return RedirectToPage("PagarCurso", routeValues: new { esMiembro = true }); ;
        }
    }
}