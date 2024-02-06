using BikerConnectDIW.DTO;
using BikerConnectDIW.Servicios;
using DAL.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BikerConnectDIW.Controllers
{
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


        [Authorize]
        [HttpGet]
        [Route("/privada/quedadas")]
        public IActionResult Quedadas()
        {
            try
            {
                List<QuedadaDTO> quedadas = _quedadaServicio.obtenerQuedadas();
                if (quedadas != null && quedadas.Count > 0)
                    ViewBag.quedadas = quedadas;
                return View("~/Views/Home/quedadas.cshtml");
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                return View("~/Views/Home/quedadas.cshtml");
            }
        }

        [Authorize]
        [HttpGet]
        [Route("/privada/quedadas/planificar-quedada")]
        public IActionResult PlanificarQuedada()
        {
            try
            {
                QuedadaDTO quedadaDTO = new QuedadaDTO();
                return View("~/Views/Home/registroQuedada.cshtml", quedadaDTO);

            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                return View("~/Views/Home/quedadas.cshtml");
            }
        }

        [Authorize]
        [HttpPost]
        [Route("/privada/quedadas/planificar-quedada")]
        public IActionResult RegistrarQuedada(QuedadaDTO quedadaDTO)
        {
            try
            {
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

                return View("~/Views/Home/quedadas.cshtml");
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                return View("~/Views/Home/quedadas.cshtml");
            }
        }

        [Authorize]
        [HttpGet]
        [Route("/privada/quedadas/detalle-quedada/{id}")]
        public IActionResult VerDetallesQuedada(long id)
        {
            try
            {
                QuedadaDTO quedada = _quedadaServicio.obtenerQuedadaPorId(id);
                if (quedada != null)
                {
                    List<UsuarioDTO> participantes = _quedadaServicio.obtenerUsuariosParticipantes(id);

                    if (participantes != null && participantes.Count > 0)
                    {
                        ViewBag.Participantes = participantes;
                    }

                    return View("~/Views/Home/detalleQuedada.cshtml", quedada);
                }
                else
                {
                    return RedirectToAction("Quedadas");
                }
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                return View("~/Views/Home/quedadas.cshtml");
            }
        }

        [Authorize]
        [HttpGet]
        [Route("/privada/quedadas/detalle-quedada/unirse/{id}")]
        public IActionResult UnirseQuedada(long id)
        {
            try
            {
                string mensaje = _quedadaServicio.unirseQuedada(id, User.Identity.Name);
                List<QuedadaDTO> quedadas = _quedadaServicio.obtenerQuedadas();
                ViewBag.Quedadas = quedadas;
                switch (mensaje)
                {
                    case "Usuario unido a la quedada":
                        ViewData["quedadaAsistenciaExito"] = "Se ha unido correctamente";
                        break;
                    case "Ya estás unido a esta quedada":
                        ViewData["quedadaAsistenciaInfo"] = "Ya estás unido a esta quedada";
                        break;
                    case "La quedada está completada":
                        ViewData["quedadaYaCompletada"] = "La quedada ya está completada";
                        break;
                    case "Usuario o quedada no encontrado":
                        ViewData["usuarioQuedadaNoEncontrado"] = "La quedada ya está completada";
                        break;
                }
                return View("~/Views/Home/quedadas.cshtml");
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                return View("~/Views/Home/quedadas.cshtml");
            }
        }

        [Authorize]
        [HttpGet]
        [Route("/privada/quedadas/detalle-quedada/cancelar-asistencia/{id}")]
        public IActionResult CancelarAsistenciaQuedada(long id)
        {
            try
            {
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

                return View("~/Views/Home/quedadas.cshtml");
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                return View("~/Views/Home/quedadas.cshtml");
            }

        }

        [Authorize]
        [HttpGet]
        [Route("/privada/quedadas/mis-quedadas")]
        public IActionResult MisQuedadas()
        {
            try
            {
                UsuarioDTO? usuario = _usuarioServicio.obtenerUsuarioPorEmail(User.Identity.Name);
                if (usuario != null)
                {
                    List<QuedadaDTO> misQuedadas = _quedadaServicio.obtenerQuedadasDelUsuario(usuario.Id);

                    if (misQuedadas != null && misQuedadas.Count > 0)
                    {
                        ViewBag.MisQuedadas = misQuedadas;
                    }

                    return View("~/Views/Home/misQuedadas.cshtml");
                }
                else
                {
                    return RedirectToAction("Quedadas");
                }
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, reintente.";
                return View("~/Views/Home/quedadas.cshtml");
            }
        }

        [Authorize]
        [HttpGet]
        [Route("/privada/quedadas/detalle-quedada/cancelar-quedada/{id}")]
        public IActionResult CancelarQuedada(long id)
        {
            try
            {
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
                return View("~/Views/Home/quedadas.cshtml");
            }
            return View("~/Views/Home/quedadas.cshtml");
        }
    }
}
