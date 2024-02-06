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

        public QuedadasController(
            IQuedadaServicio quedadaServicio, 
            IConvertirAdto convertirAdto
            )
        {
            _quedadaServicio = quedadaServicio;
            _convertirAdto = convertirAdto;
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


    }
}
