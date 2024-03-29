﻿using BikerConnectDIW.DTO;
using BikerConnectDIW.Servicios;
using BikerConnectDIW.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BikerConnectDIW.Controllers
{
    /// <summary>
    /// Controlador para manejar las peticiones HTTP POST y GET relacionadas con las quedadas de usuarios.
    /// </summary>
    public class QuedadasController : Controller
    {

        private readonly IQuedadaServicio _quedadaServicio;
        private readonly IConvertirAdto _convertirAdto;
        private readonly IUsuarioServicio _usuarioServicio;

        public QuedadasController(
            IQuedadaServicio quedadaServicio,
            IConvertirAdto convertirAdto,
            IUsuarioServicio usuarioServicio
            )
        {
            _quedadaServicio = quedadaServicio;
            _convertirAdto = convertirAdto;
            _usuarioServicio = usuarioServicio;
        }

        /// <summary>
        /// Método para mostrar las quedadas disponibles.
        /// </summary>
        /// <returns>La vista de las quedadas</returns>
        [Authorize]
        [HttpGet]
        [Route("/privada/quedadas")]
        public IActionResult Quedadas()
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método Quedadas() de la clase QuedadasController");

                List<QuedadaDTO> quedadas = _quedadaServicio.obtenerQuedadas();
                if (quedadas != null && quedadas.Count > 0)
                    ViewBag.quedadas = quedadas;

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método Quedadas() de la clase QuedadasController");
                return View("~/Views/Home/quedadas.cshtml");
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método Quedadas() de la clase QuedadasController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/quedadas.cshtml");
            }
        }

        /// <summary>
        /// Método para mostrar el formulario de planificación de una quedada.
        /// </summary>
        /// <returns>Vista del formulario de planificación de quedadas</returns>
        [Authorize]
        [HttpGet]
        [Route("/privada/quedadas/planificar-quedada")]
        public IActionResult PlanificarQuedada()
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método PlanificarQuedada() de la clase QuedadasController");

                QuedadaDTO quedadaDTO = new QuedadaDTO();
                return View("~/Views/Home/registroQuedada.cshtml", quedadaDTO);
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método PlanificarQuedada() de la clase QuedadasController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/quedadas.cshtml");
            }
        }

        /// <summary>
        /// Método para registrar una nueva quedada.
        /// </summary>
        /// <param name="quedadaDTO">Datos de la quedada a registrar</param>
        /// <returns>Vista de las quedadas</returns>
        [Authorize]
        [HttpPost]
        [Route("/privada/quedadas/planificar-quedada")]
        public IActionResult RegistrarQuedada(QuedadaDTO quedadaDTO)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método RegistrarQuedada() de la clase QuedadasController");

                string usuarioOrganizador = User.Identity.Name;
                quedadaDTO.UsuarioOrganizador = usuarioOrganizador;

                bool quedadaCreada = _quedadaServicio.crearQuedada(quedadaDTO);

                if (quedadaCreada)
                {
                    List<QuedadaDTO> quedadas = _quedadaServicio.obtenerQuedadas();
                    ViewBag.Quedadas = quedadas;
                    ViewData["quedadaCreadaExito"] = "Quedada planificada correctamente";
                }
                else
                {
                    List<QuedadaDTO> quedadas = _quedadaServicio.obtenerQuedadas();
                    ViewBag.Quedadas = quedadas;
                    ViewData["quedadaCreadaError"] = "No se pudo registrar la quedada";
                }
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método RegistrarQuedada() de la clase QuedadasController" +
                    (ViewData["quedadaCreadaExito"] != null ? ". " + ViewData["quedadaCreadaExito"] :
                    (ViewData["quedadaCreadaError"] != null ? ". " + ViewData["quedadaCreadaError"] : "")));
                return View("~/Views/Home/quedadas.cshtml");
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método RegistrarQuedada() de la clase QuedadasController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/quedadas.cshtml");
            }
        }

        /// <summary>
        /// Método para mostrar los detalles de una quedada.
        /// </summary>
        /// <param name="id">Identificador de la quedada</param>
        /// <returns>La vista con los detalles de la quedada si se encuentra, de lo contrario redirecciona a la página de quedadas</returns>
        [Authorize]
        [HttpGet]
        [Route("/privada/quedadas/detalle-quedada/{id}")]
        public IActionResult VerDetallesQuedada(long id)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método VerDetallesQuedada() de la clase QuedadasController");

                QuedadaDTO quedada = _quedadaServicio.obtenerQuedadaPorId(id);
                if (quedada != null)
                {
                    List<UsuarioDTO> participantes = _quedadaServicio.obtenerUsuariosParticipantes(id);

                    if (participantes != null && participantes.Count > 0)
                    {
                        ViewBag.Participantes = participantes;
                    }
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método VerDetallesQuedada() de la clase QuedadasController");
                    return View("~/Views/Home/detalleQuedada.cshtml", quedada);
                }
                else
                {
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método VerDetallesQuedada() de la clase QuedadasController. No se encontró la quedada con id " + id);
                    return RedirectToAction("Quedadas");
                }
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método VerDetallesQuedada() de la clase QuedadasController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/quedadas.cshtml");
            }
        }

        /// <summary>
        /// Método para permitir que un usuario se una a una quedada.
        /// </summary>
        /// <param name="id">Identificador de la quedada a la que desea unirse el usuario</param>
        /// <returns>La vista de quedadas con un mensaje de éxito o error según el resultado</returns>
        [Authorize]
        [HttpGet]
        [Route("/privada/quedadas/detalle-quedada/unirse/{id}")]
        public IActionResult UnirseQuedada(long id)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método UnirseQuedada() de la clase QuedadasController");

                string mensaje = _quedadaServicio.unirseQuedada(id, User.Identity.Name);
                List<QuedadaDTO> quedadas = _quedadaServicio.obtenerQuedadas();
                ViewBag.Quedadas = quedadas;
                switch (mensaje)
                {
                    case "Usuario unido a la quedada":
                        ViewData["quedadaAsistenciaExito"] = "El usuario se ha unido correctamente";
                        break;
                    case "Ya estás unido a esta quedada":
                        ViewData["quedadaAsistenciaInfo"] = "El usuarios ya está unido a la quedada";
                        break;
                    case "La quedada está completada":
                        ViewData["quedadaYaCompletada"] = "El usuario no pudo unirse, la quedada ya está completada";
                        break;
                    case "Usuario o quedada no encontrado":
                        ViewData["usuarioQuedadaNoEncontrado"] = "Usuario o quedada no encontrado";
                        break;
                    case "La quedada ya ha pasado":
                        ViewData["quedadaPasada"] = "La fecha de la quedada ya ha pasado";
                        break;
                    case "El usuario no tiene una moto registrada":
                        ViewData["motoNoRegistrada"] = "El usuario no tiene una moto registrada";
                        break;
                }
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método UnirseQuedada() de la clase QuedadasController");
                return View("~/Views/Home/quedadas.cshtml");
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método UnirseQuedada() de la clase QuedadasController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/quedadas.cshtml");
            }
        }

        /// <summary>
        /// Método para permitir que un usuario cancele su asistencia a una quedada.
        /// </summary>
        /// <param name="id">Identificador de la quedada de la que el usuario desea cancelar su asistencia</param>
        /// <returns>La vista de quedadas con un mensaje de éxito o error según el resultado</returns>
        [Authorize]
        [HttpGet]
        [Route("/privada/quedadas/detalle-quedada/cancelar-asistencia/{id}")]
        public IActionResult CancelarAsistenciaQuedada(long id)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método CancelarAsistenciaQuedada() de la clase QuedadasController");

                string userId = User.Identity.Name;

                bool estaElUsuarioUnido = _quedadaServicio.estaUsuarioUnido(id, userId);

                List<QuedadaDTO> quedadas = _quedadaServicio.obtenerQuedadas();

                if (!estaElUsuarioUnido)
                {
                    ViewData["quedadaCancelacionInfo"] = "No puedes cancelar asistencia si no estás unido";
                    ViewBag.Quedadas = quedadas;
                }
                else
                {
                    bool canceladoCorrectamente = _quedadaServicio.cancelarAsistenciaQuedada(id, userId);

                    if (canceladoCorrectamente)
                    {
                        ViewData["quedadaCancelacionExito"] = "Se ha cancelado la asistencia con exito";
                        ViewBag.Quedadas = _quedadaServicio.obtenerQuedadas();
                    }
                    else
                    {
                        ViewData["quedadaCancelacionError"] = "No se ha podido cancelar la asistencia";
                        ViewBag.Quedadas = quedadas;
                    }
                }
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método CancelarAsistenciaQuedada() de la clase QuedadasController");
                return View("~/Views/Home/quedadas.cshtml");
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método CancelarAsistenciaQuedada() de la clase QuedadasController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/quedadas.cshtml");
            }

        }

        /// <summary>
        /// Método para mostrar las quedadas en las que el usuario actual está inscrito.
        /// </summary>
        /// <returns>La vista de 'MisQuedadas', si no se encuentra al usuario redirecciona a la página de quedadas</returns>
        [Authorize]
        [HttpGet]
        [Route("/privada/quedadas/mis-quedadas")]
        public IActionResult MisQuedadas()
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método MisQuedadas() de la clase QuedadasController");

                UsuarioDTO? usuario = _usuarioServicio.obtenerUsuarioPorEmail(User.Identity.Name);
                if (usuario != null)
                {
                    List<QuedadaDTO> misQuedadas = _quedadaServicio.obtenerQuedadasDelUsuario(usuario.Id);

                    if (misQuedadas != null && misQuedadas.Count > 0)
                    {
                        ViewBag.MisQuedadas = misQuedadas;
                    }
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método MisQuedadas() de la clase QuedadasController");
                    return View("~/Views/Home/misQuedadas.cshtml");
                }
                else
                {
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método MisQuedadas() de la clase QuedadasController");
                    return RedirectToAction("Quedadas");
                }
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, reintente.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método MisQuedadas() de la clase QuedadasController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/quedadas.cshtml");
            }
        }

        /// <summary>
        /// Método para permitir que el organizador de una quedada la cancele.
        /// </summary>
        /// <param name="id">Identificador de la quedada a cancelar</param>
        /// <returns>La vista de quedadas con un mensaje según el resultado de la operación</returns>
        [Authorize]
        [HttpGet]
        [Route("/privada/quedadas/detalle-quedada/cancelar-quedada/{id}")]
        public IActionResult CancelarQuedada(long id)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método CancelarQuedada() de la clase QuedadasController");

                QuedadaDTO q = _quedadaServicio.obtenerQuedadaPorId(id);
                string emailUsuario = User.Identity.Name;

                if (q.UsuarioOrganizador == emailUsuario)
                {
                    string mensaje = _quedadaServicio.cancelarQuedada(id);
                    List<QuedadaDTO> quedadas = _quedadaServicio.obtenerQuedadas();

                    if (quedadas != null && quedadas.Count > 0)
                    {
                        ViewBag.Quedadas = quedadas;
                    }

                    switch (mensaje)
                    {
                        case "Quedada cancelada":
                            ViewData["quedadaCancelacionQuedadaExito"] = "Se ha cancelado la asistencia correctamente";
                            ViewBag.Quedadas = _quedadaServicio.obtenerQuedadas();
                            break;
                        case "Quedada completada":
                            ViewData["quedadaCancelacionCompletada"] = "No se puede cancelar una quedada completada";
                            break;
                        case "Usuarios participantes":
                            ViewData["quedadaCancelacionParticipantes"] = "No se puede cancelar una quedada con participantes";
                            break;
                    }
                }
                else
                {
                    ViewData["quedadaCancelacionPermiso"] = "No tiene permiso para eliminar esta quedada";
                }
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, reintente.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método CancelarQuedada() de la clase QuedadasController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/quedadas.cshtml");
            }
            EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método CancelarQuedada() de la clase QuedadasController");
            return View("~/Views/Home/quedadas.cshtml");
        }

        [Authorize]
        [HttpGet]
        [Route("/quedada/completar/{id}")]
        public IActionResult MarcarQuedadaComoCompletada(long id)
        {
            try
            {
                QuedadaDTO? quedada = _quedadaServicio.obtenerQuedadaPorId(id);

                if (quedada != null)
                {
                    if (quedada.FechaHora < DateTime.Now)
                    {
                        quedada.Estado = "Completada";
                        _quedadaServicio.actualizarQuedada(quedada);
                        ViewData["quedadaCompletada"] = "Quedada marcada como completada";
                        ViewBag.Quedadas = _quedadaServicio.obtenerQuedadas();

                        return View("~/Views/Home/quedadas.cshtml");
                    }
                    else
                    {
                        ViewData["quedadaNoCompletada"] = "Quedada no marcada como completada";
                        ViewBag.Quedadas = _quedadaServicio.obtenerQuedadas();
                        return View("~/Views/Home/quedadas.cshtml");
                    }
                }
                else 
                {
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método MisQuedadas() de la clase QuedadasController");
                    return RedirectToAction("Quedadas");
                }

               
            }
            catch (Exception ex)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, reintente.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método CancelarQuedada() de la clase QuedadasController: " + ex.Message + ex.StackTrace);
                return View("~/Views/Home/quedadas.cshtml");
            }
        }

    }
}
