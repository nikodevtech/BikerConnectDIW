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
                return View("Registro", usuarioDTO);
            }
            catch (Exception e)
            {
                ViewData["Error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                return View("Registro");
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
                    return View("Login");
                }
                else if (nuevoUsuario.CuentaConfirmada)
                {
                    ViewData["MensajeRegistroExitoso"] = "Registro del nuevo usuario OK";
                    //ViewData["Usuarios"] = _usuarioServicio.ObtenerTodos();
                    return View("AdministracionUsuarios");
                }
                else
                {
                    ViewData["EmailYaRegistrado"] = "Ya existe un usuario con ese email";
                    return View("Registro");
                }
            }
            catch (Exception e)
            {
                ViewData["Error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                return View("Registro");
            }
        }
    }
}
