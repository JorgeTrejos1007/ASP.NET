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
        InformacionDeRegistroEnEvento registro { get; set; }

        public void OnGet(string emailCoordinador, string nombreEvento, string fechaYHora, string lugar)
        {
            DateTime fecha = Convert.ToDateTime(fechaYHora);
            sectores = baseDeDatosHandler.obtenerSectoresEventoPresencial(emailCoordinador, nombreEvento, fecha);

            for (int index = 0; index < sectores.Count; index++) {
                if (sectores[index].tipo == "Numerado") {
                    sectores[index].asientosDisponibles = baseDeDatosHandler.asientosDisponiblesEnSector(emailCoordinador, nombreEvento, fecha, sectores[index].nombreDeSector);
                }
            }
        }

        public void OnPost () {
            // datos de prueba
            InformacionDeRegistroEnEvento info = new InformacionDeRegistroEnEvento();
            info.nombreEvento = "Standup comedy con Ronny  se puede quedar 5 minutitos mas";
            info.emailCoordinador = "stevegc112016@gmail.com";
            info.nombreSector = "Altair";
            info.fechaYHora = Convert.ToDateTime("2020-11-27 18:00:00.000");
            info.tipoDeSector = "Numerado";
            info.cantidadAsientos = 1;
            List<int> asientos = new List<int>();
            asientos.Add(1);
            info.asientosDeseados = asientos;

            if (info.tipoDeSector == "Numerado") {
                var miembro = Sesion.obtenerDatosDeSesion(HttpContext.Session, "Miembro");
                bool exito = baseDeDatosHandler.transaccionReservarAsientosNumerados(info, miembro.email);
            }
            else
            {
                bool exito = baseDeDatosHandler.transaccionReservarAsientosNoNumerados(info);
            }
        }
    }
}