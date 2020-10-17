﻿using System;
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
        private Miembro estudiante { get; set; }
        public IActionResult OnGet(bool esMiembro)
        {
             
           
            ViewData["esMiembro"] = esMiembro;
            
            
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
                        if (accesoDatos.registrarEstudiante(email))
                        {
                            TempData["mensaje"] = "Se ha logrado inscribir con exito";
                            TempData["exitoAlEditar"] = true;
                            accesoDatos.inscribirEstudianteACurso(email, nombreCurso);
                        }
                        else
                        {
                            TempData["mensaje"] = "Se ha ocurrido un error en el registro";
                            TempData["exitoAlEditar"] = false;
                        }
                        return RedirectToAction("~/Curso/InscribirmeCursos");
                    }
                    else
                    {

                        string codigo = obtenerCodigoDeCurso();
                        estudiante.password = codigo;
                        estudiante.nombre = nombre;
                        estudiante.primerApellido = primerApellido;
                        estudiante.segundoApellido = segundoApellido;
                        estudiante.email = email;
                        estudiante.genero = genero;

                        if (accesoDatos.crearEstudianteComoParticipanteExterno(estudiante))
                        {
                            TempData["mensaje"] = "Se ha logrado inscribir con exito, este es su codigo para el Curso: " + codigo;
                            TempData["exitoAlEditar"] = true;
                            bool exito=  accesoDatos.registrarEstudiante(estudiante.email);
                            accesoDatos.inscribirEstudianteACurso(estudiante.email, nombreCurso);
                        }
                        else
                        {
                            TempData["mensaje"] = "Ha ocurrido un error en el registro";
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

        public string obtenerCodigoDeCurso()
        {
            string codigo = "";
            string[] caracteres = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", ".", "_", "-", "?", "!" };
            Random random = new Random();
            for (int caracter = 0; caracter < 4; ++caracter)
            {
                int caracterAleatorio = random.Next(caracteres.Length);
                codigo += caracteres[caracterAleatorio];
            }
            int randomNumber = random.Next(10000);
            codigo += randomNumber.ToString();
            return codigo;
        }
    }
}