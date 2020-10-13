using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using PIBasesISGrupo1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PIBasesISGrupo1.Filtros
{
    [AttributeUsage(AttributeTargets.Method,AllowMultiple =false)]
    public class PermisosDeVista : AuthorizeAttribute, IAuthorizationFilter
    {
        private int nivelDePermisoDeVista;
        
        public PermisosDeVista(int nivelDePermisoDeVista)
        {
            this.nivelDePermisoDeVista = nivelDePermisoDeVista;

            


        }

        public  void OnAuthorization(AuthorizationFilterContext context)
        {
            throw new NotImplementedException();

          

        }
    }
}
