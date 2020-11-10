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

namespace PIBasesISGrupo1.Pages.Curso
{
    public class CrearCertificadoModel : PageModel
    {
        public void OnGet()
        {
            Miembro datosDelMiembro = Sesion.obtenerDatosDeSesion(HttpContext.Session, User.Identity.Name);
            MiembroHandler accesoDatos = new MiembroHandler();
            ViewData["firma"] = accesoDatos.obtenerFirmaEducador(datosDelMiembro.email);
         
        }
    }
}