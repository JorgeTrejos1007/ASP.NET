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
        private int nivelDePermisoDeVista;
        
        public PermisosDeVista(int nivel)
        {
            nivelDePermisoDeVista = nivel;


        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {


            string pagina = context.HttpContext.Request.Path.Value;
            Miembro miembroSesionActual=Sesion.obtenerDatosDeSesion(context.HttpContext.Session);

            //context.Result = new RedirectResult("/Curso/ProponerCurso");
            
            //throw new NotImplementedException();
            //context.Result = new RedirectResult(pagina);
        }
    }

}

