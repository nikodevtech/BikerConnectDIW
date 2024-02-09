using BikerConnectDIW.DTO;
using BikerConnectDIW.Servicios;
using BikerConnectDIW.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BikerConnectDIW.Controllers
{
    /// <summary>
    /// Controlador para gestionar las peticiones HTTP POST y GET relacionadas con las motos de un usuario.
    /// </summary>
    public class MisMotosController : Controller
    {

        private readonly IUsuarioServicio _usuarioServicio;
        private readonly IMotoServicio _motoServicio;

        public MisMotosController(IUsuarioServicio usuarioServicio, IMotoServicio motoServicio)
        {
            _usuarioServicio = usuarioServicio;
            _motoServicio = motoServicio;
        }


        /// <summary>
        /// Método para mostrar las motos asociadas al usuario autenticado.
        /// </summary>
        /// <returns>La vista de "misMotos.cshtml" con la lista de motos del usuario.</returns>
        [Authorize]
        [HttpGet]
        [Route("/privada/mis-motos")]
        public IActionResult MostrarMisMotos()
        {

            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método MostrarMisMotos() de la clase MisMotosController");

                string? emailDelUsuario = User.Identity?.Name;
                UsuarioDTO usuario = _usuarioServicio.obtenerUsuarioPorEmail(emailDelUsuario);

                if (usuario != null)
                {
                    List<MotoDTO> misMotos = _motoServicio.obtenerMotosPorPropietarioId(usuario.Id);

                    if (misMotos != null && misMotos.Count > 0)
                    {
                        ViewBag.MisMotos = misMotos;
                    }
                }
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método MostrarMisMotos() de la clase MisMotosController");
                return View("~/Views/Home/misMotos.cshtml");
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método MostrarMisMotos() de la clase MisMotosController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/misMotos.cshtml");
            }
        }

        /// <summary>
        /// Método para mostrar el formulario de registro de una nueva moto.
        /// </summary>
        /// <returns>La vista de "registroMoto.cshtml" con el formulario para agregar una nueva moto.</returns>
        [Authorize]
        [HttpGet]
        [Route("/privada/crear-moto")]
        public IActionResult MostrarFormNuevaMoto()
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método MostrarFormNuevaMoto() de la clase MisMotosController");

                string emailDelUsuario = User.Identity.Name;
                UsuarioDTO usuarioSesionActual = _usuarioServicio.obtenerUsuarioPorEmail(emailDelUsuario);
                MotoDTO nuevaMoto = new MotoDTO();
                nuevaMoto.IdPropietario = usuarioSesionActual.Id;

                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método MostrarFormNuevaMoto() de la clase MisMotosController");
                return View("~/Views/Home/registroMoto.cshtml", nuevaMoto);
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método MostrarFormNuevaMoto() de la clase MisMotosController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/misMotos.cshtml");
            }
        }

        /// <summary>
        /// Método para procesar el formulario de registro de una nueva moto.
        /// </summary>
        /// <param name="motoDTO">Objeto DTO que contiene los datos de la nueva moto a registrar.</param>
        /// <returns>La vista de "misMotos.cshtml" con un mensaje indicando el resultado del registro.</returns>
        [Authorize]
        [HttpPost]
        [Route("/privada/crear-moto")]
        public IActionResult RegistrarMotoPost(MotoDTO motoDTO)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método RegistrarMotoPost() de la clase MisMotosController");

                bool motoRegistradaConExito = _motoServicio.registrarMoto(motoDTO);

                if (motoRegistradaConExito)
                {
                    ViewData["altaMotoExito"] = "Alta de la moto en el sistema OK";
                    List<MotoDTO> misMotos = _motoServicio.obtenerMotosPorPropietarioId(motoDTO.IdPropietario);
                    ViewBag.MisMotos = misMotos;
                }
                else
                {
                    ViewData["altaMotoError"] = "No se pudo dar de alta la moto";
                    List<MotoDTO> misMotos = _motoServicio.obtenerMotosPorPropietarioId(motoDTO.IdPropietario);
                    ViewBag.MisMotos = misMotos;
                }
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método RegistrarMotoPost() de la clase MisMotosController" +
                    (ViewData["altaMotoExito"] != null ? ". " + ViewData["altaMotoExito"] :
                    (ViewData["altaMotoError"] != null ? ". " + ViewData["altaMotoError"] : "")));
                return View("~/Views/Home/misMotos.cshtml");
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método RegistrarMotoPost() de la clase MisMotosController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/dashboard.cshtml");
            }
        }

        /// <summary>
        /// Método para eliminar una moto existente.
        /// </summary>
        /// <param name="id">El ID de la moto a eliminar.</param>
        /// <returns>La vista de "misMotos.cshtml" con un mensaje indicando el resultado de la eliminación.</returns>
        [Authorize]
        [HttpGet]
        [Route("/privada/eliminar-moto/{id}")]
        public IActionResult EliminarMoto(long id)
        {
            try
            {
                EscribirLog.escribirEnFicheroLog("[INFO] Entrando en el método EliminarMoto() de la clase MisMotosController");

                MotoDTO moto = _motoServicio.buscarPorId(id);
                if (moto != null)
                {
                    _motoServicio.eliminarMoto(id);
                    List<MotoDTO> misMotos = _motoServicio.obtenerMotosPorPropietarioId(moto.IdPropietario);
                    if (misMotos != null && misMotos.Count > 0)
                    {
                        ViewBag.MisMotos = misMotos;
                    }
                    ViewData["eliminacionCorrecta"] = "La moto se ha eliminado correctamente";
                }
                EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método EliminarMoto() de la clase MisMotosController. " + ViewData["eliminacionCorrecta"]);
                return View("~/Views/Home/misMotos.cshtml");
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar el borrado de la moto";
                EscribirLog.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método EliminarMoto() de la clase MisMotosController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/dashboard.cshtml");
            }
        }

    }
}
