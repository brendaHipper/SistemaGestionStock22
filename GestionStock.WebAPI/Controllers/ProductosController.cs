using Microsoft.AspNetCore.Mvc;
using Stock.Core.Business;

namespace GestionStock.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController: ControllerBase
    {
        private readonly StockBusinessVenta _stockBusinessVenta;

        public ProductosController(StockBusinessVenta stockBusinessVenta)
        {
            // Inyecto el business (servicio) de venta en el constructor
            _stockBusinessVenta = stockBusinessVenta;
        }

        [HttpGet("{id}/stock")]
        public IActionResult ObtenerStock(int id)
        {
            var (stock, fecha) = _stockBusinessVenta.ObtenerStockYFecha(id);

            if (stock == null && fecha == null)
            {
                return NotFound(new { message = "Producto no encontrado" });
            }

            return Ok(new { id, stock, fecha });
        }
    }
}
