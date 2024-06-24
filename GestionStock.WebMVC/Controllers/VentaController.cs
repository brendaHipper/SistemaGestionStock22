using GestionStock.WebMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Stock.Core.Business;
using Stock.Core.Entidades;

namespace GestionStock.WebMVC.Controllers
{
    public class VentaController : Controller
    {
        private readonly ILogger<VentaController> _logger;
        private readonly StockBusinessVenta _stockBusinessVenta;
        public VentaController(ILogger<VentaController> logger, StockBusinessVenta stockBusinessVenta)
        {
            _logger = logger;
            _stockBusinessVenta = stockBusinessVenta;
        }

        public IActionResult Index()
        {
            var venta = _stockBusinessVenta.ObtenerVentas();
            return View(venta);
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
            var model = new VentaViewModel
            {
                Productos = GetProductosSelectList(),
                Fecha = DateTime.Now,
                UsuarioId = userId.Value
            };
            return View(model);
        }

        // Desplegable de Productos que aparece en el Formulario para el Registro de Venta
        private List<SelectListItem> GetProductosSelectList()
        {
            var productos = _stockBusinessVenta.ObtenerProductosHabilitados();
            return productos.Select(p => new SelectListItem
            {
                Text = p.Nombre,
                Value = p.ProductoId.ToString()
            }).ToList();
        }

        [HttpPost]
        public IActionResult Create(VentaViewModel model)
        {
            var venta = new Venta
            {
                ProductoId = model.ProductoId,
                Fecha = DateTime.Now,
                Cantidad = model.Cantidad,
                UsuarioId = model.UsuarioId
            };

            try
            {
                _stockBusinessVenta.AgregarVenta(venta);
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
