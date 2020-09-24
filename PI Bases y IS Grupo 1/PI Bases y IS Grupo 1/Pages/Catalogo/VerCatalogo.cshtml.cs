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
        public string categoria { get; set; }

        [BindProperty]
        public Catalogo catalogo { get; set; }


        public void OnGet()
        {
            CatalogoHandler accesoCatalago = new CatalogoHandler();
            ViewData["TopicosYCategorias"] = accesoCatalago.obteneTodosLosTopicosYCategoriasAsociadas();
        }


        public void OnPost()
        {
            /*CatalogoHandler accesoCatalogoTopico = new CatalogoHandler();
            if (accesoCatalogoTopico.insertarTopico(catalogo))
            {
                TempData["mensaje"] = "Topico agregado exito";
                TempData["exitoAlEditar"] = true;
            }
            else
            {
                TempData["mensaje"] = "Algo salió mal al añadir tópico";
                TempData["exitoAlEditar"] = false;
            }*/
        }

        public void OnPostCategoria()
        {
            CatalogoHandler accesoCatalogoCategoria = new CatalogoHandler();
            if (accesoCatalogoCategoria.insertarCategoria(categoria))
            {
                TempData["mensaje"] = "Topico agregado exito";
                TempData["exitoAlEditar"] = true;
            }
            else
            {
                TempData["mensaje"] = "Algo salió mal al añadir tópico";
                TempData["exitoAlEditar"] = false;
            }
        }


     


    }
}