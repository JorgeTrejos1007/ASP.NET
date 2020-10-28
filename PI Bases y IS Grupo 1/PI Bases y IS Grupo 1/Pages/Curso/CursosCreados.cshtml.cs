using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PIBasesISGrupo1.Handler;
using PIBasesISGrupo1.Models;
using Microsoft.AspNetCore.Http;
using PIBasesISGrupo1.Filters;

namespace PIBasesISGrupo1.Pages.Curso
{
    [PermisosDeVista("Miembro", "Miembro de Nucleo", "Educador", "Coordinador")]
    public class CursosCreadosModel : PageModel
    {
        private CursoHandler cursoHandler = new CursoHandler();
        public void OnGet()
        {
            var miembroEnSesion = Sesion.obtenerDatosDeSesion(HttpContext.Session, "Miembro");
            ViewData["CursosCreados"] = cursoHandler.obtenerCursosCreados(miembroEnSesion.email);
            TempData["cursoModificado"] = false;
        }
    }
}