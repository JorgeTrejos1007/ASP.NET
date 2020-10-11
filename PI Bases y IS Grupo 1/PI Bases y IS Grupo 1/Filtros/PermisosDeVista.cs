using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PIBasesISGrupo1.Filtros
{
    [AttributeUsage(AttributeTargets.Method,AllowMultiple =false)]
    public class PermisosDeVista:AuthorizeAttribute
    {
    }
}
