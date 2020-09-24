using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PIBasesISGrupo1.Models;
using PIBasesISGrupo1.Handler;

namespace PIBasesISGrupo1.Pages.Catalogo
{
    public class EditarCatalogoModel : PageModel
    {
        [BindProperty]
        string categoria { get; set; }

        public void OnGet()
        {

        }


        public void OnPostAgregarCategoria()
        {
            CatalogoHandler accesoCatalogo = new CatalogoHandler();
            accesoCatalogo.insertarCategoria(categoria);

        }

    }
}