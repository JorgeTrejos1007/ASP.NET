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
        public void OnGet(string nombreCurso)
        {
            ViewData["nombreCurso"] = nombreCurso;
        }

    }
}