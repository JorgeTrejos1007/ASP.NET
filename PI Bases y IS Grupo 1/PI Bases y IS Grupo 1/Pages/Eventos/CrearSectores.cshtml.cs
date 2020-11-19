using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PIBasesISGrupo1.Models;
using PIBasesISGrupo1.Handler;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace PIBasesISGrupo1.Pages.Eventos
{
    public class CrearSectoresModel : PageModel
    {
        public IActionResult OnGet()
        {
            IActionResult vista;
            try
            {
                ViewData["nombreEvento"] = TempData["nombreEvento"];
                TempData["nombreEvento"] = ViewData["nombreEvento"];
                vista = Page();
            }
            catch
            {
                vista = Redirect("~/Error");
            }
            return vista;
        }
    }
}