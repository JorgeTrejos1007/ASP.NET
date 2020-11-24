using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PIBasesISGrupo1.Handler;
using PIBasesISGrupo1.Models;

namespace PIBasesISGrupo1.Pages.Eventos
{
    public class RegistrarmeEnEventoPresencialNumeradoModel : PageModel
    {

        EventoHandler baseDeDatosHandler = new EventoHandler();

        [BindProperty]
        public InformacionDeRegistroEnEvento registro { get; set; }

        public IActionResult OnGet()
        {
            IActionResult vista;
            try {
                string emailCoordinador = (string)TempData["emailCoordinador"];
                string nombreEvento = (string)TempData["nombreEvento"];
                DateTime fechaYHora = (DateTime)TempData["fechaEvento"];
                string lugar = (string)TempData["lugarEvento"];
                ViewData["nombreEvento"] = nombreEvento;
                ViewData["fechaYHora"] = fechaYHora;
                ViewData["lugarEvento"] = lugar;
                ViewData["emailCoordinador"] = emailCoordinador;
                List<Sector> listaSectoresNoNumerados = new List<Sector>();
                listaSectoresNoNumerados = baseDeDatosHandler.obtenerSectoresNoNumeradosEventoPresencial(emailCoordinador,nombreEvento,fechaYHora);
                ViewData["listaSectoresNoNumerados"] = listaSectoresNoNumerados;

                List<Sector> listaSectoresNumerados = new List<Sector>();            
                listaSectoresNumerados = baseDeDatosHandler.obtenerSectoresNumeradosEventoPresencial(emailCoordinador, nombreEvento, fechaYHora);
                ViewData["listaSectoresNumerados"] = listaSectoresNumerados;


                
                vista = Page();
            }
            catch
            {
                vista = Redirect("~/Error");
            }
            return vista;
        }

        public IActionResult OnPostElegirAsientos(string nombreSectorElegido, string nombreEvento, string emailCoordinador, DateTime fechaYHora)
        {
            Sector sector = new Sector();
            sector.asientosDisponibles = baseDeDatosHandler.asientosDisponiblesEnSectorNumerado(emailCoordinador, nombreEvento, fechaYHora, nombreSectorElegido);
            return new JsonResult(sector);
        }

        public IActionResult OnPostRegistrarmeEnElEvento() {
            IActionResult vista;

            if(registro.tipoDeSector == "Numerado") {
                registro.asientosDeseados = convertirAsientosElegidosALista(registro.asientosElegidos);
            }
            

            vista = Redirect("~/index");

            if (registro.tipoDeSector == "Numerado") {
                var miembro = Sesion.obtenerDatosDeSesion(HttpContext.Session, "Miembro");
                bool exito = baseDeDatosHandler.transaccionReservarAsientosNumerados(registro, miembro.email);
                if (exito == false) {
                    vista = Redirect("~/index");
                }
            }
            else
            {
                bool exito = baseDeDatosHandler.transaccionReservarAsientosNoNumerados(registro);
                if (exito == false) {
                    vista = Redirect("~/index");
                }
            }

            return vista;
        }

        public List<int> convertirAsientosElegidosALista(string asientos)
        {
            List<int> asientosDisponibles = new List<int>();
            string[] arregloAsientos = asientos.Split(",");

            for(int i = 0; i < arregloAsientos.Length; i++)
            {
                asientosDisponibles.Add(Convert.ToInt32(arregloAsientos[i]));
            }

            return asientosDisponibles;
        }
    }
}