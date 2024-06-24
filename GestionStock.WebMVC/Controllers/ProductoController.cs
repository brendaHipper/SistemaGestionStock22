using GestionStock.WebMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Stock.Core.Business;
using Stock.Core.Entidades;

namespace GestionStock.WebMVC.Controllers
{
    public class ProductoController : Controller
    {
        private readonly ILogger<ProductoController> _logger;
        // declaro business para inicializarlo/construirlo en el controlador del Producto
        private readonly StockBusinessProducto _stockBusinessProducto;

        // Aplica Inyeccion de Dependencia a traves del Constructor
        public ProductoController(StockBusinessProducto stockBusiness,ILogger<ProductoController> logger)
        {
            _logger = logger;
            // Aplica Inyeccion de Dependencia a traves del parámetro que viene por Constructor
            _stockBusinessProducto = stockBusiness;
        }

        // Se envia el Modelo = productos
        // Obtiene los Productos con sus Compras Y Ventas
        public IActionResult Index()
        {
            var productos = _stockBusinessProducto.ObtenerProductosComprasYVentas();
            return View(productos);
            // La capa de negocio se encarga de traer de la capa de datos
            // var productos = _stockBusiness.ObtenerTodosLosProductos();
            //return View(productos);
        }

        // Controladores para los Formularios del CRUD
        // Acción para mostrar el formulario de eliminación
        // Elimina Producto desde la Web
        public IActionResult EliminarProducto(int id)
        {
            var producto = _stockBusinessProducto.ObtenerProductosComprasYVentas().FirstOrDefault(p => p.ProductoId == id);
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }

        // método POST para eliminar Producto
        [HttpPost, ActionName("EliminarProducto")]
        public IActionResult EliminarConfirmado(int id)
        {
            var producto = _stockBusinessProducto.ObtenerProductosComprasYVentas().FirstOrDefault(p => p.ProductoId == id);
            if (producto != null)
            {
                _stockBusinessProducto.EliminarProducto(id);
            }
            return RedirectToAction("Index");
        }

        // Crea Producto desde la Web
        [HttpGet]
        public IActionResult CrearProducto()
        {
            var model = new ProductoViewModel
            {
                Categorias = GetCategoriasSelectList()
            };
            return View(model);
        }

        private List<SelectListItem> GetCategoriasSelectList()
        {
            var categoria = _stockBusinessProducto.ObtenerCategorias();
            return categoria.Select(c => new SelectListItem
            {
                Text = c.Nombre,
                Value = c.CategoriaId.ToString()
            }).ToList();
        }

        [HttpPost]
        public IActionResult CrearProducto(ProductoViewModel model)
        {
            var producto = new Producto
            {
                Nombre = model.Nombre,
                CategoriaId = model.CategoriaId,
                Habilitado = model.Habilitado
            };

            try
            {
                _stockBusinessProducto.AgregarProducto(producto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            model.Categorias = GetCategoriasSelectList();
            return View(model);
        }

        // Edita Producto desde la Web
        [HttpGet]
        public IActionResult EditarProducto(int id)
        {
            var producto = _stockBusinessProducto.ObtenerProductosComprasYVentas().FirstOrDefault(p => p.ProductoId == id);
            
            if (producto == null)
            {
                return NotFound();
            }

            var viewModel = new ProductoViewModel
            {
                Nombre = producto.Nombre,
                CategoriaId = producto.CategoriaId,
                Habilitado = producto.Habilitado,
                Categorias = _stockBusinessProducto.ObtenerCategorias().Select(c => new SelectListItem
                {
                    Value = c.CategoriaId.ToString(),
                    Text = c.Nombre
                }).ToList()
            };

            return View(viewModel);
        }


        [HttpPost]
        public IActionResult EditarProducto(int id, ProductoViewModel model)
        {
            // Obtiene el producto existente
            var producto = _stockBusinessProducto.ObtenerProductosComprasYVentas().FirstOrDefault(p => p.ProductoId == id);
            if (producto == null)
            {
                return NotFound();
            }

            // Actualiza los campos del producto
            producto.Nombre = model.Nombre;
            producto.CategoriaId = model.CategoriaId;
            producto.Habilitado = model.Habilitado;

            // Edita el producto en la base de datos
            var resultado = _stockBusinessProducto.EditarProducto(producto);
            if (resultado != null)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }


        // Una de las sobrecargas que tiene el método Action es que se le puede pasar el nombre de la Vista
        // Tambien puede indicarle que soporte 2 rutas para la vista:
        //[Route("/details-internal")]
        //[Route("/details")]

    }

}
