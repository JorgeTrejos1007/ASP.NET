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
        [BindProperty]
        public string busqueda { get; set; }

        private CursoHandler cursoHandler = new CursoHandler();
        public void OnGet()
        {

            ViewData["CursosDisponibles"] = cursoHandler.obtenerCursosDisponibles();
        }
         
        public void OnPost()
        {
            try
            {
                Int32 precio = Int32.Parse(busqueda);
                ViewData["CursosDisponibles"] = cursoHandler.obtenerCursosBuscados("C.precio =" + busqueda);
            }
            catch {
                ViewData["CursosDisponibles"] = cursoHandler.obtenerCursosBuscados("(C.nombre ="+busqueda+" OR E.nombre = "+busqueda+" )");
            }
            



        }
    }
}