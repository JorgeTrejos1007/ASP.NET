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
        public static void guardarDatosDeSesion(this ISession session, object value)
        {
            
            session.SetString("User", JsonConvert.SerializeObject(value));
        }

         public static Miembro obtenerDatosDeSesion(this ISession session)
        {
            var value = session.GetString("User");
            return value == null ? default(Miembro) : JsonConvert.DeserializeObject<Miembro>(value);
        }
    }
}
