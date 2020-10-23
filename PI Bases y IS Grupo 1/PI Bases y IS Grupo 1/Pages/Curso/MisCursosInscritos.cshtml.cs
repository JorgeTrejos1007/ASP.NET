using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PIBasesISGrupo1.Models;
using PIBasesISGrupo1.Handler;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;
using PIBasesISGrupo1.Filters;

namespace PIBasesISGrupo1.Pages.Curso
{
    
    public class MisCursosModel : PageModel
    {
        private CursoHandler cursoHandler = new CursoHandler();

        public IActionResult OnGet()
        {
            IActionResult vista;
            Miembro usuarioEnSesion = Sesion.obtenerDatosDeSesion(HttpContext.Session, User.Identity.Name);
            if (usuarioEnSesion != null)
            {
               

                ViewData["MisCursos"] = cursoHandler.obtenerMisCursosMatriculados(usuarioEnSesion.email);




                vista = Page();
            }
            else {

                vista = Redirect("/Index");
            }

            return vista;

        }

        public IActionResult OnPost()
        {





            return Redirect("/Index");
        }
    }
}