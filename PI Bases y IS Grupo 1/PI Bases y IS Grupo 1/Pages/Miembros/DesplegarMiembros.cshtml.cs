using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PIBasesISGrupo1.Models;
using PIBasesISGrupo1.Handler;
using Microsoft.Extensions.Configuration;


namespace PIBasesISGrupo1.Pages.Miembros
{
    public class DesplegarMiembrosModel : PageModel
    {
        [BindProperty]
        public Miembro Miembro { get; set; }
        
        public void OnGet()
        {
            MiembroHandler accesoDatos = new MiembroHandler();
            ViewData["Miembros"] = accesoDatos.obtenerTodosLosMiembros();

            SimilitudPerfilHandler prueba = new SimilitudPerfilHandler();
            string[] habilidades = {"Liderazgo", "Optimismo"};

            string consulta = prueba.crearConsultaTamañoDinamicoHabilidades(habilidades);
            List<string> correos = prueba.extraerCorreosConAlMenosUnaSimilitud(habilidades, consulta);

            string pais = "Costa Rica";
            List<string> resultado = prueba.extraerCorreosConAlMenosUnaSimiltudEnPais(pais);
        }
    }
}