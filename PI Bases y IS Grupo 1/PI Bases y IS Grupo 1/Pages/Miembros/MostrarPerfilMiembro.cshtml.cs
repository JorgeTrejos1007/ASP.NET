using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PIBasesISGrupo1.Models;
using PIBasesISGrupo1.Handler;

namespace PIBasesISGrupo1.Pages.Miembros
{
    public class MostrarPerfilMiembroModel : PageModel
    {
       
            [BindProperty]
        public Miembro miembro { get; set; }

        public void OnGet(string email)
        {
            MiembroHandler accesoDatos = new MiembroHandler();
            ViewData["email"] = email;
            miembro = accesoDatos.obtenerDatosDeUnMiembro(email);
        }
    }    
}