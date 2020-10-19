using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PIBasesISGrupo1.Models;

namespace PIBasesISGrupo1.Pages.Logout
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {

            IActionResult vista;

            Sesion.cerrarSesion(HttpContext.Session);

            vista = Redirect("/Index");

            return vista;
        }
    }
}