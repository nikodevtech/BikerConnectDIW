using BikerConnectDIW.DTO;
using BikerConnectDIW.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BikerConnectDIW.Controllers
{
    public class AdministracionUsuariosController : Controller
    {

        private readonly IUsuarioServicio _usuarioServicio;

        public AdministracionUsuariosController(IUsuarioServicio usuarioServicio)
        {
            _usuarioServicio = usuarioServicio;
        }


        [Authorize(Roles = "ROLE_ADMIN")]
        [HttpGet]
        [Route("/privada/administracion-usuarios")]
        public IActionResult ListadoUsuarios()
        {
            try
            {
                List<UsuarioDTO> usuarios = _usuarioServicio.obtenerTodosLosUsuarios();
                ViewBag.Usuarios = usuarios;

                return View("~/Views/Home/administracionUsuarios.cshtml");
            }
            catch (Exception e)
            {
                ViewData["error"] = "Ocurrió un error al obtener la lista de usuarios";
                return View("~/Views/Home/dashboard.cshtml");
            }
        }

        [Authorize(Roles = "ROLE_ADMIN")]
        [HttpGet]
        [Route("/privada/eliminar-usuario/{id}")]
        public IActionResult EliminarUsuario(long id)
        {
            try
            {
                UsuarioDTO usuario = _usuarioServicio.buscarPorId(id);
                List<UsuarioDTO> usuarios = _usuarioServicio.obtenerTodosLosUsuarios();

                string emailUsuarioActual = User.Identity.Name;

                int adminsRestantes = _usuarioServicio.contarUsuariosPorRol("ROLE_ADMIN");

                if (emailUsuarioActual == usuario.EmailUsuario)
                {
                    ViewData["noTePuedesEliminar"] = "No puedes eliminarte a ti mismo";
                    ViewBag.Usuarios = usuarios;
                    return View("~/Views/Home/administracionUsuarios.cshtml");

                }
                else
                {
                    if (User.IsInRole("ROLE_ADMIN") && adminsRestantes == 1)
                    {
                        ViewData["noSePuedeEliminar"] = "No se puede eliminar al ultimo admin";
                        ViewBag.Usuarios = usuarios;
                        return View("~/Views/Home/administracionUsuarios.cshtml");
                    }

                    if (usuario.MisQuedadas.Count > 0)
                    {
                        ViewData["elUsuarioTieneQuedadas"] = "No se puede eliminar un usuario con quedadas pendientes";
                        ViewBag.Usuarios = usuarios;
                        return View("~/Views/Home/administracionUsuarios.cshtml");
                    }
                }

                _usuarioServicio.eliminar(id);
                ViewData["eliminacionCorrecta"] = "El usuario se ha eliminado correctamente";
                ViewBag.Usuarios = _usuarioServicio.obtenerTodosLosUsuarios();
                return View("~/Views/Home/administracionUsuarios.cshtml");

            }
            catch (Exception e)
            {
                ViewData["error"] = "Ocurrió un error al eliminar el usuario";
                return View("~/Views/Home/dashboard.cshtml");
            }
        }

        [Authorize(Roles = "ROLE_ADMIN")]
        [HttpGet]
        [Route("/privada/editar-usuario/{id}")]
        public IActionResult MostrarFormularioEdicion(long id)
        {
            try
            {
                UsuarioDTO usuarioDTO = _usuarioServicio.buscarPorId(id);

                if (usuarioDTO == null)
                {
                    return View("~/Views/Home/administracionUsuarios.cshtml");
                }

                return View("~/Views/Home/editarUsuario.cshtml", usuarioDTO);
            }
            catch (Exception e)
            {
                ViewData["error"] = "Ocurrió un error al obtener el usuario para editar";
                return View("~/Views/Home/dashboard.cshtml");
            }
        }

        [Authorize(Roles = "ROLE_ADMIN")]
        [HttpPost]
        [Route("/privada/procesar-editar")]
        public IActionResult ProcesarFormularioEdicion(UsuarioDTO usuarioDTO)
        {
            try
            {
                _usuarioServicio.actualizarUsuario(usuarioDTO);

                ViewData["EdicionCorrecta"] = "El Usuario se ha editado correctamente";
                ViewBag.Usuarios = _usuarioServicio.obtenerTodosLosUsuarios();
                return View("~/Views/Home/administracionUsuarios.cshtml");
            }
            catch (Exception e)
            {
                ViewData["error"] = "Ocurrió un error al editar el usuario";
                return View("~/Views/Home/dashboard.cshtml");
            }
        }

        [Authorize(Roles = "ROLE_ADMIN")]
        [HttpPost]
        [Route("/auth/admin/crear-cuenta")]
        public IActionResult RegistrarPost(UsuarioDTO usuarioDTO)
        {
            try
            {
                UsuarioDTO nuevoUsuario = _usuarioServicio.registrarUsuario(usuarioDTO);

                if (nuevoUsuario.EmailUsuario == "EmailNoConfirmado")
                {
                    ViewData["EmailNoConfirmado"] = "Ya existe un usuario registrado con ese email con la cuenta sin verificar";
                    ViewBag.Usuarios = _usuarioServicio.obtenerTodosLosUsuarios();
                    return View("~/Views/Home/administracionUsuarios.cshtml");
                }
                else if (nuevoUsuario.EmailUsuario == "EmailRepetido")
                {
                    ViewData["EmailRepetido"] = "Ya existe un usuario con ese email registrado en el sistema";
                    ViewBag.Usuarios = _usuarioServicio.obtenerTodosLosUsuarios();
                    return View("~/Views/Home/registro.cshtml");
                }
                else
                {
                    ViewData["MensajeRegistroExitoso"] = "Registro del nuevo usuario OK";
                    ViewBag.Usuarios = _usuarioServicio.obtenerTodosLosUsuarios();
                    return View("~/Views/Home/administracionUsuarios.cshtml");
                }
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                return View("~/Views/Home/administracionUsuarios.cshtml");
            }
        }

        [Authorize(Roles = "ROLE_ADMIN")]
        [HttpGet]
        [Route("/auth/admin/crear-cuenta")]
        public IActionResult RegistroUsuarioDesdeAdminGet()
        {
            try
            {
                UsuarioDTO usuarioDTO = new UsuarioDTO();
                ViewData["esRegistroDeAdmin"] = true;
                return View("~/Views/Home/registro.cshtml", usuarioDTO);

            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, reintente";
                return View("~/Views/Home/registro.cshtml");
            }
        }
    }
}
