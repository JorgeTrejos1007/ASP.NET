using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PIBasesISGrupo1.Models;

namespace PIBasesISGrupo1.Pages.Miembros
{
    public class CrearMiembroModel : PageModel
    {

        [BindProperty]        
        public Noticia Miembro { get; set; }
        public void OnGet()
        {

        }
        public void OnPost(int id)
        {

        }
        public void OnPostMiembro() {
        } 
    }
}