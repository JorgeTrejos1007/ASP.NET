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
    [PermisosDeVista("Educador", "Coordinador")]
    public class RegistrarFirmaModel : PageModel
    {
        [BindProperty]
        public IFormFile firma { get; set; }

        public void OnGet()
        {
        }
        public IActionResult OnPostAgregarFirma()
        {
            IActionResult vista;
            try
            {
                
                if ((firma != null) && (firma.ContentType == "image/jpeg" || firma.ContentType == "image/png"))
                {
                    ViewData["tipoUsuario"] = TempData["tipoUsuario"];
                    if ((string)ViewData["tipoUsuario"] == "Educador")
                    {
                        Miembro datosDelMiembro = Sesion.obtenerDatosDeSesion(HttpContext.Session, User.Identity.Name);
                        MiembroHandler accesoDatos = new MiembroHandler();
                        accesoDatos.agregarFirmaEducador(datosDelMiembro.email, firma);
                        vista = Redirect("~/Curso/MisCursosPropuestos");
                    }
                    else if ((string)ViewData["tipoUsuario"] == "Coordinador")
                    {
                        Miembro datosDelMiembro = Sesion.obtenerDatosDeSesion(HttpContext.Session, User.Identity.Name);
                        MiembroHandler accesoDatos = new MiembroHandler();
                        accesoDatos.agregarFirmaCoordinador(datosDelMiembro.email, firma);
                        vista = Redirect("~/Certificado/AprobarCertificado");
                    }
                    else
                    {
                        vista = Redirect("~/Index"); //Redirigir a pagina 404
                    }
                }
                else
                {
                    vista = RedirectToPage("./RegistrarFirma");
                }
            }
            catch
            {
                vista = Redirect("~/Miembros/RegistrarFirma");
            }
            return vista;

        }
    }
}