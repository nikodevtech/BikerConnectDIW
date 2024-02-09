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
            EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método Login() de la clase LoginController");
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
                    UsuarioDTO u = _usuarioServicio.obtenerUsuarioPorEmail(usuarioDTO.EmailUsuario);

                    // Al hacer login correctamente se crea una identidad de reclamaciones (claims identity) con información del usuario 
                    // y su rol, de esta manera se controla que solo los admin puedan acceder a la administracion de usuarios
                    // y se mantiene esa info del usuario autenticado durante toda la sesión en una cookie.
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuarioDTO.EmailUsuario),
                    };
                    if (!string.IsNullOrEmpty(u.Rol))
                    {
                        claims.Add(new Claim(ClaimTypes.Role, u.Rol));
                    }

                    var identidadDeReclamaciones = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // establece una cookie en el navegador con los datos del usuario antes mencionados y se mantiene en el contexto.
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identidadDeReclamaciones));

                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método ProcesarInicioSesion() de la clase LoginController");
                    return RedirectToAction("Dashboard", "Login");
                }
                else
                {
                    ViewData["MensajeErrorInicioSesion"] = "Credenciales inválidas o cuenta no confirmada. Inténtelo de nuevo.";
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método ProcesarInicioSesion() de la clase LoginController. " + ViewData["MensajeErrorInicioSesion"]);
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
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método ConfirmarCuenta() de la clase LoginController");

                bool confirmacionExitosa = _usuarioServicio.confirmarCuenta(token);

                if (confirmacionExitosa)
                {
                    ViewData["CuentaVerificada"] = "La dirección de correo ha sido confirmada correctamente";
                }
                else
                {
                    ViewData["yaEstabaVerificada"] = "El usuario ya estaba registrado y verificado";
                }

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método ConfirmarCuenta() de la clase LoginController" +
                    (ViewData["CuentaVerificada"] != null ? ". " + ViewData["CuentaVerificada"] :
                    (ViewData["yaEstabaVerificada"] != null ? ". " + ViewData["yaEstabaVerificada"] : "")));
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
            EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método Dashboard() de la clase LoginController");
            return View("~/Views/Home/dashboard.cshtml");
        }

        /// <summary>
        /// Método para cerrar la sesión del usuario.
        /// </summary>
        /// <returns>Al metodo llamado Index de HomeController (mostrando la vista inicial)</returns>
        [HttpPost]
        public IActionResult CerrarSesion()
        {
            EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método CerrarSesion() de la clase LoginController");
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
