using Biblioteca;
using Microsoft.AspNetCore.Mvc;

namespace Obligatorio2.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult Inicio()
        {
            string? rol = HttpContext.Session.GetString("Rol");
            if(rol != null && rol.Equals("Miembro"))
            {
                try
                {

                    string? emailLogueado = HttpContext.Session.GetString("usuarioLogueado");
                    if (emailLogueado != null)
                    {
                        Usuario user = Sistema.ObtenerInstancia.ObtenerMiembroPorEmail(emailLogueado);
                        ViewBag.NombreUsuario = user.ToString();
                    }

                }
                catch (Exception ex)
                {
                    TempData["error"] = ex.Message;
                }
                return View();
            }
            else
            {
                TempData["MensajeError"] = "Error, no puede acceder a la página";
                return RedirectToAction("Index", "Error");
            }
            
            
        }

        
        
        public IActionResult Registrarse()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ValidarRegistro(Miembro m)
        {
            try
            {
                Sistema.ObtenerInstancia.AltaMiembro(m.Email,m.Contraseña,m.Nombre,m.Apellido,m.FechaNacimiento);
                TempData["error"] = false;
                return RedirectToAction("Registrarse");
            } catch (Exception ex)
            {
                TempData["error"] = true;
                TempData["MensajeError"] = ex.Message;
                return RedirectToAction("Registrarse");
            }
           
        }
    }

}
