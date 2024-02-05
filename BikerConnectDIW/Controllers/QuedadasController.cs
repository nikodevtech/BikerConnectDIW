using BikerConnectDIW.DTO;
using BikerConnectDIW.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BikerConnectDIW.Controllers
{
    public class QuedadasController : Controller
    {

        private readonly IQuedadaServicio _quedadaServicio;

        public QuedadasController(IQuedadaServicio quedadaServicio)
        {
            _quedadaServicio = quedadaServicio;
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

    }
}
