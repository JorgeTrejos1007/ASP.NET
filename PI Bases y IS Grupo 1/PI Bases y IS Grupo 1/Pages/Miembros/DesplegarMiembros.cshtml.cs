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
            List<int> valores = new List<int>();
            List<string> idioma = new List<string>();
            idioma.Add("Español");
            idioma.Add("Ingles");
            valores = prueba.extraerFrecuenciaDeIdiomas(idioma);

            List<int> cantidadHabilidades = new List<int>();
            List<string> habilidades = new List<string>();
            habilidades.Add("Liderazgo");
            habilidades.Add("Optimismo");
            cantidadHabilidades = prueba.extraerFrecuenciaDeHabilidades(habilidades);

            int cantidadPais = prueba.extraerFrecuenciaDePais("Costa Rica");
        }
    }
}