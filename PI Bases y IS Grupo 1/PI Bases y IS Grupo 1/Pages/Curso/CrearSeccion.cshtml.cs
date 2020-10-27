﻿using System;
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

        public IActionResult OnGet(string nombreCurso, string nombrePaginaCurso)
        {
            try{
                TempData["nombreCurso"] = nombreCurso;
                ViewData["seccionCreada"] = false;
                ViewData["nombrePaginaCurso"] = nombrePaginaCurso;
                TempData["nombrePaginaCurso"] = nombrePaginaCurso;
                if (TempData["ocurrioError"]!=null)
                {
                    ViewData["mensajeError"] = "Esta seccion ya esta en el curso";
                }
                else
                {
                    ViewData["mensajeError"] = "";
                }
                return Page();
            }
            catch
            {
                return RedirectToPage(nombrePaginaCurso);
            }
            
        }

        public void OnGetSeccionCreada()
        {    
            ViewData["nombreCurso"] = TempData["nombreCurso"];
            ViewData["seccionCreada"] = TempData["seccionCreada"];
            ViewData["nombreSeccion"] = TempData["nombreSeccion"];
            ViewData["nombrePaginaCurso"] = TempData["nombrePaginaCurso"];
            TempData["nombrePaginaCurso"] = ViewData["nombrePaginaCurso"];
        }

        public void OnGetMaterialAgregado()
        {
            ViewData["nombreCurso"] = TempData["nombreCurso"];
            ViewData["seccionCreada"] = TempData["seccionCreada"];
            ViewData["nombreSeccion"] = TempData["nombreSeccion"];
            ViewData["nombrePaginaCurso"] = TempData["nombrePaginaCurso"];
            TempData["nombrePaginaCurso"] = ViewData["nombrePaginaCurso"];
            CursoHandler accesoDatos = new CursoHandler();
            string nombreSeccion = (string)ViewData["nombreSeccion"];
            ViewData["listaMateriales"] = accesoDatos.obtenerMaterialDeUnaSeccion(nombreSeccion, (string)ViewData["nombreCurso"]);
        }
        public IActionResult OnPostCrearSeccion()
        {
            IActionResult vista;
            CursoHandler accesoDatos = new CursoHandler();
            if (accesoDatos.crearSeccion(seccion)) {
                TempData["seccionCreada"] = true;
                TempData["nombreSeccion"] = seccion.nombreSeccion;
                TempData["nombreCurso"] = seccion.nombreCurso;
                vista = RedirectToPage("CrearSeccion", "SeccionCreada");
            }
            else
            {
                TempData["ocurrioError"] = true;
                string nombrePaginaCursoTemp = (string)TempData["nombrePaginaCurso"];
                vista = RedirectToPage("CrearSeccion", new {nombreCurso = seccion.nombreCurso, nombrePaginaCurso = nombrePaginaCursoTemp });
            }
            return vista;
        }
        public IActionResult OnPostAgregarMaterial()
        {
            IActionResult vista;
            try
            {
                TempData["seccionCreada"] = true;
                TempData["nombreSeccion"] = material.nombreDeSeccion;
                TempData["nombreCurso"] = material.nombreDeCurso;
                CursoHandler accesoDatos = new CursoHandler();
                accesoDatos.agregarMaterial(material, archivo);
                vista = RedirectToPage("CrearSeccion", "MaterialAgregado");
            }
            catch
            {
                vista = RedirectToPage("CursoCreado",new { nombreCurso = material.nombreDeCurso});
            }
            return vista;
           
        }
    }
}