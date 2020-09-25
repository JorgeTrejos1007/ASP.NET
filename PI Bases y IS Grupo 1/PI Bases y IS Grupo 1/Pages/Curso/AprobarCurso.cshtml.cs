using System;
using System.Collections.Generic;
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

    public class AprobarCursoModel : PageModel
    {
        CursoHandler cursoHandler = new CursoHandler();
        public void OnGet()
        {
            
            ViewData["CursosPropuestos"] = cursoHandler.obtenerCursosPropuestos();
        }
        public void onPost(string nombreCurso)
        {
            bool exito = cursoHandler.aprobarCurso(nombreCurso);
        }
    }
}