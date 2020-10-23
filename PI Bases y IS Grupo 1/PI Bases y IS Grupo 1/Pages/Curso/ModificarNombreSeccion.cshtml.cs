using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Mime;
using System.IO;
using PIBasesISGrupo1.Handler;
using PIBasesISGrupo1.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;


namespace PIBasesISGrupo1.Pages.Curso
{
    public class ModificarNombreSeccionModel : PageModel
    {
        [BindProperty]
        public SeccionModel seccion { get; set; }

        public void OnGet(string nombreCurso, string nombreSeccion)
        {
            ViewData["nombreCurso"] = nombreCurso;
            ViewData["nombreSeccion"] = nombreSeccion;
     
            TempData["nombreCurso"] = nombreCurso;
            TempData["nombreSeccion"] = nombreSeccion;
          
            if (TempData["ocurrioError"] != null)
            {
                ViewData["mensajeError"] = "Esta seccion ya esta en el curso";
            }
            else
            {
                ViewData["mensajeError"] = "";
            }
        }

        public IActionResult OnPostModificarSeccion()
        {
            IActionResult vista;
            CursoHandler accesodatos = new CursoHandler();
            if (accesodatos.modificarSeccion(seccion, (string)TempData["nombreSeccion"]))
            {
                vista = RedirectToPage("ModificarSeccion", new { nombreCurso = seccion.nombreCurso, nombreSeccion = seccion.nombreSeccion });
            }
            else
            {
                TempData["ocurrioError"] = true;
                vista = RedirectToPage("ModificarNombreSeccion");
            }
            return vista;

        }
    }
}