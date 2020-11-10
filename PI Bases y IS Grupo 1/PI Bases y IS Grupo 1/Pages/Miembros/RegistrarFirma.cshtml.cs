using System;
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
using System.ComponentModel.DataAnnotations;
using PIBasesISGrupo1.Filters;

namespace PIBasesISGrupo1.Pages.Miembros
{
    //[PermisosDeVista("Educador","Coordinador")]
    public class RegistrarFirmaModel : PageModel
    {
        [BindProperty]
        public IFormFile firma { get; set; }

        public void OnGet()
        {

        }
    }
}