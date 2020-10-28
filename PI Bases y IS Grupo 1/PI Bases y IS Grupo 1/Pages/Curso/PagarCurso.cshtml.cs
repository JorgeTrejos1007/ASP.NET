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

namespace PIBasesISGrupo1.Pages.Curso
{
    public class PagarCursoModel : PageModel
    {

        [BindProperty]
        public IFormFile comprobante { get; set; }
        [BindProperty]
        public Miembro estudiante { get; set; }
        public IActionResult OnGet()
        {
            ViewData["esMiembro"] = TempData["esMiembro"];
            return Page();
        }
        [HttpPost]
        public IActionResult OnPostAñadirEstudiante(string nombre,string primerApellido,   string segundoApellido, string email,string genero , string nombreCurso, bool esMiembro)
        {
            try
            {
                if (comprobante != null)
                {
                    MiembroHandler accesoDatos = new MiembroHandler();
                    if (esMiembro)
                    {
                        accesoDatos.registrarEstudiante(email);
                        if (accesoDatos.inscribirEstudianteACurso(email, nombreCurso) )
                        {
                            TempData["mensaje"] = "Inscripcion Exitosa";
                            TempData["exitoAlEditar"] = true;
                        }
                        else
                        {
                            TempData["mensaje"] = "Usted ya está Inscrito a este curso";
                            TempData["exitoAlEditar"] = false;
                        }
                        return RedirectToAction("~/Curso/InscribirmeCursos");
                    }
                    else
                    {

                        string codigo = ""+obtenerCodigoTemporal();
                        estudiante.password = new String(codigo);
                        estudiante.nombre = new String(nombre);
                        estudiante.primerApellido = primerApellido;
                        estudiante.segundoApellido = segundoApellido;
                        estudiante.email = email;
                        estudiante.genero = genero;
                        bool estudianteRegistrado = accesoDatos.crearEstudianteComoParticipanteExterno(estudiante);
                        if (estudianteRegistrado) {
                            accesoDatos.registrarEstudiante(email);
                        }
                        if (accesoDatos.inscribirEstudianteACurso(estudiante.email, nombreCurso))
                        {
                            if(estudianteRegistrado)
                                TempData["mensaje"] = "Inscripcion Exitosa, esta es tu contraseña Temporal " + codigo;
                            else
                                TempData["mensaje"] = "Inscripcion Exitosa" ;
                            TempData["exitoAlEditar"] = true;
                        }
                        else
                        {
                            TempData["mensaje"] = "Usted ya esta Inscrito a este Curso";
                            TempData["exitoAlEditar"] = false;
                        }
                    }
                }
            }
            catch
            {

            }
            return RedirectToAction("~/Curso/PagarCurso");
        }

        public string obtenerCodigoTemporal()
        {
            string codigo = "";
            string[] caracteres = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", ".", "_", "-", "?", "!" };
            Random random = new Random();
            for (int caracter = 0; caracter < 4; ++caracter)
            {
                int caracterAleatorio = random.Next(caracteres.Length);
                codigo += caracteres[caracterAleatorio];
            }
            int randomNumber = random.Next(1000000);
            codigo += randomNumber.ToString();
            return codigo;
        }
    }
}