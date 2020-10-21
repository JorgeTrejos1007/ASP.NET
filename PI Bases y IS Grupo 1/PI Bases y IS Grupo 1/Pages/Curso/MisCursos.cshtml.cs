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

namespace PIBasesISGrupo1.Pages.Curso
{
    public class MisCursosModel : PageModel
    {
        private CursoHandler accesoCursos= new CursoHandler();

        public IActionResult OnGet()
        {
            IActionResult vista;

            if (User.Identity.Name != null)
            {
               Miembro usuarioEnSesion = Sesion.obtenerDatosDeSesion(HttpContext.Session, User.Identity.Name);

                ViewData["MisCursos"] = accesoCursos.obtenerMisCursosMatriculados(usuarioEnSesion.email);




                vista = Page();
            }
            else {

                vista = Redirect("/Index");
            }

            return vista;

        }
    }
}