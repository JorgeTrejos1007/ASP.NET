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

namespace PIBasesISGrupo1.Pages.Certificado
{
    public class AprobarCertificadoModel : PageModel
    {
        private CertificadoHandler accesoAlCertificado = new CertificadoHandler();
        public void OnGet()
        {
            ViewData["Certificados"] = accesoAlCertificado.obtenerCertificadosNoAprobados();

        }
        public IActionResult OnPost(int id)
        {

            return RedirectToPage("AprobarCertificado");
        }
    }
}