using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PIBasesISGrupo1.Models;
using PIBasesISGrupo1.Handler;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace PIBasesISGrupo1.Pages.Miembros
{
    public class CrearMiembroModel : PageModel
    {

        [BindProperty]        
        public Miembro miembro { get; set; }
        [BindProperty]
        public IFormFile archivoImagen { get; set; }
        [BindProperty]
        public string codigoDeCurso { get; set; }
     MiembroHandler accesoDatos = new MiembroHandler();
        
        public string [] idiomas= new string[73]
                  { "Azeri", "Afrikaans", "Albanes", "Aleman", "Alsaciano",
                        "Anglosajon", "Arabe", "Aragones", "Armenio", "Asturiano", "Aymara", "Bengali", "Bielorruso",
                        "Birmano", "Bosnio", "Breton","Bulgaro", "Canares", "Catalan", "Chamorro", "Checo", "Cheroqui",
                        "Chino", "Coreano", "Corso", "Croata", "Curdo", "Danes", "Eslovaco", "Esloveno", "Español", "Esperanto",
                        "Estonio","Euskera", "Feroes", "Fiyiano", "Finlandes", "Frances", "Frison", "Gales", "Gallego", "Georgiano",
                        "Griego", "Guarani", "Gujarati","Hebreo", "Hindi", "Holandes", "Hungaro", "Ido","Indonesio", "Ingles",
                        "Irlandes", "Islandes", "Italiano", "Japones", "Javanes","Latin", "Lituano", "Luxemburgues", "Macedonio",
                        "Noruego", "Occitano", "Papiamento", "PersaPolaco", "Portugues", "Rumano","Ruso",
                        "Serbio", "Somali", "Sueco", "Tailandes", "Turco"
                  };
        public string [] habilidades= new string[17]
               {
                    "Empatia","Saber escuchar","Liderazgo", "Flexibilidad","Optimismo", "Confianza","Honestidad",
                    "Paciencia","Comunicacion","Convencimiento","Esfuerzo", "Esfuerzo","Dedicacion","Comprension",
                    "Comprension", "Asertividad", "Credibilidad"

               };

        public IActionResult OnGet()
        {
            ViewData["estudiante"] = Sesion.obtenerDatosDeSesion(HttpContext.Session, "Estudiante");
            IActionResult vista;
            try
            {
                vista = Page();
                ViewData["idiomas"] = idiomas;
                ViewData["habilidades"] = habilidades;
            }
            catch
            {
                vista = Redirect("~/Curso/CrearMiembro");
            }
            return vista;
            
          
    }
        
        public IActionResult OnPost(bool esEstudiante)
        {
            if (esEstudiante)
            {
                intentarRegistrarEstudiante();
            }
            else
            {
                intentarRegistrarParticipanteExterno();

            }
            try
            {
                if (esEstudiante) {
                    intentarRegistrarEstudiante();
                }
                else {
                    intentarRegistrarParticipanteExterno();
                     
                }

            }
            catch {

                TempData["mensaje"] = " Ha ocurrido un error en el registro";
                TempData["exitoAlEditar"] = false;
            }
            return RedirectToAction("~/Miembros/CrearMiembro");



        }
        private void intentarRegistrarEstudiante()
        {
            if (accesoDatos.resgistrarEstudianteALaComunidad(miembro))
            {
                TempData["mensaje"] = "Se ha logrado registar con exito";
                TempData["exitoAlEditar"] = true;
                if (archivoImagen != null)
                {
                    accesoDatos.actualizarImagen(miembro.email, archivoImagen);
                }
            }
            else
            {
                TempData["mensaje"] = "Ha ocurrido un error en el registro";
                TempData["exitoAlEditar"] = false;
            }


        }
        private void intentarRegistrarParticipanteExterno() {
            if (accesoDatos.crearMiembro(miembro))
            {
                TempData["mensaje"] = "Se ha logrado registar con exito";
                TempData["exitoAlEditar"] = true;
                if (archivoImagen != null)
                {
                    accesoDatos.actualizarImagen(miembro.email, archivoImagen);
                }
            }
            else
            {
                TempData["mensaje"] = "Se ha ocurrido un error en el registro";
                TempData["exitoAlEditar"] = false;
            }
           

        }
         
       
    }
}