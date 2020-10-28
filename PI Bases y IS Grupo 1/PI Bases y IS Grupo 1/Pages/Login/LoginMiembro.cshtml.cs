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
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

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

        public async Task<IActionResult> OnPostAsync()
        {
            
            MiembroHandler accesoDatos = new MiembroHandler();
            LoginHandler login = new LoginHandler();

            IActionResult vista;
            if (login.validarMiembro(email.Trim(), password.Trim()))
            {
                Miembro datosDelmiembro = accesoDatos.obtenerDatosDeUnMiembro(email.Trim());
                Sesion.guardarDatosDeSesion(HttpContext.Session, datosDelmiembro,"Miembro");
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "Sesion"));
                identity.AddClaim(new Claim(ClaimTypes.Name, "Miembro"));
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
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