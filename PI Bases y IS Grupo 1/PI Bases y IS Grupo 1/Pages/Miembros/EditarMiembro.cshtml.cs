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

        public void ActionResult(String email)
        {
            ActionResult vista = null;
            try {

                MiembroHandler accesoDatos = new MiembroHandler();
                Miembro miembroModificar= accesoDatos.obtenerTodoslosMiembros().Find(smodel => smodel.Email == email);
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

        public void OnPost()
        {

        }


        public void OnPostMiembro()
        {

        }

    }
}