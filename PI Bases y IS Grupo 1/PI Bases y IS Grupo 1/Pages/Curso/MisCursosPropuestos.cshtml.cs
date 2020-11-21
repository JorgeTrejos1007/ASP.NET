using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PIBasesISGrupo1.Filters;
using PIBasesISGrupo1.Handler;
using PIBasesISGrupo1.Models;

namespace PIBasesISGrupo1.Pages.Curso
{
    [PermisosDeVista("Miembro", "Miembro de Nucleo", "Educador", "Coordinador")]
    public class MisCursosPropuestosModel : PageModel
    {
        private CursoHandler cursoHandler = new CursoHandler();
        public void OnGet()
        {
            Miembro datosDelMiembro = Sesion.obtenerDatosDeSesion(HttpContext.Session, User.Identity.Name);

            ViewData["MisCursosPropuestos"] = cursoHandler.obtenerMisCursosPropuestos(datosDelMiembro.email);

            MiembroHandler accesoDatos = new MiembroHandler();
            ViewData["firmaEducador"] = accesoDatos.obtenerFirmaEducador(datosDelMiembro.email);
        }
    }
}