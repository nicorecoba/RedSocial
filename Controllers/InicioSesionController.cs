using Biblioteca;
using Microsoft.AspNetCore.Mvc;

namespace Obligatorio2.Controllers
{
    public class InicioSesionController : Controller
    {
        public IActionResult Index()
        {
            string? us = HttpContext.Session.GetString("usuarioLogueado");
            if (us == null)
            {
                return View();
            }
            else
            {
                TempData["MensajeError"] = "Acceso Denegado";
                return RedirectToAction("Index", "Error");
            }
            
        }
        [HttpPost]
        public IActionResult Login(Usuario s)
        {
            
            try
            {                
                Usuario userLogueado = Sistema.ObtenerInstancia.IniciarSesion(s.Email, s.Contraseña);
                HttpContext.Session.SetString("usuarioLogueado", userLogueado.Email);
                HttpContext.Session.SetString("Rol", userLogueado.Rol);
                HttpContext.Session.SetInt32("Id", userLogueado.Id);

                return RedirectToAction("IdentificarRol");
            } catch (Exception ex)
            {
                TempData["MensajeError"] = ex.Message;
                return RedirectToAction("Index");
            }
        }


        public IActionResult IdentificarRol() 
        {            
            string? rol = HttpContext.Session.GetString("Rol");

                if (rol!= null && rol.Equals("Admin")) 
            {

             return RedirectToAction("Index", "Administrador");

            }else if (rol!= null && rol.Equals("Miembro"))
            {
                return RedirectToAction("Inicio", "Miembro");
            } else
            {
                return RedirectToAction("Index");
            }

        }

    }


  
}
