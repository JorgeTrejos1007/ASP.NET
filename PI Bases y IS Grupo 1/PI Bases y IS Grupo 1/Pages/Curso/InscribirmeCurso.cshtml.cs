using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PIBasesISGrupo1.Models;
using PIBasesISGrupo1.Handler;
using Microsoft.AspNetCore.Http;


namespace PIBasesISGrupo1.Pages.Curso
{
    public class InscribirmeCursoModel : PageModel
    {
        [BindProperty]
        public Miembro miembro { get; set; }
        [BindProperty]
        public IFormFile archivoImagen { get; set; }
        public string[] idiomas = new string[73]
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
        public string[] habilidades = new string[17]
               {
                    "Empatia","Saber escuchar","Liderazgo", "Flexibilidad","Optimismo", "Confianza","Honestidad",
                    "Paciencia","Comunicacion","Convencimiento","Esfuerzo", "Esfuerzo","Dedicacion","Comprension",
                    "Comprension", "Asertividad", "Credibilidad"

               };

        public void OnGet()
        {
            try
            {
                ViewData["idiomas"] = idiomas;
                ViewData["habilidades"] = habilidades;
            }
            catch
            {
                Redirect("~/Curso/CrearMiembro");
            }

        }

        public void OnPost()
        {

            try
            {
                MiembroHandler accesoDatos = new MiembroHandler();
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
            catch
            {

                TempData["mensaje"] = "Se ha ocurrido un error en el registro";
                TempData["exitoAlEditar"] = false;
            }



        }
    }
}