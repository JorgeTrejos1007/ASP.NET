using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PIBasesISGrupo1.Models;
using PIBasesISGrupo1.Handler;
namespace PIBasesISGrupo1.Pages
{
    public class IndexModel : PageModel
    {   
        public void OnGet()
        {
            MiembroHandler accesoDatos = new MiembroHandler();
            ViewData["TotalMiembros"] = accesoDatos.obtenerNumeroDeMiembros();
            ViewData["PaisesMiembro"] = accesoDatos.obtenerPaisesMiembro();
        }
    }
}
