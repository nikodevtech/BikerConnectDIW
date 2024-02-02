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
        [HttpGet("/privada/administracion-usuarios")]
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
                ViewBag.Error = "Ocurrió un error al obtener la lista de usuarios";
                return View("~/Views/Home/dashboard.cshtml");
            }
        }

        [Authorize(Roles = "ROLE_ADMIN")]
        [HttpGet("/privada/eliminar-usuario/{id}")]
        public IActionResult EliminarUsuario(long id)
        {
            try
            {
                UsuarioDTO usuario = _usuarioServicio.buscarPorId(id);
                List<UsuarioDTO> usuarios = _usuarioServicio.obtenerTodosLosUsuarios();

                if (User.IsInRole("ROLE_USER"))
                {
                    TempData["noAdmin"] = "No tiene permiso para acceder al recurso";
                    ViewBag.Usuarios = usuarios;
                    return View("~/Views/Home/dashboard.cshtml");
                }
                else
                {
                    if (User.IsInRole("ROLE_ADMIN") && usuario.Rol == "ROLE_ADMIN")
                    {
                        TempData["noSePuedeEliminar"] = "No se puede eliminar a un admin";
                        ViewBag.Usuarios = usuarios;
                        return View("~/Views/Home/administracionUsuarios.cshtml");
                    }

                    if (usuario.MisQuedadas.Count > 0)
                    {
                        TempData["elUsuarioTieneQuedadas"] = "No se puede eliminar un usuario con quedadas";
                        ViewBag.Usuarios = usuarios;
                        return View("~/Views/Home/administracionUsuarios.cshtml");
                    }

                    _usuarioServicio.eliminar(id);
                    TempData["eliminacionCorrecta"] = "El usuario se ha eliminado correctamente";
                    ViewBag.Usuarios = _usuarioServicio.obtenerTodosLosUsuarios();
                    return View("~/Views/Home/administracionUsuarios.cshtml");
                }
            }
            catch (Exception e)
            {
                TempData["Error"] = "Ocurrió un error al eliminar el usuario";
                return View("~/Views/Home/dashboard.cshtml");
            }
        }
    }
}
