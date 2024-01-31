using Microsoft.AspNetCore.Mvc;

namespace BikerConnectDIW.Controllers
{
    public class RegistroController : Controller
    {
        [HttpGet]
        public IActionResult Registro()
        {
            return View("~/Views/Home/registro.cshtml");
        }
    }
}
