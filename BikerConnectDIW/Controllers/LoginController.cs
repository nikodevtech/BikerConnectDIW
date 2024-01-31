using BikerConnectDIW.Servicios;
using DAL.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace BikerConnectDIW.Controllers
{
    public class LoginController : Controller
    {

        private readonly IUsuarioServicio _usuarioServicio;

        public LoginController(IUsuarioServicio usuarioServicio)
        {
            _usuarioServicio = usuarioServicio;
        }

        [HttpGet]
        [Route("/auth/login")]
        public IActionResult Login()
        {
            return View("~/Views/Home/login.cshtml");
        }

        [HttpGet("/auth/confirmar-cuenta")]
        public IActionResult ConfirmarCuenta([FromQuery] string token)
        {
            try
            {
                bool confirmacionExitosa = _usuarioServicio.ConfirmarCuenta(token);

                if (confirmacionExitosa)
                {
                    ViewData["CuentaVerificada"] = "Su dirección de correo ha sido confirmada correctamente";
                }
                else
                {
                    ViewData["CuentaNoVerificada"] = "Error al confirmar su email";
                }

                return View("~/Views/Home/login.cshtml"); 
            }
            catch (Exception e)
            {
                ViewData["Error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                return View("~/Views/Home/login.cshtml"); 
            }
        }
    }
}
