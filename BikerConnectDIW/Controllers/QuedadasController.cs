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
        private readonly BikerconnectContext _contexto;
        private readonly IConvertirAdto _convertirAdto;

        public QuedadasController(
            IQuedadaServicio quedadaServicio, 
            BikerconnectContext contexto,
            IConvertirAdto convertirAdto
            )
        {
            _quedadaServicio = quedadaServicio;
            _contexto = contexto;
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


    }
}
