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

namespace PIBasesISGrupo1.Pages.Curso
{
    public class ProponerCursoModel : PageModel
    {
        [BindProperty]
        public Cursos miembro { get; set; }
        public IActionResult OnGet()
        {
            IActionResult vista;
            List<string> topicos;
            try
            {
                MiembroHandler accesoDatos = new MiembroHandler();
                LobtenerTopicosAsociadosACategorias
            }
                return vista;
        }
    }
}