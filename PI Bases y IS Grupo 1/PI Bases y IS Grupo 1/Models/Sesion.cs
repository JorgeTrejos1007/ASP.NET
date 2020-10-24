using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;

namespace PIBasesISGrupo1.Models
{
    public static class Sesion
    {
        public static void guardarDatosDeSesion(this ISession sesionActual, object datosDelmiembro, string usuario)
        {

            sesionActual.SetString(usuario, JsonConvert.SerializeObject(datosDelmiembro));
        }

         public static Miembro obtenerDatosDeSesion(this ISession sesionActual, string usuario)
        {
            var datosDelmiembro = " ";
            if (usuario != null)
            {

                datosDelmiembro = sesionActual.GetString(usuario);
            }

            return datosDelmiembro == null ? default(Miembro) : JsonConvert.DeserializeObject<Miembro>(datosDelmiembro);
        }

        public static void cerrarSesion(this ISession sesionActual)
        {
            sesionActual.Clear();
           
        }
    }
}
