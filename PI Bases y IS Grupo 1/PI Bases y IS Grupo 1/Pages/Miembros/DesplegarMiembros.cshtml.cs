using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PIBasesISGrupo1.Models;
using PIBasesISGrupo1.Handler;
using PIBasesISGrupo1.MotorSimilitudes;
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
            string[] habilidades = { "Optimismo", "Liderazgo"};
            string[] idiomas = { "Español", "Ingles"};
            string pais="Costa Rica";
            int cantidadDePerfiles = 5;
            string miEmail = "hellenfdz12@gmail.com";
            List<string> perfilesMasSimilares = new List<string>();

            MotorDeSimilitudes motorDeSimilitudes = new MotorDeSimilitudes(habilidades, idiomas, pais, cantidadDePerfiles, miEmail);
            perfilesMasSimilares = motorDeSimilitudes.retorneLosPerfilesMasSimilares();

            for (int i = 0; i < cantidadDePerfiles; i++) {
                Console.WriteLine(perfilesMasSimilares[i]);
            }

        }
    }
}