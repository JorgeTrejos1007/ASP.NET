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
        [BindProperty]
        public string tipoImagen { get; set; }
        public IActionResult OnGet()
        {
            IActionResult vista;
            try
            {
                Miembro datosDelCoordinador = Sesion.obtenerDatosDeSesion(HttpContext.Session, User.Identity.Name);
                ViewData["email"] = datosDelCoordinador.email;
                MiembroHandler accesoDatos = new MiembroHandler();
                ViewData["firmaCoordinador"] = accesoDatos.obtenerFirmaCoordinador(datosDelCoordinador.email);
                if (ViewData["firmaCoordinador"] != null)
                {

                    accesoAlCertificado = new CertificadoHandler();
                    ViewData["Certificados"] = accesoAlCertificado.obtenerCertificadosNoAprobados();
                    vista = Page();
                }
                else
                {
                    TempData["tipoUsuario"] = "Coordinador";
                    vista = Redirect("~/Miembros/RegistrarFirma");
                }
            }
            catch
            {
                vista = Redirect("~/Index"); //Redirigir a pagina 404
            }
            return vista;
        }
        public IActionResult OnPost(string emailEstudiante, string nombreCurso, string emailCoordinador)
        {
            accesoAlCertificado = new CertificadoHandler();
            string tipoBinario = tipoImagen.Substring(22);
            byte[] tipoImagenBinaria = Convert.FromBase64String(tipoBinario);
            //bool exito = accesoAlCertificado.aprobarCertificado(emailEstudiante, nombreCurso, emailCoordinador,tipoImagenBinaria);
            
            return RedirectToPage("AprobarCertificado");
        }
    }
}