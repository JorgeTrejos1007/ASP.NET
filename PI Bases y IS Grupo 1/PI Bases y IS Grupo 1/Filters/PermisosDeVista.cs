using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using PIBasesISGrupo1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;

namespace PIBasesISGrupo1.Filters
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple = false)]
    public class PermisosDeVista : AuthorizeAttribute, IAuthorizationFilter 
    {
        
        private string[] rolesPerimitidos;
        public PermisosDeVista(params string[] roles)
        {

            rolesPerimitidos = roles;

            //nivelDePermisoDeVista = nivel;


        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            
             Miembro miembroSesionActual = Sesion.obtenerDatosDeSesion(context.HttpContext.Session);


            //context.Result = new RedirectResult("~/Error/UnauthorizedOperation?operacion");
            if (rolesPerimitidos.Contains(miembroSesionActual.tipoDeUsuario)==false) {
                context.Result = new RedirectResult("~/Error/PermisosInsuficientes");
            }







            //throw new NotImplementedException();

        }
    }

}

