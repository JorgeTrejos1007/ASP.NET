using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PIBasesISGrupo1.Models;
using PIBasesISGrupo1.Handler;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
namespace PIBasesISGrupo1.Pages.Eventos
{
    public class RegistrarEventoModel : PageModel
    { 

        [BindProperty]
        public Evento evento { get; set; }

        [BindProperty]
        public IFormFile archivoImagen { get; set; }
    
        public void OnGet()
        {
            Sector sector = new Sector();
            ViewData["sector"] = sector; 
        }

        public IActionResult OnPost()
        {
            /*evento.tipo = "Virtual";
            evento.nombreCanalStream = "Ronnyale";
            
            evento.lugar = "Jaco";
            Sector sector1 = new Sector();
            sector1.nombreDeSector = "Sombra";
            sector1.tipo = "No numerado";
            sector1.cantidadAsientos = 3;
            Sector sector2 = new Sector();
            sector2.nombreDeSector = "Pentahouse";
            sector2.tipo = "Numerado";
            sector2.cantidadAsientos = 4;
            evento.sectores.Add(sector1);
            evento.sectores.Add(sector2);*/

            IActionResult vista;
            try
            {
                var miembro = Sesion.obtenerDatosDeSesion(HttpContext.Session, "Miembro");
                evento.emailCoordinador = miembro.email;

                EventoHandler accesoDatosEventos = new EventoHandler();

                if (accesoDatosEventos.registrarEvento(evento, archivoImagen))
                {
                    if (evento.tipo.Equals("Virtual"))
                    {
                        accesoDatosEventos.registrarEventoVirtual(evento);
                    }
                    else
                    {
                        if (accesoDatosEventos.registrarEventoPresencial(evento))
                        {
                            accesoDatosEventos.registrarSectores(evento);
                        }
                    }
                }
                vista = Redirect("~/Index");
            }
            catch
            {
                vista = Redirect("~/Error");
            }
            return vista;
        }
    }
}