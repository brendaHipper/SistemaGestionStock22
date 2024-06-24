using Stock.Core.DataEF;
using Stock.Core.Entidades;

namespace Stock.Core.Business
{
    // La capa de negocio solo llama a los métodos y conoce a la capa de entidades y DataEF. 
    // Con esto represento la parte de Abstracción de Datos
    public class StockBusinessProducto
    {
        // Usa repositorio en Business pero lo inyecta (p/evitar hacer new de la clase)
        private readonly StockRepositoryProducto _stockRepositoryProducto;

        // Constructor para la capa de Business
        public StockBusinessProducto(StockRepositoryProducto stockRepository)
        {
            _stockRepositoryProducto = stockRepository;
        }

        public Producto GetProductoPorNombre(string nombre)
        {
            return _stockRepositoryProducto.ObtenerProductoPorNombre(nombre);
        }

        //Llamo en Business al método para Agregar la Categoria.
        public List<Categoria> ObtenerCategorias()
        {
            return _stockRepositoryProducto.ObtenerCategorias();
        }

        // llamo al método Obtener todos los productos y que retorne de la Capa de Repositorio
        public List<Producto> ObtenerProductosComprasYVentas()
        {
            return _stockRepositoryProducto.ObtenerProductosComprasYVentas();
        }

        // ::::::::::::::: CRUD
        // Desde Business llamo al Metodo para agregar Producto (llamada al método)
        public Producto AgregarProducto(Producto producto)
        {
            return _stockRepositoryProducto.AgregarProducto(producto);
        }
        

        // Llama al Método para eliminar
        public Producto EliminarProducto(int ProductoId)
        {
            return _stockRepositoryProducto.EliminarProducto(ProductoId);
        }

        // Llamo al método para modificar un producto
        public Producto EditarProducto(Producto producto)
        {
            return _stockRepositoryProducto.EditarProducto(producto);
        }

    }
}
