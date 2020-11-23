using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PIBasesISGrupo1.Handler;

namespace PIBasesISGrupo1.Pages.Eventos
{
    public class StreamDeEventoModel : PageModel
    {
        EventoHandler eventoHandler = new EventoHandler();

        public void OnGet(string nombreE, string fecha, string email)
        {
            nombreE = "Evento de prueba"; 
            TempData["nombreEvento"] = nombreE;

            fecha = "2020-12-02 13:00:00.000";
            email = "stevegc112016@gmail.com";
            TempData["nombreCanal"] = eventoHandler.obtenerNombreCanalDeStream(email, nombreE, Convert.ToDateTime(fecha));
        }
    }
}