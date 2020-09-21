﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Mime;
using System.IO;
using PIBasesISGrupo1.Handler;
using PIBasesISGrupo1.Models;
using Microsoft.AspNetCore.Http;

namespace PIBasesISGrupo1.Pages.Miembros
{
    public class EditarMiembroModel : PageModel
    {
        [BindProperty]
        public Miembro miembro { get; set; }

        [BindProperty]
        public IFormFile archivoImagen { get; set; }

        [BindProperty]
        public string[] habilidadesBorrar { get; set; }

        [BindProperty]
        public string[] idiomasBorrar { get; set; }

        public IActionResult OnGet(String email)
        {
            IActionResult vista;

            string[] habilidadesElegir= { "Empatia", "Saber escuchar", "Liderazgo", "Flexibilidad",
            "Optimismo","Confianza","Optimismo","Confianza","Comunicacion","Convencimiento","Esfuerzo","Dedicacion",
            "Comprension","Asertividad","Credibilidad"};

            string[] idiomasElegir = { "Arabe", "Aleman", "Bengali", "Español",
            "Frances","Hindi","Ingles","Mandarin","Panyabí","Portugues","Ruso"};


            ViewData["HabilidadesElegir"] = habilidadesElegir;

            ViewData["idiomasElegir"] = idiomasElegir;

            try {
                MiembroHandler accesoDatos = new MiembroHandler();
                Miembro miembroModificar = accesoDatos.obtenerTodosLosMiembros().Find(smodel => smodel.email == email);
             

                if (miembroModificar == null)
                {
                    vista = Redirect("~/Miembros/DesplegarMiembros");
                }
                else {

                    ViewData["MiembroModificar"] = miembroModificar;
                    
                    
                    vista = Page();
                }
            }
            catch
            {
                vista = Redirect("~/Miembros/DesplegarMiembros");
            }

            return vista;

        }

       
        public IActionResult OnPost()
        {
            MiembroHandler accesoDatos = new MiembroHandler();
            if (archivoImagen != null) {
                accesoDatos.actualizarImagen(miembro.email, archivoImagen);
            }

            if (habilidadesBorrar.Length>0) {
                accesoDatos.eliminarHabilidesMiembro(miembro.email, habilidadesBorrar);
            }

            if (idiomasBorrar.Length > 0)
            {
                accesoDatos.eliminarIdiomasMiembro(miembro.email, idiomasBorrar);
            }


            if (accesoDatos.modificarMiembro(miembro))
            {
                TempData["mensaje"] = "Informacion editada con exito";
                TempData["exitoAlEditar"] = true;
            }
            else {
                TempData["mensaje"] = "Algo salió mal y no fue posible editadar la informacion :(";
                TempData["exitoAlEditar"] = false;
            }
            return RedirectToAction("~/Miembros/EditarMiembro", new { email = miembro.email });
        }

    }

   
}


