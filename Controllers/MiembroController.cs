    using Biblioteca;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Obligatorio2.Controllers
{
    public class MiembroController : Controller
    {

        public IActionResult ListarPostSeleccionados(string texto, int valorAceptacion)
        {
              try
              {
                List<Publicacion> retorno = new List<Publicacion>();

                retorno = Sistema.ObtenerInstancia.ListarPost(texto, valorAceptacion);

                return View(retorno);

            }
            catch (Exception ex)
            {
                TempData["errorListar"] = ex.Message;
                return RedirectToAction("VerPost"); 

            }

            
        }




        public IActionResult VerPost()
        {
            string? rol = HttpContext.Session.GetString("Rol");
            if (rol != null && rol.Equals("Miembro"))
            {
                try
                {

                    //producto p = Sistema.ObtenerInstancia.ObtenerMiembroPorId(id);
                    //ViewBag.RutaImagen = "Images" + p.nombreImagen;

                    string? emailLogueado = HttpContext.Session.GetString("usuarioLogueado");

                    if (emailLogueado != null)
                    {
                        Usuario user = Sistema.ObtenerInstancia.ObtenerMiembroPorEmail(emailLogueado);
                        List<Post> postMiembro = Sistema.ObtenerInstancia.ListarPostsMiembro(user);
                        
                        return View(postMiembro);


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

        public IActionResult Inicio()
        {

            string? rol = HttpContext.Session.GetString("Rol");
            if (rol != null && rol.Equals("Miembro"))
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


        public IActionResult Logout()
        {

            string? rol = HttpContext.Session.GetString("Rol");
            if (rol != null && rol.Equals("Miembro") || rol.Equals("Admin"))
            {
                try
                {

                    string? emailLogueado = HttpContext.Session.GetString("usuarioLogueado");
                    if (emailLogueado != null)
                    {
                        Usuario user = Sistema.ObtenerInstancia.ObtenerMiembroPorEmail(emailLogueado);
                        ViewBag.NombreUsuario = user.ToString();

                        HttpContext.Session.Clear();

                    }

                }
                catch (Exception ex)
                {
                    TempData["error"] = ex.Message;
                }

                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["MensajeError"] = "Error, no puede acceder a la página";
                return RedirectToAction("Index", "Error");
            }
        }


        //----------------------------------------------------------------------------------------------------------------------------------------

        //Solicitudes Pendientes

        public IActionResult VerSolicitudesPendiente()
        {
            string? rol = HttpContext.Session.GetString("Rol");
            if (rol != null && rol.Equals("Miembro"))
            {
                try
                {
                    string? usuarioLogueado = HttpContext.Session.GetString("usuarioLogueado");
                    if (usuarioLogueado != null)
                    {
                        Usuario us = Sistema.ObtenerInstancia.ObtenerMiembroPorEmail(usuarioLogueado);
                        if (us is Miembro)
                        {
                            Miembro m = (Miembro)us;
                            List<Miembro> solicitudesPendientes = m.MostrarPendientes();
                            return View(solicitudesPendientes);
                        }
                    }
                } catch {
                    TempData["MensajeError"] = "Lo siento no hay solicitudes pendiente";
                }
                return View();
            }
            else
            {
                TempData["MensajeError"] = "Error, no puede acceder a la página";
                return RedirectToAction("Index", "Error");
            }
        }
        //----------------------------Aceptar Solicitud---------------------------------------------------
        [HttpGet]
        public IActionResult AceptarORechazarSolitud(string email, string opcion)
        {
            string? rol = HttpContext.Session.GetString("Rol");
            if (rol != null && rol.Equals("Miembro"))
            {
                try
                {
                    string? usuarioLogueado = HttpContext.Session.GetString("usuarioLogueado");
                    if (usuarioLogueado != null)
                    {
                        Usuario us = Sistema.ObtenerInstancia.ObtenerMiembroPorEmail(usuarioLogueado);
                        if (us is Miembro)
                        {
                            Miembro miembro = (Miembro)us;
                            Usuario usAceptar = Sistema.ObtenerInstancia.ObtenerMiembroPorEmail(email);
                            if (usAceptar is Miembro) {
                                Miembro m = (Miembro)usAceptar;
                                
                                if (opcion.Equals("ACEPTAR"))
                                {
                                    
                                    miembro.AceptarSolicitud(m);
                                    TempData["error"] = false;

                                    TempData["Mensaje"] = $"Ha agregado con éxito a {m.Nombre} {m.Apellido}";
                                }
                                else if (opcion.Equals("RECHAZAR"))
                                {
                                    
                                    miembro.RechazarSolicitud(m);
                                    TempData["error"] = false;
                                    TempData["Mensaje"] = $"Ha rechazado la solicitud de {m.Nombre} {m.Apellido}";

                                }

                                return RedirectToAction("VerSolicitudesPendiente");
                            }

                        }
                    }
                } catch (Exception e)
                {
                    TempData["error"] = true;
                    TempData["Mensaje"] = e.Message;
                    return RedirectToAction("VerSolicitudesPendiente");
                }
            }
            else
            {
                
                TempData["MensajeError"] = "No tiene permitido acceder a la página";
                return RedirectToAction("Index", "Error");
            }
            return RedirectToAction("VerSolicitudesPendiente");
        }
        //---------------------------------------------------------------------------------------------------------------------------------

        //----------------------------------------------------------------------------------------------------------------------------------
        public IActionResult RealizarPost()
        {

            string? rol = HttpContext.Session.GetString("Rol");
            if (rol != null && rol.Equals("Miembro"))
            {

                try
                {
                    string? usuarioLogueado = HttpContext.Session.GetString("usuarioLogueado");
                    if (usuarioLogueado != null)
                    {
                        Usuario us = Sistema.ObtenerInstancia.ObtenerMiembroPorEmail(usuarioLogueado);

                        if (us is Miembro)
                        {
                            Miembro m = (Miembro)us;

                            return View(m);
                        }
                    }
                }
                catch
                {
                    TempData["MensajeError"] = "No puede realizar un post";
                    return View();
                }
                return View();

            }
            else
            {
                TempData["MensajeError"] = "Error, no puede acceder a la página";
                return RedirectToAction("Index", "Error");
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------
        public IActionResult MostrarMiembros()
        {
            string? rol = HttpContext.Session.GetString("Rol");
            if (rol != null && rol.Equals("Miembro")) {
                try
                {
                    string? userLogueado = HttpContext.Session.GetString("usuarioLogueado");
                    if (userLogueado != null)
                    {
                        Usuario us = Sistema.ObtenerInstancia.ObtenerMiembroPorEmail(userLogueado);
                        if (us is Miembro)
                        {
                            Miembro m = (Miembro)us;
                            List<Miembro> miembros = Sistema.ObtenerInstancia.MostrarMiembros(m);
                            return View(miembros);
                        }
                    }
                } catch
                {
                    return View();
                }
            }
            else
            {
                TempData["MensajeError"] = "No tiene autorización para acceder a la página";
                return RedirectToAction("Index", "Error");
            }
            return View();
        }


        //----------------------------------------------------------------------------------------------------------------------------------
        public IActionResult EnviarInvitacion(string email)
        {
            string? rol = HttpContext.Session.GetString("Rol");
            if (rol != null && rol.Equals("Miembro"))
            {
                try
                {
                    string? userLogueado = HttpContext.Session.GetString("usuarioLogueado");
                    if (userLogueado != null)
                    {
                        Usuario us = Sistema.ObtenerInstancia.ObtenerMiembroPorEmail(userLogueado);
                        Usuario usRecibeInvitacion = Sistema.ObtenerInstancia.ObtenerMiembroPorEmail(email);
                        if (us is Miembro && usRecibeInvitacion is Miembro)
                        {
                            Miembro m = (Miembro)us;
                            Miembro mRecibeInvitacion = (Miembro)usRecibeInvitacion;
                            Sistema.ObtenerInstancia.CrearVinculo(m, mRecibeInvitacion);
                            TempData["Mensaje"] = $"Se ha enviado con exito la solicitud de amistad a {mRecibeInvitacion.Nombre} {mRecibeInvitacion.Apellido}";

                        }
                    }
                    return RedirectToAction("MostrarMiembros");
                }
                catch
                {
                    TempData["Mensaje"] = $"Error, no se pudo ejecutar correctamente";
                    return RedirectToAction("MostrarMiembros");
                }
            }
            else
            {
                TempData["MensajeError"] = "Lo siento accesso denegado";
                return RedirectToAction("Index", "Error");
            }
        }
        //-------------------------------------------------------------------


        

        [HttpPost]
        public IActionResult publicarpost(Post p, IFormFile file)
        {

            string? rol = HttpContext.Session.GetString("Rol");

            if (rol != null && rol.Equals("Miembro"))
            {
                try
                {

                    if (file != null && file.Length > 0)
                    {
                        string[] partesnombrearchivo = file.FileName.Split(".");
                        string extension = partesnombrearchivo[partesnombrearchivo.Length - 1];

                        p.NombreImagen = p.Id + "." + extension;

                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", p.NombreImagen);

                        var stream = new FileStream(path, FileMode.Create);

                        file.CopyTo(stream);
                    }


                    string? emaillogueado = HttpContext.Session.GetString("usuarioLogueado");

                    if (emaillogueado != null)
                    {
                        Usuario user = Sistema.ObtenerInstancia.ObtenerMiembroPorEmail(emaillogueado);

                        if (user is Miembro)
                        {
                            Miembro m = (Miembro)user;
                            
                            Sistema.ObtenerInstancia.AltaPost(p.Titulo, p.Texto, m, p.Privado, p.NombreImagen);
                            
                            TempData["error"] = false;
                            TempData["Mensaje"] = "Se realizó el post con exito";
                            return RedirectToAction("RealizarPost");
                        }
                    }

                }

                catch (Exception ex)
                {
                    TempData["error"] = true;
                    TempData["MensajeError"] = ex.Message;
                    return RedirectToAction("RealizarPost");
                }
            }

            return RedirectToAction("RealizarPost");
        }

        [HttpPost]
        public IActionResult Comentar(string titulo, string texto, int Id)
        {

            string? rol = HttpContext.Session.GetString("Rol");

            if (rol != null && rol.Equals("Miembro"))
            {
                try
                {

                    string? emaillogueado = HttpContext.Session.GetString("usuarioLogueado");

                    if (emaillogueado != null)
                    {
                        Usuario user = Sistema.ObtenerInstancia.ObtenerMiembroPorEmail(emaillogueado);

                        if (user is Miembro)
                        {
                            Miembro miembro = (Miembro)user;                                                     
                            
                            Sistema.ObtenerInstancia.AltaComentario(titulo, miembro, texto, Id);

                            TempData["error"] = false;
                            TempData["IdPost"] = Id;

                            return RedirectToAction("VerPost");
                        }
                    }

                }
                catch (Exception ex)
                {
                    TempData["error"] = true;
                    TempData["MensajeError"] = ex.Message;
                    return RedirectToAction("VerPost");
                }
            }

            return RedirectToAction("VerPost");
        }

        public IActionResult Like(int Id)
        {

            try
            {
                Publicacion p = Sistema.ObtenerInstancia.ObtenerPublicacionPorId(Id);

                int? idMiembro = HttpContext.Session.GetInt32("Id");
                


                Miembro m = Sistema.ObtenerInstancia.ObtenerMiembroPorID(idMiembro);

                Sistema.ObtenerInstancia.ReaccionLike(m, p);
                
                return RedirectToAction("VerPost");
            }
            catch (Exception e)
            {
                TempData["Mensaje"] = e.Message;
                TempData["MensajeError"] = true;
                return RedirectToAction("VerPost");
            }
        }

        public IActionResult Dislike(int Id)
        {
            try
            {
                Publicacion p = Sistema.ObtenerInstancia.ObtenerPublicacionPorId(Id);

                int? idMiembro = HttpContext.Session.GetInt32("Id");

                Miembro m = Sistema.ObtenerInstancia.ObtenerMiembroPorID(idMiembro);

                Sistema.ObtenerInstancia.ReaccionDislike(m, p);
                //TempData["Mensaje"] = "Ha reaccionado con éxito";
                //TempData["MensajeError"] = false;
                return RedirectToAction("VerPost");
            }
            catch (Exception e)
            {
                TempData["Mensaje"] = e.Message;
                TempData["MensajeError"] = true;
                return RedirectToAction("VerPost");
            }
        }




        //public IActionResult DislikeComentario(int IdCo, int IdP)
        //{
        //    try
        //    {
        //        Post p = Sistema.ObtenerInstancia.ObtenerPostPorId(IdP);

        //        int? idMiembro = HttpContext.Session.GetInt32("Id");

        //        Miembro m = Sistema.ObtenerInstancia.ObtenerMiembroPorID(idMiembro);

        //        Sistema.ObtenerInstancia.ReaccionDisLikeComent(m, IdPost, IdComentario);
        //        //TempData["Mensaje"] = "Ha reaccionado con éxito";
        //        //TempData["MensajeError"] = false;
        //        return RedirectToAction("VerPost");
        //    }
        //    catch (Exception e)
        //    {
        //        TempData["Mensaje"] = e.Message;
        //        TempData["MensajeError"] = true;
        //        return RedirectToAction("VerPost");
        //    }
        //}



        //public IActionResult LikeComentario(int IdCo, int IdP)
        //{

        //    try
        //    {
        //        Post p = Sistema.ObtenerInstancia.ObtenerPostPorId(IdP);

        //        int? idMiembro = HttpContext.Session.GetInt32("Id");

        //        Miembro m = Sistema.ObtenerInstancia.ObtenerMiembroPorID(idMiembro);

        //        Sistema.ObtenerInstancia.ReaccionLikeComent(m, IdP, IdComentario);
        //        //TempData["Mensaje"] = "Ha reaccionado con éxito";
        //        //TempData["MensajeError"] = false;
        //        return RedirectToAction("VerPost");
        //    }
        //    catch (Exception e)
        //    {
        //        TempData["Mensaje"] = e.Message;
        //        TempData["MensajeError"] = true;
        //        return RedirectToAction("VerPost");
        //    }
        //}

        // GET: PostController
        public ActionResult Index()
        {
            return View();
        }

        // GET: PostController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PostController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PostController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PostController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PostController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PostController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PostController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}

    






    