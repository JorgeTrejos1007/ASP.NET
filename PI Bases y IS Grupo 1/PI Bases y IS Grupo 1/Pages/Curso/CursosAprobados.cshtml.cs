﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PIBasesISGrupo1.Handler;
using PIBasesISGrupo1.Models;
namespace PIBasesISGrupo1.Pages.Curso
{
  
    public class CursosAprobadosModel : PageModel
    {
        private CursoHandler cursoHandler = new CursoHandler();
        public void OnGet()
        {

            ViewData["CursosAprobados"] = cursoHandler.obtenerCursosDisponibles();
        }
    }
    
}