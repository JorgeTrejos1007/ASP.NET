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
                Miembro miembroDeLaComunidad = Sesion.obtenerDatosDeSesion(HttpContext.Session,"Miembro");
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

        public virtual Microsoft.AspNetCore.Mvc.RedirectToPageResult OnPost()
        {

            TempData["esMiembro"] = true;
            TempData["nombreCurso"] = curso.nombre;
            try {
                Miembro miembroDeLaComunidad = Sesion.obtenerDatosDeSesion(HttpContext.Session,"Miembro");
                if (miembroDeLaComunidad != null)
                {
                    TempData["nombre"] = miembroDeLaComunidad.nombre;
                    TempData["primerApellido"] = miembroDeLaComunidad.primerApellido;
                    TempData["segundoApellido"] = miembroDeLaComunidad.segundoApellido;
                    TempData["email"] = miembroDeLaComunidad.email;
                    TempData["genero"] = miembroDeLaComunidad.genero;
                }
                else {
                    TempData["nombre"]= participanteExterno.nombre;
                    TempData["primerApellido"] = participanteExterno.primerApellido;
                    TempData["segundoApellido"] = participanteExterno.segundoApellido;
                    TempData["email"] = participanteExterno.email;
                    TempData["genero"] = participanteExterno.genero;
                    TempData["esMiembro"]= false;

                }
               
            }
            catch {
            }
            return RedirectToPage("PagarCurso");  
        }
    }
}