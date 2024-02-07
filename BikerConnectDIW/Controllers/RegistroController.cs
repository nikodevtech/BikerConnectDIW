using BikerConnectDIW.DTO;
using BikerConnectDIW.Servicios;
using BikerConnectDIW.Utils;
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
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método RegistrarGet() de la clase RegistroController");

                UsuarioDTO usuarioDTO = new UsuarioDTO();
                return View("~/Views/Home/registro.cshtml", usuarioDTO);

            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método RegistrarGet() de la clase RegistroController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/registro.cshtml");
            }
        }

        [HttpPost]
        [Route("/auth/crear-cuenta")]
        public IActionResult RegistrarPost(UsuarioDTO usuarioDTO)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método RegistrarPost() de la clase RegistroController");

                UsuarioDTO nuevoUsuario = _usuarioServicio.registrarUsuario(usuarioDTO);

                if (nuevoUsuario.EmailUsuario == "EmailNoConfirmado")
                {
                    ViewData["EmailNoConfirmado"] = "Ya existe un usuario registrado con ese email pero con la cuenta sin verificar";
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método RegistrarGet() de la clase RegistroController. " + ViewData["EmailNoConfirmado"]);
                    return View("~/Views/Home/login.cshtml");

                }
                else if (nuevoUsuario.EmailUsuario == "EmailRepetido")
                {
                    ViewData["EmailRepetido"] = "Ya existe un usuario con ese email registrado en el sistema";
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método RegistrarGet() de la clase RegistroController. " + ViewData["EmailRepetido"]);
                    return View("~/Views/Home/registro.cshtml");
                }
                else
                {
                    ViewData["MensajeRegistroExitoso"] = "Registro del nuevo usuario OK";
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método RegistrarGet() de la clase RegistroController. " + ViewData["MensajeRegistroExitoso"]);
                    return View("~/Views/Home/login.cshtml");
                }


            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método RegistrarPost() de la clase  RegistroController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/registro.cshtml");
            }
        }
    }
}
