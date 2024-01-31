using BikerConnectDIW.DTO;
using BikerConnectDIW.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace BikerConnectDIW.Controllers
{
    public class RegistroController : Controller
    {

        private readonly IUsuarioServicio _usuarioServicio;

        public RegistroController(IUsuarioServicio usuarioServicio)
        {
            _usuarioServicio = usuarioServicio;
        }

        [HttpGet]
        [Route("/auth/crear-cuenta")]
        public IActionResult RegistrarGet()
        {
            try
            {
                var usuarioDTO = new UsuarioDTO();
                return View("~/Views/Home/registro.cshtml", usuarioDTO);

            }
            catch (Exception e)
            {
                ViewData["Error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                return View("~/Views/Home/registro.cshtml");
            }
        }

        [HttpPost]
        [Route("/auth/crear-cuenta")]
        public IActionResult RegistrarPost(UsuarioDTO usuarioDTO)
        {
            try
            {
                UsuarioDTO nuevoUsuario = _usuarioServicio.registrarUsuario(usuarioDTO);

                if (nuevoUsuario != null && !nuevoUsuario.CuentaConfirmada)
                {
                    ViewData["MensajeRegistroExitoso"] = "Registro del nuevo usuario OK";
                    return View("~/Views/Home/login.cshtml");
                }
                else if (nuevoUsuario?.EmailUsuario == "EmailNoConfirmado")
                {
                    ViewData["EmailNoConfirmado"] = "Ya está registrado pero no confirmado";
                    //ViewData["Usuarios"] = _usuarioServicio.ObtenerTodos();
                    return View("~/Views/Home/administracionUsuarios.cshtml");
                }
                else
                {
                    ViewData["EmailYaRegistrado"] = "Ya existe un usuario con ese email";
                    return View("~/Views/Home/registro.cshtml");
                }
            }
            catch (Exception e)
            {
                ViewData["Error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                return View("~/Views/Home/registro.cshtml");
            }
        }
    }
}
