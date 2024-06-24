using Stock.Core.DataEF;
using Stock.Core.Entidades;

namespace Stock.Core.Business
{
    // La capa de negocio solo llama a los métodos y conoce a la capa de entidades y DataEF. 
    public class StockBusinessVenta
    {
        // Usa repositorio en Business pero lo inyecta (p/evitar hacer new de 1 Clase)
        private readonly StockRepositoryVenta _stockRepositoryVenta;

        // Constructor para la capa de Business donde inyecto el respositorio de datos
        public StockBusinessVenta(StockRepositoryVenta stockRepository)
        {
            _stockRepositoryVenta = stockRepository;
        }

        // llamo al método Obtener todas las ventas y que retorne de la Capa de Repositorio
        public List<Venta> ObtenerVentas()
        {
            return _stockRepositoryVenta.GetVentas();
        }

        public List<Producto> ObtenerProductos()
        {
            return _stockRepositoryVenta.ObtenerProductos();
        }

        // Llamo al método que filtra los Productos que se encuentran Habilitados. Habilitado(true) (::::agregado)
        public List<Producto> ObtenerProductosHabilitados()
        {
            return _stockRepositoryVenta.ObtenerProductosHabilitados();
        }

        //::::: Llamo en Business al método para Agregar la Categoria.
        public List<Categoria> ObtenerCategorias()
        {
            return _stockRepositoryVenta.ObtenerCategorias();
        }

        // Desde Business llamo al Metodo para agregar Venta (llamada al método)
        // Controla en el formulario que no se pueda agregar una venta si no hay suficiente stock de un Producto
        public void AgregarVenta(Venta venta)
        {
            if (!ValidarStockDisponible(venta.ProductoId, venta.Cantidad))
            {
                throw new Exception("No hay suficiente stock.");
            }
            else 
            {
                // Registrar la venta tomando la fecha del sistema
                _stockRepositoryVenta.AgregarVenta(venta);
            }
        }

        // Validar si existe Stock disponible
        private bool ValidarStockDisponible(int productoId, int cantidadAVender)
        {
            // Obtener la cantidad actual de stock (compras - ventas)
            int stockActual = _stockRepositoryVenta.ObtenerStockDisponible(productoId);

            // Verificar si hay suficiente stock para la venta
            return stockActual >= cantidadAVender;
        }


        //------------------------------------------------------------------

        // Verifica si existe id producto, lo trae en la (API) 
        public bool ProductoExiste(int productoId)
        {
            return _stockRepositoryVenta.ProductoExiste(productoId);
        }

        // Método que retorna el Stock y la fecha de la última venta. (Método que llama la API en el controlador)
        // El parámetro 
        public (int? stock, string fecha) ObtenerStockYFecha(int productoId)
        {
            // Pregunta si no existe un producto id, retorna null para el stock y null para la fecha
            if (!_stockRepositoryVenta.ProductoExiste(productoId))
            {
                return (null, null);
            }

            var stock = _stockRepositoryVenta.ObtenerStockDisponible(productoId);
            var fecha = _stockRepositoryVenta.ObtenerFechaUltimaVenta(productoId);
            return (stock, fecha);
        }

    }
}
