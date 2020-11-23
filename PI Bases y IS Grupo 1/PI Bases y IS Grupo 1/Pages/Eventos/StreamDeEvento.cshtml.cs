using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PIBasesISGrupo1.Pages.Eventos
{
    public class StreamDeEventoModel : PageModel
    {
        public void OnGet(string nombreE, string fecha, string email)
        {
            nombreE = "Gamplay con auron"; 
            TempData["nombreEvento"] = nombreE;
            /*Aqui se llama al handler para conseguir el nombre del canal con los datos dados en parametros*/
            TempData["nombreCanal"] = "auronplay";
        }
    }
}