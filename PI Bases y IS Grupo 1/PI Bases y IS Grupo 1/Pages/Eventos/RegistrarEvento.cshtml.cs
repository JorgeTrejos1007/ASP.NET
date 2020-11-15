using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PIBasesISGrupo1.Models;
using PIBasesISGrupo1.Handler;
using Microsoft.AspNetCore.Http;
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

        }

        public void OnPost()
        {

            var miembro = Sesion.obtenerDatosDeSesion(HttpContext.Session, "Miembro");
            evento.emailCoordinador  = miembro.email;

            EventoHandler accesoDatosEventos = new EventoHandler();
            if (accesoDatosEventos.registrarEvento(evento, archivoImagen))
            {
                TempData["mensaje"] = "Se ha logrado agregar noticia con exito";
                TempData["exitoAlEditar"] = true; 
            }
        }
    }
}