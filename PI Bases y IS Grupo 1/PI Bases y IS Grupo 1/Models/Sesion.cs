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
        public static void guardarDatosDeSesion(this ISession session, string key, object value)
        {
            
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

         public static Miembro obtenerDatosDeSesion(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(Miembro) : JsonConvert.DeserializeObject<Miembro>(value);
        }
    }
}
