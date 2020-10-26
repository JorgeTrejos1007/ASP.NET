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
using PIBasesISGrupo1.Filters;

namespace PIBasesISGrupo1.Pages.Curso
{
    [PermisosDeVista("Miembro de Nucleo")]
    public class AprobarCursoModel : PageModel
    {
    

        CursoHandler cursoHandler = new CursoHandler();
        public void OnGet()
        {
            
            ViewData["CursosPropuestos"] = cursoHandler.obtenerCursosPropuestos();
        }
        public IActionResult OnPost(string id,string emailDelQueLoPropuso)
        {
            bool exito = cursoHandler.aprobarCurso(id, emailDelQueLoPropuso);

            return RedirectToPage("AprobarCurso");
        }
    }
}