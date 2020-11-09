using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using PIBasesISGrupo1.Handler;
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


            

        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            FilterHandler filtros = new FilterHandler();

            

            try
            {
                Miembro miembroSesionActual = Sesion.obtenerDatosDeSesion(context.HttpContext.Session, "Miembro");
              

                if (filtros.verificarRol(miembroSesionActual.email, rolesPerimitidos) == false)
                {
                    context.Result = new RedirectResult("~/ErrorDePermisosInsuficientes");
                }
            }
            catch {
                context.Result = new RedirectResult("~/Login/LoginMiembro");
            }
        }
    }

}

