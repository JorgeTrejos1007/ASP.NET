using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PIBasesISGrupo1.Handler;
using PIBasesISGrupo1.Models;


namespace PIBasesISGrupo1.Pages.Encuestas
{
    public class IndexPreguntasModel : PageModel
    {
        [BindProperty]
        public List<PreguntaModel> preguntas { get; set; }

        public void OnGet(int id)
        {
            ViewData["id"]= id;
            PreguntasHandler accesoDatos = new PreguntasHandler();
            preguntas = accesoDatos.obtenerPreguntas(id);
            
            
        }
        public void OnPost() {
            
        }
    }
}