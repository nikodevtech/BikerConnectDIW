using BikerConnectDIW.DTO;
using BikerConnectDIW.Servicios;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BikerConnectDIW.Utils;
using System;

namespace BikerConnectDIW.Controllers
{
    /// <summary>
    /// Controlador para manejar las peticiones HTTP POST y GET para la autenticación y la sesión de usuario.
    /// </summary>
    public class LoginController : Controller
    {
        private readonly IUsuarioServicio _usuarioServicio;

        public LoginController(IUsuarioServicio usuarioServicio)
        {
            _usuarioServicio = usuarioServicio;
        }

        /// <summary>
        /// Método para mostrar la página de inicio de sesión.
        /// </summary>
        [HttpGet]
        [Route("/auth/login")]
        public IActionResult Login()
        {
            try
            {
                UsuarioDTO usuarioDTO = new UsuarioDTO();
                return View("~/Views/Home/login.cshtml", usuarioDTO);
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método Login() de la clase LoginController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/login.cshtml");
            }
        }

        /// <summary>
        /// Método para procesar la solicitud de inicio de sesión, comprobando las credenciales introducidas por el usuario.
        /// </summary>
        /// <param name="usuarioDTO">Objeto DTO donde están los datos introducidos por el usuario</param>
        /// <returns>La vista de Dashboard si inicia sesíon correctamente, vuelta al login en caso contrario</returns>
        [HttpPost]
        [Route("/auth/iniciar-sesion")]
        public IActionResult ProcesarInicioSesion(UsuarioDTO usuarioDTO)
        {
            try
            {
                bool credencialesValidas = _usuarioServicio.verificarCredenciales(usuarioDTO.EmailUsuario, usuarioDTO.ClaveUsuario);

                if (credencialesValidas)
                {
                    // Crear identidad de reclamaciones (claims identity) y establecer una cookie en el navegador.
                    // También redirige al usuario al panel de control.
                    // Más adelante se explicará la función de este método.
                    return RedirectToAction("Dashboard", "Login");
                }
                else
                {
                    // En caso de credenciales inválidas, se muestra un mensaje de error.
                    ViewData["MensajeErrorInicioSesion"] = "Credenciales inválidas o cuenta no confirmada. Inténtelo de nuevo.";
                    return View("~/Views/Home/login.cshtml");
                }
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método ProcesarInicioSesion() de la clase LoginController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/login.cshtml");
            }
        }


        /// <summary>
        /// Método para confirmar la cuenta (su dirección de correo electrónico) del usuario.
        /// </summary>
        /// <param name="token"> Token asociado a la confirmación de la cuenta</param>
        /// <returns>La vista de login</returns>
        [HttpGet]
        [Route("/auth/confirmar-cuenta")]
        public IActionResult ConfirmarCuenta([FromQuery] string token)
        {
            try
            {
                bool confirmacionExitosa = _usuarioServicio.confirmarCuenta(token);

                if (confirmacionExitosa)
                {
                    ViewData["CuentaVerificada"] = "La dirección de correo ha sido confirmada correctamente";
                }
                else
                {
                    ViewData["yaEstabaVerificada"] = "El usuario ya estaba registrado y verificado";
                }

                return View("~/Views/Home/login.cshtml");
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método ConfirmarCuenta() de la clase LoginController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/login.cshtml");
            }
        }

        /// <summary>
        /// Método para mostrar el panel de control del usuario.
        /// </summary>
        /// <returns>La vista de dashboard</returns>
        [Authorize]
        [HttpGet]
        [Route("/privada/dashboard")]
        public IActionResult Dashboard()
        {
            UsuarioDTO u = _usuarioServicio.obtenerUsuarioPorEmail(User.Identity.Name);
            ViewBag.UsuarioDTO = u;
            return View("~/Views/Home/dashboard.cshtml");
        }

        /// <summary>
        /// Método para cerrar la sesión del usuario.
        /// </summary>
        /// <returns>Al metodo llamado Index de HomeController (mostrando la vista inicial)</returns>
        [HttpPost]
        public IActionResult CerrarSesion()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
