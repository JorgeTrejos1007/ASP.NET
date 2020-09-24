using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PIBasesISGrupo1.Models;
using PIBasesISGrupo1.Handler;
using Microsoft.AspNetCore.Http;


namespace PIBasesISGrupo1.Pages.Catalogos
{
    public class EditarCatalogoModel : PageModel
    {
        [BindProperty]
        string categoria { get; set; }

        [BindProperty]
        public Catalogo catalogo { get; set; }

        public void OnGet()
        {
            CatalogoHandler accesoCatalogo = new CatalogoHandler();
            ViewData["Categorias"] = accesoCatalogo.obtenerCategorias();

        }

        public void OnPostAgregarCategoria()
        {
            CatalogoHandler accesoCatalogo = new CatalogoHandler();
            accesoCatalogo.insertarCategoria(catalogo.categoria);
        }

        public void OnPostAgregarTopico()
        {
            CatalogoHandler accesoCatalogo = new CatalogoHandler();
            accesoCatalogo.insertarTopico(catalogo);
        }

    }
}