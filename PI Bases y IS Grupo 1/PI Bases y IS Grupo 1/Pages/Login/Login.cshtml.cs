using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PIBasesISGrupo1.Models;
using PIBasesISGrupo1.Handler;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;
using System.ComponentModel.DataAnnotations;

namespace PIBasesISGrupo1.Pages.Login
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        [Required(ErrorMessage = "Es necesario que ingreses tu correo")]
        public string email { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Es necesario que ingreses tu contraseña")]
        public string password { get; set; }

        public void OnGet()
        {
            



        }

        public IActionResult OnPost()
        {
            
            MiembroHandler accesoDatos = new MiembroHandler();
            Miembro datosDelmiembro = accesoDatos.obtenerTodosLosMiembros().Find(smodel => smodel.email == email.Trim() && smodel.password== password.Trim());
            IActionResult vista;
            
            if (datosDelmiembro != null)
            {

                Sesion.guardarDatosDeSesion(HttpContext.Session, datosDelmiembro);

                //var datos = Sesion.GetObjectFromJson<Miembro>(HttpContext.Session,"User");

                vista = Redirect("/Index");
                
            }
            else {
                TempData["mensaje"] = "Correo o contraseña incorrecta";
                vista= Page();
            }

            return vista;
        }
    }
}