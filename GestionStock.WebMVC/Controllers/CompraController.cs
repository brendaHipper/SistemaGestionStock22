using GestionStock.WebMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Stock.Core.Business;
using Stock.Core.Entidades;

namespace GestionStock.WebMVC.Controllers
{
    public class CompraController : Controller
    {
        private readonly ILogger<CompraController> _logger;
        private readonly StockBusinessCompra _stockBusinessCompra;
        public CompraController(ILogger<CompraController> logger, StockBusinessCompra stockBusinessCompra)
        {
            _logger = logger;
            _stockBusinessCompra = stockBusinessCompra;
        }

        public IActionResult Index()
        {
            var compras = _stockBusinessCompra.ObtenerTotalCompras();
            return View(compras);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var userId = HttpContext.Session.GetInt32("UsuarioId");
            if (userId == null)
            {
                // Redirige al login si el usuario no está autenticado
                return RedirectToAction("Login", "Usuario");
            }
            var model = new CompraViewModel
            {
                Productos = GetProductosSelectList(),
                Fecha = DateTime.Now,
                UsuarioId = userId.Value
            };
            return View(model);
        }

        // Desplegable de Productos que aparece en el Formulario para el Registro de Compra
        private List<SelectListItem> GetProductosSelectList()
        {
            var productos = _stockBusinessCompra.ObtenerProductosHabilitados();
            return productos.Select(p => new SelectListItem
            {
                Text = p.Nombre,
                Value = p.ProductoId.ToString()
            }).ToList();
        }

        [HttpPost]
        public IActionResult Create(CompraViewModel model)
        {
            var compra = new Compra
            {
                ProductoId = model.ProductoId,
                Fecha = model.Fecha,
                Cantidad = model.Cantidad,
                UsuarioId = model.UsuarioId
            };

            try
            {
                _stockBusinessCompra.AgregarCompra(compra);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            model.Productos = GetProductosSelectList();
            //  var modelo = model;
            return View(model);
        }


    }
}
