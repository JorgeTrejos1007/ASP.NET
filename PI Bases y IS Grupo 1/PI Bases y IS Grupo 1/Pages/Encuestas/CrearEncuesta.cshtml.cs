using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PIBasesISGrupo1.Models;

namespace PIBasesISGrupo1.Pages.Encuestas
{
    public class CrearEncuestaModel : PageModel
    {
       [BindProperty]
        public EncuestaModel Encuesta { get; set; }
        public void OnGet()
        {

        }
    }
}