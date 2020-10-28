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

namespace PIBasesISGrupo1.Pages.Curso
{
    public class ModificarSeccionModel : PageModel
    {
        [BindProperty]
        public SeccionModel seccion { get; set; }
        [BindProperty]
        public MaterialModel material { get; set; }
        public void OnGet(String nombreCurso, String nombreSeccion)
        {
            ViewData["nombreCurso"] = nombreCurso;
            ViewData["nombreSeccion"] = nombreSeccion;
            CursoHandler accesoDatos = new CursoHandler();        
            ViewData["listaMateriales"] = accesoDatos.obtenerMaterialDeUnaSeccion(nombreSeccion,nombreCurso);
            TempData["cursoModificado"] = TempData["cursoModificado"];
        }
        public IActionResult OnPostBorrarMaterial()
        {
            CursoHandler accesodatos = new CursoHandler();
            if ((bool)TempData["cursoModificado"] == false)
            {
                TempData["cursoModificado"] = true;
              
                accesodatos.actualizarVersion(material.nombreDeCurso);
            }
            accesodatos.borrarMaterial(material);
            return RedirectToPage("ModificarSeccion", new { nombreCurso = material.nombreDeCurso, nombreSeccion=material.nombreDeSeccion });
        }

        public IActionResult OnPostBorrarSeccion()
        {
            CursoHandler accesodatos = new CursoHandler();
            if ((bool)TempData["cursoModificado"] == false)
            {
                TempData["cursoModificado"] = true;

                accesodatos.actualizarVersion(seccion.nombreCurso);
            }
            accesodatos.borrarSeccion(seccion);
            return RedirectToPage("CursoCreado" , new{nombreCurso = seccion.nombreCurso});
        }
    }
}