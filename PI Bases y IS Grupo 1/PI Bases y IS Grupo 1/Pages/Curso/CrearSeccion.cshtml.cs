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
    public class CrearSeccionModel : PageModel
    {
        [BindProperty]
        public SeccionModel seccion { set; get; }

        [BindProperty]
        public List<MaterialModel> materiales { set; get; }

        [BindProperty]
        public List<IFormFile> archivos { get; set; }

        public void OnGet(string nombreCurso)
        {
            TempData["nombreCurso"] = "Algebra";
            ViewData["seccionCreada"] = false;
        }

        public void OnGetSeccionCreada()
        {
            ViewData["nombreCurso"] = TempData["nombreCurso"];
            ViewData["seccionCreada"] = TempData["seccionCreada"];
            ViewData["nombreSeccion"] = TempData["nombreSeccion"];
        }
        public IActionResult OnPostCrearSeccion()
        {
            TempData["seccionCreada"] = true;
            TempData["nombreSeccion"] = seccion.nombreSeccion;
            return RedirectToPage("CrearSeccion","SeccionCreada");
        }
    }
}