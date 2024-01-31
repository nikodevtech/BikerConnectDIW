using BikerConnectDIW.DTO;
using Microsoft.AspNetCore.Mvc;

namespace BikerConnectDIW.Controllers
{
    public class SolicitarRecuperacionController : Controller
    {
        [HttpGet]
        [Route("/auth/solicitar-recuperacion")]
        public IActionResult MostrarVistaIniciarRecuperacion()
        {
            try
            {
                UsuarioDTO usuarioDTO = new UsuarioDTO();
                return View("~/Views/Home/solicitarRecuperacionPassword.cshtml", usuarioDTO);
            }
            catch (Exception e)
            {
                ViewData["Error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                return View("~/Views/Home/solicitarRecuperacionPassword.cshtml");
            }
        }
    }
}
