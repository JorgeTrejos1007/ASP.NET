﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PIBasesISGrupo1.Models;
using PIBasesISGrupo1.Handler;

namespace PIBasesISGrupo1.Pages
{
    public class NoticiaModel : PageModel
    {
        public void OnGet()
        {
            NoticiaHandler accesoNoticias = new NoticiaHandler();
            ViewData["Noticias"] = accesoNoticias.obtenerTodasLasNoticias();
        }
    }
}