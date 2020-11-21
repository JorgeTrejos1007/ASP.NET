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
using System.Text;
namespace PIBasesISGrupo1.Pages.Certificado
{
    
    public class MisCertificadosModel : PageModel
    {
        [BindProperty]
        public Certificados certificado { get; set; }
        public void OnGet()
        {
            Miembro datosDelEstudiante = Sesion.obtenerDatosDeSesion(HttpContext.Session, User.Identity.Name);
            CertificadoHandler accesoAlCertificado = new CertificadoHandler();
            ViewData["Certificados"] = accesoAlCertificado.obtenerMisCertificados(datosDelEstudiante.email);
            
        }
        public virtual RedirectToPageResult OnPost( string fecha,string emailCoordinador,string emailEducador, string nombreEstudiante,string nombreCurso, string nombreEducador ) {
            TempData["emailEducador"] = emailEducador;
            TempData["emailCoordinador"] = emailCoordinador;
            TempData["fecha"] = fecha;
            TempData["nombreEducador"] = nombreEducador;
            TempData["nombreCurso"] = nombreCurso;
            TempData["nombreEstudiante"] = nombreEstudiante;
            return RedirectToPage("DescargarCertificado");

        }
        

}
}