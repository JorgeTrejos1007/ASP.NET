﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Mime;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using PIBasesISGrupo1.Handler;
using PIBasesISGrupo1.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

using PIBasesISGrupo1.Filters;

namespace PIBasesISGrupo1.Pages.Curso
{
    [PermisosDeVista("Miembro", "Miembro de Nucleo", "Educador","Coordinador")]
    public class ProponerCursoModel : PageModel
    {
        [BindProperty]
        public Cursos curso { get; set; }
        [BindProperty]

        [Required(ErrorMessage = "Es necesario que suba el documento descriptivo del curso")]
        public IFormFile archivo { get; set; }

        
        public IActionResult OnGet()
        {
           

            IActionResult vista;
           
                try
                {
                    vista = Page();
                    CatalogoHandler accesoCatalago = new CatalogoHandler();
                    ViewData["TopicosYCategorias"] = accesoCatalago.obteneTodosLosTopicosYCategoriasAsociadas();
                }
                catch
                {
                    vista = Redirect("~/Curso/ProponerCurso");
                }

          
            return vista;
        }
        public IActionResult OnPost()
        {

            try
            {
                CursoHandler accesoDatos = new CursoHandler();
                if (accesoDatos.proponerCurso(curso, archivo))
                {
                    TempData["mensaje"] = "Curso Propuesto Con Exito";
                    TempData["exitoAlProponer"] = true;
                }
                else
                {
                    TempData["mensaje"] = "Algo salió mal y no fue posible proponer el curso";
                    TempData["exitoAlProponer"] = false;
                }
                

            }
            catch {

                TempData["mensaje"] = "Solo un educador puede proponer un curso";
                TempData["exitoAlProponer"] = false;

            }
            return RedirectToAction("~/Cursos/ProponerCurso");




        }
    }
}