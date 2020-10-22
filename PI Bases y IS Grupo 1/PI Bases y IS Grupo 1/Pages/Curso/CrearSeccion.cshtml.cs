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
    public class CrearSeccionModel : PageModel
    {
        [BindProperty]
        public SeccionModel seccion { get; set; }

        [BindProperty]
        public MaterialModel material { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Es necesario que ingrese un archivo")]
        public IFormFile archivo { get; set; }

        public IActionResult OnGet(string nombreCurso)
        {
            try{
                TempData["nombreCurso"] = nombreCurso;
                ViewData["seccionCreada"] = false;
                return Page();
            }
            catch
            {
                return RedirectToPage("CursosAprobados");
            }
            
        }

        public void OnGetSeccionCreada()
        {    
            ViewData["nombreCurso"] = TempData["nombreCurso"];
            ViewData["seccionCreada"] = TempData["seccionCreada"];
            ViewData["nombreSeccion"] = TempData["nombreSeccion"];        
        }

        public void OnGetMaterialAgregado()
        {
            ViewData["nombreCurso"] = TempData["nombreCurso"];
            ViewData["seccionCreada"] = TempData["seccionCreada"];
            ViewData["nombreSeccion"] = TempData["nombreSeccion"];
            CursoHandler accesoDatos = new CursoHandler();
            string nombreSeccion = (string)ViewData["nombreSeccion"];
            ViewData["listaMateriales"] = accesoDatos.obtenerMaterialDeUnaSeccion(nombreSeccion);
        }
        public IActionResult OnPostCrearSeccion()
        {
            TempData["seccionCreada"] = true;
            TempData["nombreSeccion"] = seccion.nombreSeccion;
            TempData["nombreCurso"] = seccion.nombreCurso;
            CursoHandler accesoDatos = new CursoHandler();
            accesoDatos.crearSeccion(seccion);
            return RedirectToPage("CrearSeccion","SeccionCreada");
        }
        public IActionResult OnPostAgregarMaterial()
        {
            TempData["seccionCreada"] = true;
            TempData["nombreSeccion"] = material.nombreDeSeccion;
            TempData["nombreCurso"] = material.nombreDeCurso;   
            CursoHandler accesoDatos = new CursoHandler();
            accesoDatos.agregarMaterial(material, archivo);
            return RedirectToPage("CrearSeccion", "MaterialAgregado");
        }
    }
}