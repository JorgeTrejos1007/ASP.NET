using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PIBasesISGrupo1.Handler;
using PIBasesISGrupo1.Models;
using Microsoft.AspNetCore.Http;
namespace PIBasesISGrupo1.Pages.Curso
{
    public class CrearCursoModel : PageModel
    {
        [BindProperty]
        public List<MaterialModel> materiales { set; get; }
        [BindProperty]
        public MaterialModel material { set; get; }
        [BindProperty]
        public List<IFormFile> archivos { get; set; }
        public void OnGet( string nombreCurso)
        {
            ViewData["nombreCurso"] = nombreCurso;
            //ViewData["materiales"] = materiales;
            //ViewData["archivos"] = archivos;

        }
        public void OnPostPatito() {

        }
    }
}