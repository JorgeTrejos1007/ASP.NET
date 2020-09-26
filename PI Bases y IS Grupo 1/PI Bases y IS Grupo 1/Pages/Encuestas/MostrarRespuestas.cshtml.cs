using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PIBasesISGrupo1.Models;
using PIBasesISGrupo1.Handler;


namespace PIBasesISGrupo1.Pages.Encuestas
{
    public class MostrarRespuestasModel : PageModel
    {
        [BindProperty]
        public List<MostarRespuestaModel> respuestas { get; set; }
        public void OnGet(int id)
        {
            ViewData["id"] = id;
            RespuestasHandler accesoDatos = new RespuestasHandler();
            respuestas = accesoDatos.obtenerRespuestas(id);

        }
    }
}