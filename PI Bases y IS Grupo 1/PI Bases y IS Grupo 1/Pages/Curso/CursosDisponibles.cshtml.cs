using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PIBasesISGrupo1.Handler;
using PIBasesISGrupo1.Models;
namespace PIBasesISGrupo1.Pages.Curso
{
    public class CursosDisponiblesModel : PageModel
    {
        private CursoHandler cursoHandler = new CursoHandler();
        public void OnGet()
        {

            ViewData["CursosDisponibles"] = cursoHandler.obtenerCursosDisponibles();

            Miembro usuarioEnSesion = Sesion.obtenerDatosDeSesion(HttpContext.Session, User.Identity.Name);
            if (usuarioEnSesion != null)
            {
                

                ViewData["cursosMatriculados"] = cursoHandler.obtenerMisCursosMatriculados(usuarioEnSesion.email);

            }

        }



        [HttpPost]
        public void OnPost(string searching)
        {
            try
            {
                Int32 precio = Int32.Parse(searching);
                ViewData["CursosDisponibles"] = cursoHandler.buscarCursos(true, searching); ;
            }
            catch {
                ViewData["CursosDisponibles"] = cursoHandler.buscarCursos(false,searching);
            }
            



        }
    }
}