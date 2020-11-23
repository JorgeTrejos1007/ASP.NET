using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PIBasesISGrupo1.Models;
using PIBasesISGrupo1.Handler;
using PIBasesISGrupo1.MotorSimilitudes;
using Newtonsoft.Json;


namespace PIBasesISGrupo1.Pages.Miembros
{
    public class MostrarPerfilMiembroModel : PageModel
    {      
        [BindProperty]
        public Miembro miembro { get; set; }

        public void OnGet(string email)
        {
            //Si es null es porque es mi perfil
            if (email == null) {
                var miembro = Sesion.obtenerDatosDeSesion(HttpContext.Session, "Miembro");
                email = miembro.email;
            }

            MiembroHandler accesoDatos = new MiembroHandler();
            ViewData["email"] = email;
            miembro = accesoDatos.obtenerDatosDeUnMiembro(email);

            int cantidadDePerfiles = 4;
            List<Miembro> informacionDePerfilesMasSimilares = new List<Miembro>();
            List<string> correosDePerfilesMasSimilares = new List<string>();

            //si es mi perfil
            var perfilActual = Sesion.obtenerDatosDeSesion(HttpContext.Session, "Miembro");
            string emailDelPerfilEnSesion=null;

            if (perfilActual != null) {
                emailDelPerfilEnSesion = perfilActual.email;
            }
            if (email== emailDelPerfilEnSesion) {
                MotorDeSimilitudes motorDeSimilitudes = new MotorDeSimilitudes(miembro.habilidades, cantidadDePerfiles, email);
                correosDePerfilesMasSimilares = motorDeSimilitudes.retorneLosCorreosDePerfilesConHabilidadesSimilaresAMiPerfil();
            }
            else {
                MotorDeSimilitudes motorDeSimilitudes = new MotorDeSimilitudes(miembro.habilidades, miembro.idiomas, miembro.pais, cantidadDePerfiles, email);
                correosDePerfilesMasSimilares = motorDeSimilitudes.retorneCorreosDeLosPerfilesMasSimilares();
            }
            for (int index = 0; index < correosDePerfilesMasSimilares.Count; index++)
            {
                informacionDePerfilesMasSimilares.Add(accesoDatos.obtenerDatosDeUnMiembro(correosDePerfilesMasSimilares[index]));
            }
            ViewData["informacionDePerfilesMasSimilares"] = informacionDePerfilesMasSimilares;
            ViewData["misLikes"] = accesoDatos.obtenerLikesTotalesDeMiembro(email);
        }
        public IActionResult OnPostActualizarLikesDelMiembro(string emailDelPerfilActual)
        {
            string emailMiembroEnSesion = "stevegc112016@gmail.com";
            MiembroHandler accesoMiembro = new MiembroHandler();
            if (!accesoMiembro.darLike(emailMiembroEnSesion,emailDelPerfilActual)) {
                accesoMiembro.darDisLike(emailMiembroEnSesion,emailDelPerfilActual);
            }
            return new JsonResult(emailMiembroEnSesion); ;
        }
    }
    
     

}