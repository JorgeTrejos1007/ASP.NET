using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PIBasesISGrupo1.Models;
using PIBasesISGrupo1.Handler;

namespace PIBasesISGrupo1.Pages
{
    public class CatalogoModel : PageModel
    {
        [BindProperty]
        public string[] topicosAelegir { get; set; }

        public void OnGet()
        {
            CatalogoHandler accesoCatalago = new CatalogoHandler();
            ViewData["TopicosYCategorias"] = accesoCatalago.obteneTodosLosTopicosYCategoriasAsociadas();
        }


        public void OnPost()
        {
            CatalogoHandler accesoCatalago = new CatalogoHandler();
            
        }

    }
}