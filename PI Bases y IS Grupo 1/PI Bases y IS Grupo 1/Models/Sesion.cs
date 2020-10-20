using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace PIBasesISGrupo1.Models
{
    public static class Sesion
    {
        public static void guardarDatosDeSesion(this ISession sesionActual, object datosDelmiembro)
        {

            sesionActual.SetString("User", JsonConvert.SerializeObject(datosDelmiembro));
        }

         public static Miembro obtenerDatosDeSesion(this ISession sesionActual)
        {
            var datosDelmiembro = sesionActual.GetString("User");
            
            return datosDelmiembro == null ? default(Miembro) : JsonConvert.DeserializeObject<Miembro>(datosDelmiembro);
        }

        public static void cerrarSesion(this ISession sesionActual)
        {
            sesionActual.Clear();
        }
    }
}
