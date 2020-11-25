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
        [BindProperty]
        public Sector sector { get; set; }

        public IActionResult OnGet()
        {
            IActionResult vista;
            try
            {
                if (TempData["nombreEvento"] == null)
                {
                    vista = Redirect("~/Error");
                }
                else
                {
                    ViewData["nombreEvento"] = TempData["nombreEvento"];
                    TempData["nombreEvento"] = ViewData["nombreEvento"];
                    vista = Page();
                }

            }
            catch
            {
                vista = Redirect("~/Error");
            }
            return vista;
        }

        public IActionResult OnPostAgregarSector(string nombre, int cantidadDeAsientos, string tipoAsiento)
        {
            if (nombre == null || cantidadDeAsientos == 0 || tipoAsiento == null)
            {
                return new JsonResult("FaltanDatos");
            }
            else
            {
                EventoHandler accesoDatos = new EventoHandler();
                Miembro datosDelMiembro = Sesion.obtenerDatosDeSesion(HttpContext.Session, User.Identity.Name);
                sector.nombreDeSector = nombre;
                sector.cantidadAsientos = cantidadDeAsientos;
                sector.tipo = tipoAsiento;
                string nombreEvento = (string)TempData["nombreEvento"];
                DateTime fechaYHora = (DateTime)TempData["fechaYHora"];
                TempData["fechaYHora"] = fechaYHora;
                TempData["nombreEvento"] = nombreEvento;
                if (accesoDatos.registrarSector(sector, datosDelMiembro.email, nombreEvento, fechaYHora))
                {
                    return new JsonResult(sector);
                }
                else
                {
                    return new JsonResult("ErrorAlAñadirSector");
                }

            }
        }

        public IActionResult OnPostTerminarCrearSectores()
        {
            return Redirect("~/Eventos/MostrarEventos");
        }
    }
}