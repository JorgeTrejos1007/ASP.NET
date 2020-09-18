using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PIBasesISGrupo1.Models;
using PIBasesISGrupo1.Handler;
using Microsoft.AspNetCore.Http;

namespace PIBasesISGrupo1.Pages.Miembros
{
    public class CrearMiembroModel : PageModel
    {

        [BindProperty]        
        public Miembro Miembro { get; set; }
        [BindProperty]
        public IFormFile archivoImagen { get; set; }
        public void OnGet()
        {

        }
        public void OnPost(int id)
        {

        }
        public void OnPostMiembro()
        {
            bool ExitoAlCrear = false;
            MiembroHandler accesoDatos = new MiembroHandler();
            ExitoAlCrear = accesoDatos.crearMiembro(Miembro); // recuerde que este método devuelve un booleano                     
            if (archivoImagen == null) {

            }

        }
    }
}