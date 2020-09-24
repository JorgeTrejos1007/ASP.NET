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

        public IActionResult OnPostAgregarCategoria()
        {
            try
            {
                CatalogoHandler accesoCatalogo = new CatalogoHandler();
                accesoCatalogo.insertarCategoria(catalogo.categoria);
                TempData["mensajeCategoria"] = "Categoría agregada con exito";
                TempData["exitoAlAgregarCategoria"] = true;

            }
            catch {
                TempData["mensajeCategoria"] = "Es posible que se esté agregando una categoría existente";
                TempData["exitoAlAgregarCategoria"] = false;
                
            }
            return RedirectToPage("EditarCatalogo");
        }

        public IActionResult OnPostAgregarTopico()
        {
            
           try
            {
                CatalogoHandler accesoCatalogo = new CatalogoHandler();
                accesoCatalogo.insertarTopico(catalogo);
                TempData["mensajeTopico"] = "Topico agregado con exito";
                TempData["exitoAlAgregarTopico"] = true;

            }
            catch
            {
                TempData["mensajeTopico"] = "Es posible que se esté agregando un topico existente";
                TempData["exitoAlAgregarTopico"] = false;

            }
            return RedirectToPage("EditarCatalogo");
        }
    }
}




    