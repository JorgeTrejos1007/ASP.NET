﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PIBasesISGrupo1.Models;
using PIBasesISGrupo1.Handler;

namespace PIBasesISGrupo1.Pages.Encuestas
{
    public class EditarPreguntaModel : PageModel
    {
        [BindProperty]
        public PreguntaModel pregunta { get; set; }
        public void OnGet(int idEnc, int idPreg)
        {
            PreguntasHandler accesodatos = new PreguntasHandler();
            this.pregunta = accesodatos.obtenerTuplaPregunta(idEnc, idPreg);
        }
        public IActionResult OnPostEditarPregunta()
        {
            PreguntasHandler accesodatos = new PreguntasHandler();
            accesodatos.modificarPregunta(pregunta);
            return RedirectToPage("ListaPreguntas", new { id = pregunta.encuestaID });
        }
    }
}