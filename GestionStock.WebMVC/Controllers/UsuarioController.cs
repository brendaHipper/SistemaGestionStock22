using GestionStock.WebMVC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Stock.Core.Business;

namespace GestionStock.WebMVC.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly StockBusinessUsuario stockBusinessUsuario;

        public UsuarioController(StockBusinessUsuario usuarioBusiness)
        {
            this.stockBusinessUsuario = usuarioBusiness;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var usuario = stockBusinessUsuario.Autenticar(model.Nombre, model.Password);

            if (usuario == null)
            {
                ModelState.AddModelError("", "Nombre de usuario o contraseña incorrectos.");
                return View(model);
            }

            HttpContext.Session.SetInt32("UsuarioId", usuario.UsuarioId);
            return RedirectToAction("Index", "Producto");
        }

        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registro(RegistroViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool registrado = stockBusinessUsuario.RegistrarUsuario(model.Nombre, model.Password);

            if (!registrado)
            {
                ModelState.AddModelError("", "El nombre de usuario ya existe.");
                return View(model);
            }

            return RedirectToAction("Login", "Usuario");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UsuarioId");
            return RedirectToAction("Login", "Usuario");
        }

        // Una de las sobrecargas que tiene el método Action es que se le puede pasar el nombre de la Vista
        // Tambien puede indicarle que soporte 2 rutas para la vista:
        //[Route("/details-internal")]
        //[Route("/details")]
    }
}
