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
    [PermisosDeVista("Coordinador")]
    public class AprobarCertificadoModel : PageModel
    {
        private CertificadoHandler accesoAlCertificado;
        public void OnGet()
        {
            accesoAlCertificado = new CertificadoHandler();
            ViewData["Certificados"] = accesoAlCertificado.obtenerCertificadosNoAprobados();

        }
        public IActionResult OnPost(int id)
        {
            accesoAlCertificado = new CertificadoHandler();
            bool exito = accesoAlCertificado.aprobarCertificado(id);
            return RedirectToPage("AprobarCertificado");
        }
    }
}