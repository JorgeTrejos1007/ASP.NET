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
    public class AgregarMaterialModel : PageModel
    {
        [BindProperty]
        public MaterialModel material { set; get; }
        [BindProperty]
        [Required(ErrorMessage = "Es necesario que ingrese un archivo")]
        public IFormFile archivo { get; set; }
        public void OnGet(String nombreCurso, String nombreSeccion)
        {
            ViewData["nombreCurso"] = nombreCurso;
            ViewData["nombreSeccion"] = nombreSeccion;
            TempData["cursoModificado"] = TempData["cursoModificado"];
        }
        public IActionResult OnPostAgregarMaterial()
        {   
            CursoHandler accesoDatos = new CursoHandler();
            if ((bool)TempData["cursoModificado"] == false)
            {
                TempData["cursoModificado"] = true;
                accesoDatos.actualizarVersion(material.nombreDeCurso);
            }
            accesoDatos.agregarMaterial(material, archivo);
            return RedirectToPage("ModificarSeccion", new { nombreCurso = material.nombreDeCurso, nombreSeccion = material.nombreDeSeccion });
        }
    }
}