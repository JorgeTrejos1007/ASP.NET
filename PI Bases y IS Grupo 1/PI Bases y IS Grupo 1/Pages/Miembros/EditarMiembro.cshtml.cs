using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PIBasesISGrupo1.Handler;
using PIBasesISGrupo1.Models;

namespace PIBasesISGrupo1.Pages.Miembros
{
    public class EditarMiembroModel : PageModel
    {
        [BindProperty]
        public Miembro Miembro { get; set; }
        public void OnGet(String email)
        {
            
            try {

                MiembroHandler accesoDatos = new MiembroHandler();
                Miembro miembroModificar= accesoDatos.obtenerTodosLosMiembros().Find(smodel => smodel.Email == email);
                if (miembroModificar == null)
                {
                     RedirectToPage("index");

                }
                else {

                    ViewData["MiembroModificar"] = miembroModificar;
                }
            }
            catch
            {
                  RedirectToPage("index");
            }

        }


        public IActionResult OnPost()
        {
            MiembroHandler accesoDatos = new MiembroHandler();

            accesoDatos.modificarMiembro(Miembro);

            return Redirect("~/Miembros/DesplegarMiembros");

        }

    }
}