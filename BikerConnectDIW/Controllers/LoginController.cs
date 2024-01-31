using DAL.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace BikerConnectDIW.Controllers
{
    public class LoginController : Controller
    {

        [HttpGet]
        public IActionResult Login()
        {
            return View("~/Views/Home/login.cshtml");
        }
    }
}
