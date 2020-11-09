using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PIBasesISGrupo1.Models;
using PIBasesISGrupo1.Handler;
using PIBasesISGrupo1.MotorSimilitudes;

namespace PIBasesISGrupo1.Pages.Miembros
{
    public class MostrarPerfilMiembroModel : PageModel
    {      
        [BindProperty]
        public Miembro miembro { get; set; }

        public void OnGet(string email)
        {
            if (email == null) {
                var miembro = Sesion.obtenerDatosDeSesion(HttpContext.Session, "Miembro");
                email = miembro.email;
            }

            MiembroHandler accesoDatos = new MiembroHandler();
            ViewData["email"] = email;
            miembro = accesoDatos.obtenerDatosDeUnMiembro(email);

            int cantidadDePerfiles = 4;
            MotorDeSimilitudes motorDeSimilitudes = new MotorDeSimilitudes(miembro.habilidades, miembro.idiomas, miembro.pais, cantidadDePerfiles, email);
            List<string > correosDePerfilesMasSimilares = motorDeSimilitudes.retorneCorreosDeLosPerfilesMasSimilares();
            List<Miembro> informacionDePerfilesMasSimilares = new List<Miembro>();

            for (int index = 0; index < correosDePerfilesMasSimilares.Count; index++) {
                informacionDePerfilesMasSimilares.Add(accesoDatos.obtenerDatosDeUnMiembro(correosDePerfilesMasSimilares[index]));
            }

            ViewData["informacionDePerfilesMasSimilares"] = motorDeSimilitudes.retorneCorreosDeLosPerfilesMasSimilares();
        }
    }    
}