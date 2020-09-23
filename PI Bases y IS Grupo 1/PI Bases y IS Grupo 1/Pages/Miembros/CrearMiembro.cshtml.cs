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
        public Miembro miembro { get; set; }

        [BindProperty]
        public IFormFile archivoImagen { get; set; }

        public void OnGet()
        {

        }
        
        public void OnPost()
        {

            try
            {
                MiembroHandler accesoDatos = new MiembroHandler();
                if (accesoDatos.crearMiembro(miembro))
                {


                    TempData["mensaje"] = "Se ha logrado registar con exito";
                    TempData["exitoAlEditar"] = true;
                    if (archivoImagen != null)
                    {
                        accesoDatos.actualizarImagen(miembro.email, archivoImagen);
                    }
                }
                else
                {
                    TempData["mensaje"] = "Se ha ocurrido un error en el registro";
                    TempData["exitoAlEditar"] = false;
                }

            }
            catch {

                TempData["mensaje"] = "Se ha ocurrido un error en el registro";
                TempData["exitoAlEditar"] = false;
            }
            

            
        }
    }
}