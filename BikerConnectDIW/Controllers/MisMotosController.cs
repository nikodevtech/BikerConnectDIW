using BikerConnectDIW.DTO;
using BikerConnectDIW.Servicios;
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
                string? emailDelUsuario = User.Identity?.Name;
                UsuarioDTO usuario = _usuarioServicio.obtenerUsuarioPorEmail(emailDelUsuario);
  
                if (usuario != null)
                {
                    List<MotoDTO> misMotos = _motoServicio.obtenerMotosPorPropietarioId(usuario.Id);

                    if (misMotos != null) 
                    {
                        if (misMotos.Count > 0)
                            ViewBag.MisMotos = misMotos;
                    }
                }

                return View("~/Views/Home/misMotos.cshtml");
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
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
                string emailDelUsuario = User.Identity.Name;
                UsuarioDTO usuarioSesionActual = _usuarioServicio.obtenerUsuarioPorEmail(emailDelUsuario);
                MotoDTO nuevaMoto = new MotoDTO();
                nuevaMoto.IdPropietario = usuarioSesionActual.Id;
                return View("~/Views/Home/registroMoto.cshtml", nuevaMoto);
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
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
                bool motoRegistradaConExito = _motoServicio.registrarMoto(motoDTO);

                if (motoRegistradaConExito)
                {
                    ViewData["altaMotoExito"] = "Alta de la moto en el sistema OK";
                }
                else
                {
                    ViewData["altaMotoError"] = "No se pudo dar de alta la moto";
                }

                return View("~/Views/Home/misMotos.cshtml");
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud";
                return View("~/Views/Home/dashboard.cshtml");
            }
        }


    }
}
