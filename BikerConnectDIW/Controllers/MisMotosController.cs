using BikerConnectDIW.DTO;
using BikerConnectDIW.Servicios;
using BikerConnectDIW.Utils;
using DAL.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BikerConnectDIW.Controllers
{
    public class MisMotosController : Controller
    {

        private readonly IUsuarioServicio _usuarioServicio;
        private readonly IMotoServicio _motoServicio;

        public MisMotosController(IUsuarioServicio usuarioServicio, IMotoServicio motoServicio)
        {
            _usuarioServicio = usuarioServicio;
            _motoServicio = motoServicio;
        }

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
