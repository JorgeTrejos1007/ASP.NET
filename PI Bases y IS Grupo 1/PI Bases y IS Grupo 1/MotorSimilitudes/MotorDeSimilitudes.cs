using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using PIBasesISGrupo1.Models;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Web;
using Microsoft.AspNetCore.Http;
using PIBasesISGrupo1.Handler;
using System.Linq;

namespace PIBasesISGrupo1.MotorSimilitudes
{
    public class MotorDeSimilitudes
    {
        SimilitudPerfilHandler consultasParaSimilitudes = new SimilitudPerfilHandler();
        List<double> pesosIdioma = new List<double>();
        List<double> pesosHabilidad = new List<double>();
        List<string> correosConSimilitudEnIdiomas = new List<string>();
        List<string> correosConSimilitudEnHabilidades = new List<string>();
        List<string> correosConSimilitudEnPais = new List<string>();
        double pesoPais = 0;

        string[] habilidades;
        string[] idiomas;
        string pais;
        int cantidadPerfiles = 0;
        string miEmail;

        public MotorDeSimilitudes(string[] habilidades, string[] idiomas, string pais, int cantidadPerfiles, string miEmail) {
            this.habilidades = habilidades;
            this.idiomas = idiomas;
            this.pais = pais;
            this.cantidadPerfiles = cantidadPerfiles;
            this.miEmail = miEmail;
        }


        public List<string> retorneLosPerfilesMasSimilares()
        {
            List<string> topPerfilesMasSimilares = new List<string>();
            List<double> significanciaDePerfilPorIdioma = new List<double>();
            List<double> significanciaDePerfilPorHabilidad = new List<double>();
            Dictionary<string, double> semejanzaDePerfiles = new Dictionary<string, double>();

            calcularPesosDeAtributos();
            extraerCorreosDePerfilesSimilares();

            significanciaDePerfilPorIdioma = calculoDeSimilitudSegunElCriterio(idiomas, correosConSimilitudEnIdiomas, pesosIdioma, "Idiomas", "idiomaPK");
            significanciaDePerfilPorHabilidad = calculoDeSimilitudSegunElCriterio(habilidades, correosConSimilitudEnHabilidades, pesosHabilidad, "Habilidades", "habilidadPK");

            for (int iterador = 0; iterador < correosConSimilitudEnIdiomas.Count; iterador++) {
                semejanzaDePerfiles.Add(correosConSimilitudEnIdiomas[iterador], significanciaDePerfilPorIdioma[iterador]);
            }

            for (int iterador = 0; iterador < correosConSimilitudEnHabilidades.Count; iterador++)
            {
                if (semejanzaDePerfiles.ContainsKey(correosConSimilitudEnHabilidades[iterador])) {
                    double nuevoValor = semejanzaDePerfiles[correosConSimilitudEnHabilidades[iterador]];
                    nuevoValor += significanciaDePerfilPorHabilidad[iterador];
                    semejanzaDePerfiles[correosConSimilitudEnHabilidades[iterador]] = nuevoValor;
                }
                else {
                    semejanzaDePerfiles.Add(correosConSimilitudEnHabilidades[iterador], significanciaDePerfilPorHabilidad[iterador]);
                }
            }

            for (int iterador = 0; iterador < correosConSimilitudEnPais.Count; iterador++)
            {
                if (semejanzaDePerfiles.ContainsKey(correosConSimilitudEnPais[iterador]))
                {
                    double nuevoValor = semejanzaDePerfiles[correosConSimilitudEnPais[iterador]];
                    nuevoValor += pesoPais;
                    semejanzaDePerfiles[correosConSimilitudEnPais[iterador]] = nuevoValor;
                }
                else
                {
                    semejanzaDePerfiles.Add(correosConSimilitudEnPais[iterador], pesoPais);
                }
            }

            
            semejanzaDePerfiles.Remove(miEmail);

            var items = from pair in semejanzaDePerfiles
                        orderby pair.Value descending
                        select pair;

            int contador = 0;
            foreach (KeyValuePair<string, double> pair in items)
            {
                if (contador < cantidadPerfiles)
                {
                    topPerfilesMasSimilares.Add(pair.Key);
                    contador = contador+1;
                }
                else {
                    break;
                }
            }

            return topPerfilesMasSimilares;
        }

        public void calcularPesosDeAtributos () {
            pesosIdioma = consultasParaSimilitudes.extraerPesoDeIdiomas(idiomas);
            pesosHabilidad = consultasParaSimilitudes.extraerPesoDeHabilidades(habilidades);
            pesoPais = consultasParaSimilitudes.extraerPesoDePaises(pais);
        }

        public void extraerCorreosDePerfilesSimilares () {
            string consulta = consultasParaSimilitudes.crearConsultaTamanoDinamicoIdiomas(idiomas);
            correosConSimilitudEnIdiomas = consultasParaSimilitudes.extraerCorreosConAlMenosUnaSimilitud(idiomas, consulta);
            consulta = consultasParaSimilitudes.crearConsultaTamañoDinamicoHabilidades(habilidades);
            correosConSimilitudEnHabilidades = consultasParaSimilitudes.extraerCorreosConAlMenosUnaSimilitud(habilidades, consulta);
            correosConSimilitudEnPais = consultasParaSimilitudes.extraerCorreosConSimiltudEnPais(pais);
        }

        public List<double> calculoDeSimilitudSegunElCriterio (string[] criterioDeBusqueda, List<string> correosConSimilitud, List<double> pesosDelCriterio, string nombreTabla, string columnaDeBusqueda) {
            List<double> similitudPorCorreo = new List<double>();
            double sumaDelCriterio = 0.0;
            double seEncuentraElAtributo = 0.0;
            
            for (int correo = 0; correo < correosConSimilitud.Count; correo++)
            {
                for (int criterio = 0; criterio < criterioDeBusqueda.Length; criterio++)
                {
                    seEncuentraElAtributo = consultasParaSimilitudes.revisarSiTieneElAtributoSegunCorreo(nombreTabla, criterioDeBusqueda[criterio], columnaDeBusqueda, correosConSimilitud[correo]);
                    sumaDelCriterio += pesosDelCriterio[criterio] * seEncuentraElAtributo;                   
                }
                similitudPorCorreo.Add(sumaDelCriterio);
                sumaDelCriterio = 0.0;
            }
            return similitudPorCorreo;
        }
    }
}
