using Biblioteca;
using Microsoft.AspNetCore.Mvc;

namespace Obligatorio2.Controllers
{
    public class AdministradorController : Controller
    {

        public IActionResult Index()
        {
            string? rol = HttpContext.Session.GetString("Rol");
            if (rol != null && rol.Equals("Admin"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index","Error");
            }
            
        }

      

        public IActionResult ListarMiembros()
        {
            string? rol = HttpContext.Session.GetString("Rol");
            if (rol != null && rol.Equals("Admin"))
            {
                try
                {
                    
                    List<Miembro> miembros = Sistema.ObtenerInstancia.MostrarMiembrosParaAdmin();
                    return View(miembros);
                }catch
                {
                    TempData["MensajeError"] = "No hay miembros para mostrar";
                }
                return View();
            }
            else
            {
                TempData["error"] = "No tiene permitido acceder a la página";
                return View("Index","Error");
            }
        }
        public IActionResult BloquearODesbloquear(string Email, string opc)
        {
            string? rol = HttpContext.Session.GetString("Rol");
            if(rol != null && rol.Equals("Admin"))
            {
                try
                {
                    string? user = HttpContext.Session.GetString("usuarioLogueado");
                    
                    if (user != null)
                    {
                        Usuario us = Sistema.ObtenerInstancia.ObtenerMiembroPorEmail(user);
                        Usuario usMiembro = Sistema.ObtenerInstancia.ObtenerMiembroPorEmail(Email);
                        if(us is Administrador && usMiembro is Miembro)
                        {
                            Administrador admin = (Administrador)us;
                            Miembro m = (Miembro)usMiembro;
                            if (opc.Equals("BLOQUEAR"))
                            {
                                admin.Bloquear(m);

                            }
                            else
                            {
                                admin.Desbloquear(m);
                            }

                        }
                    }
                }
                catch
                {

                }
                return RedirectToAction("ListarMiembros");
            }else
            {
                TempData["MensajeError"] = "No tiene permitido acceder a la página";
                return RedirectToAction("Index", "Error");
            }
        }

        public IActionResult ListarPublicaciones()
        {
            string? rol = HttpContext.Session.GetString("Rol");
            if(rol != null && rol.Equals("Admin"))
            {
                List<Post> posts = Sistema.ObtenerInstancia.MostrarPostParaAdmin();
                return View(posts);
            }
            else
            {
                TempData["error"] = "No tiene permitido acceder a la página";
                return RedirectToAction("Index", "Error");
            }
        }

        public IActionResult ListarComentarios(int id)
        {
            string ? rol = HttpContext.Session.GetString("Rol");
            if(rol != null && rol.Equals("Admin"))
            {
                try
                {
                   
                    Post p = Sistema.ObtenerInstancia.ObtenerPostPorId(id);
                    List<Comentario> c = p.Comentarios;
                    ViewBag.Post = p.Titulo;
                    return View(c);
                }
                catch
                {
                    TempData["MensajeError"] = "No hay comentarios en esta publicacion";
                }
                return View();
            }
            else
            {
                TempData["MensajeError"] = "Acceso Denegado";
                return RedirectToAction("Index", "Error");
            }

        }

        public IActionResult BanearPublicaciones(int id, string opc)
        {
            string? rol = HttpContext.Session.GetString("Rol");
            if (rol != null && rol.Equals("Admin"))
            {
                try
                {
                    string? user = HttpContext.Session.GetString("usuarioLogueado");

                    if (user != null)
                    {
                        Usuario us = Sistema.ObtenerInstancia.ObtenerMiembroPorEmail(user);
                        Post p = Sistema.ObtenerInstancia.ObtenerPostPorId(id);
                        if (us is Administrador)
                        {
                            Administrador admin = (Administrador)us;
                            
                            if (opc.Equals("BLOQUEAR"))
                            {
                                admin.Censurar(p);
                                
                            }
                            else
                            {
                                admin.DesCensurar(p);
                            }

                        }
                        return RedirectToAction("ListarPublicaciones");
                    }
                }
                catch
                {

                }
                return RedirectToAction("ListarMiembros");
            }
            else
            {
                TempData["MensajeError"] = "No tiene permitido acceder a la página";
                return RedirectToAction("Index", "Error");
            }
        }
    }
}
