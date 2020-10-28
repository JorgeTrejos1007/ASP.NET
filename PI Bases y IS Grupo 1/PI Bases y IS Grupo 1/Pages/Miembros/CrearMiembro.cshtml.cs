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
        public string[] paises = new string[194] {
            "Afganistan", "Albania", "Alemania" ,"Andorra","Angola" ,"Antigua y Barbuda" ,"Arabia Saudita" , "Argelia",
            "Argentina" ,"Armenia" ,"Australia","Austria","Azerbaiyan","Bahamas","Banglades" ,"Barbados", "Barein",
            "Belgica","Belice","Benin","Bielorrusia","Birmania","Bolivia","Bosnia y Herzegovina","Botsuana","Brasil","Brunei",
            "Bulgaria", "Burkina Faso","Burundi","Butan","Cabo Verde","Camboya","Camerun","Canada","Catar","Chad","Chile","China",
            "Chipre","Ciudad del Vaticano","Colombia","Comoras","Corea del Norte","Corea del Sur","Costa de Marfil","Costa Rica",
            "Croacia","Cuba","Dinamarca","Dominica","Ecuador","Egipto","El Salvador","Emiratos Arabes Unidos","Eritrea","Eslovaquia",
            "Eslovenia","España","Estados Unidos","Estonia","Etiopia","Filipinas","Finlandia", "Fiyi","Francia","Gabon","Gambia","Georgia",
            "Ghana","Granada","Grecia","Guatemala","Guyana","Guinea", "Guinea ecuatorial","Guinea-Bisau","Haiti","Honduras","Hungria","India",
            "Indonesia","Irak","Iran","Irlanda","Islandia","Islas Marshall","Islas Salomon","Israel","Italia","Jamaica","Japon","Jordania",
            "Kazajistan","Kenia","Kirguistan","Kiribati","Kuwait","Laos","Lesoto","Letonia","Libano","Liberia","Libia","Liechtenstein","Lituania",
            "Luxemburgo","Macedonia del Norte", "Madagascar","Malasia","Malaui","Maldivas","Mali","Malta", "Marruecos",  "Mauricio", "Mauritania",
            "Mexico","Micronesia","Moldavia", "Monaco","Mongolia","Montenegro","Mozambique","Namibia", "Nauru","Nepal","Nicaragua","Niger","Nigeria",
            "Noruega","Nueva Zelanda","Oman","Paises Bajos","Pakistan","Palaos","Panama","Nueva Guinea","Paraguay","Peru","Polonia","Portugal",
            "Reino Unido","Republica Centroafricana","Republica Checa","Republica del Congo","Republica Democratica del Congo","Republica Dominicana",
            "Republica Sudafricana","Ruanda","Rumania","Rusia","Samoa","San Cristobal y Nieves","San Marino","San Vicente y las Granadinas",
            "Santa Lucia","Santo Tome y Principe","Senegal","Serbia","Seychelles","Sierra Leona","Singapur","Siria","Somalia","Sri Lanka","Suazilandia",
            "Sudan","Sudan del Sur","Suecia","Suiza","Surinam","Tailandia","Tanzania","Tayikistan","Timor Oriental","Togo","Tonga","Trinidad y Tobago","Tunez",
            "Turkmenistan","Turquia","Tuvalu","Ucrania","Uganda","Uruguay","Uzbekistan","Vanuatu","Venezuela", "Vietnam","Yemen","Yibuti","Zambia","Zimbabue"
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
                ViewData["paises"] = paises;
            }
            catch
            {
                vista = Redirect("~/Curso/CrearMiembro");
            }
            return vista;
            
          
    }
        
        public IActionResult OnPost(bool esEstudiante)
        {
      
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