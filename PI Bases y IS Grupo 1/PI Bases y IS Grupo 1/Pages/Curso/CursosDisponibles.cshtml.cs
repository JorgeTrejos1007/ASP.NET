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
        }
        [HttpPost]
        public void OnPost(string searching)
        {
            try
            {
                Int32 precio = Int32.Parse(searching);
                ViewData["CursosDisponibles"] = cursoHandler.buscarCursosPorPrecio(searching);
            }
            catch {
                ViewData["CursosDisponibles"] = cursoHandler.buscarCursosPorNombreOInstructor(searching);
            }
            



        }
    }
}