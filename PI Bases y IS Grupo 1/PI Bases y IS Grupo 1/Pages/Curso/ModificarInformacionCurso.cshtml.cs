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
    public class ModificarInformacionCursoModel : PageModel
    {
        [BindProperty]
        public Cursos curso { get; set; }
        public void OnGet(string nombreCurso)
        {
            CursoHandler accesodatos = new CursoHandler();
            ViewData["informacionCurso"]= accesodatos.obtenerInformacionCurso(nombreCurso);
            CatalogoHandler accesoCatalago = new CatalogoHandler();
            ViewData["TopicosYCategorias"] = accesoCatalago.obteneTodosLosTopicosYCategoriasAsociadas();
            ViewData["nombreCurso"] = nombreCurso;
            TempData["nombreCurso"] = nombreCurso;
            TempData["cursoModificado"] = TempData["cursoModificado"];
        }

        public IActionResult OnPostModificarCurso()
        {
            if ((bool)TempData["cursoModificado"] == false)
            {
                TempData["cursoModificado"] = true;
                curso.version ++;
            }

            CursoHandler accesodatos = new CursoHandler();
            accesodatos.actualizarInfoCurso(curso, (string)TempData["nombreCurso"]);
            return RedirectToPage("CursoCreado", new { nombreCurso = curso.nombre });
        }
    }
}