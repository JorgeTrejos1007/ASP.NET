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
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class PermisosDeVista : AuthorizeAttribute, IAuthorizationFilter 
    {
        private int nivelDePermisoDeVista;
        
        public PermisosDeVista(int nivel)
        {
            nivelDePermisoDeVista = nivel;


        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            

            Miembro miembroSesionActual=Sesion.obtenerDatosDeSesion(context.HttpContext.Session);

            //throw new NotImplementedException();
            context.Result = new RedirectResult("/Accounts/Login");
        }
    }

}

