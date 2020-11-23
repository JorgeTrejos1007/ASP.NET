﻿using System;
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
        List<Sector> sectores = new List<Sector>();

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
                DateTime fecha = Convert.ToDateTime(fechaYHora);

                List<Sector> listaSectoresNoNumerados = new List<Sector>();

                List<Sector> listaSectoresNumerados = new List<Sector>();            
                listaSectoresNumerados = baseDeDatosHandler.obtenerSectoresNumeradosEventoPresencial(emailCoordinador, nombreEvento, fecha);
                foreach (var sector in listaSectoresNumerados)
                {
                    sector.asientosDisponibles = baseDeDatosHandler.asientosDisponiblesEnSector(emailCoordinador, nombreEvento, fechaYHora, sector.nombreDeSector);
                }
                ViewData["listaSectoresNumerados"] = listaSectoresNumerados;


                sectores = baseDeDatosHandler.obtenerSectoresEventoPresencial(emailCoordinador, nombreEvento, fecha);

                for (int index = 0; index < sectores.Count; index++)
                {
                    if (sectores[index].tipo == "Numerado")
                    {
                        sectores[index].asientosDisponibles = baseDeDatosHandler.asientosDisponiblesEnSector(emailCoordinador, nombreEvento, fecha, sectores[index].nombreDeSector);
                    }
                }
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
            sector.asientosDisponibles = baseDeDatosHandler.asientosDisponiblesEnSector(emailCoordinador, nombreEvento, fechaYHora, nombreSectorElegido);
            return new JsonResult(sector);
        }

        public IActionResult OnPost () {
            IActionResult vista;

            // datos de prueba
            InformacionDeRegistroEnEvento info = new InformacionDeRegistroEnEvento();
            info.nombreEvento = "Standup comedy con Ronny  se puede quedar 5 minutitos mas";
            info.emailCoordinador = "stevegc112016@gmail.com";
            info.nombreSector = "Altair";
            info.fechaYHora = Convert.ToDateTime("2020-11-27 18:00:00.000");
            info.tipoDeSector = "Numerado";
            //info.cantidadAsientos = 10;
            List<int> asientos = new List<int>();
            asientos.Add(2);
            asientos.Add(4);
            asientos.Add(5);
            info.asientosDeseados = asientos;

            vista = Redirect("~/index");

            if (info.tipoDeSector == "Numerado") {
                var miembro = Sesion.obtenerDatosDeSesion(HttpContext.Session, "Miembro");
                bool exito = baseDeDatosHandler.transaccionReservarAsientosNumerados(info, miembro.email);
                if (exito == false) {
                    vista = Redirect("~/index");
                }
            }
            else
            {
                bool exito = baseDeDatosHandler.transaccionReservarAsientosNoNumerados(info);
                if (exito == false) {
                    vista = Redirect("~/index");
                }
            }

            return vista;
        }
    }
}