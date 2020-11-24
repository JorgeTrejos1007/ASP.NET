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

        public void OnGet(string nombreE, string nombreCanal)
        {
            TempData["nombreEvento"] = nombreE;
            TempData["nombreCanal"] = nombreCanal;
        }
    }
}
