using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PIBasesISGrupo1.Models;
using PIBasesISGrupo1.Handler;
using Microsoft.AspNetCore.Http;


namespace PIBasesISGrupo1.Pages
{
    public class NoticiasModel : PageModel
    {
        [BindProperty]
        public Noticia noticia { get; set; }

        [BindProperty]
        public IFormFile archivoImagen { get; set; }

        [BindProperty]
        public IFormFile archivoNoticia { get; set; }


        public void OnGet()
        {

        }

        public void OnPost()
        {

            NoticiaHandler accesoDatos = new NoticiaHandler();
            if (accesoDatos.crearNoticia(noticia, archivoNoticia,archivoImagen))
            {
                TempData["mensaje"] = "Se ha logrado agregar noticia con exito";
                TempData["exitoAlEditar"] = true;

                /*
                if (archivoImagen != null)
                {
                    accesoDatos.actualizarImagen(miembro.email, archivoImagen);
                }
            }
            else
            {
                TempData["mensaje"] = "Se ha logrado registar con exito";
                TempData["exitoAlEditar"] = false;
            }
            */
            }



        }
    }
}
