using Stock.Core.DataEF;
using Stock.Core.Entidades;

namespace Stock.Core.Business
{
    // La capa de negocio solo llama a los métodos y conoce a la capa de entidades y DataEF. 
    public class StockBusinessCompra
    {
        // Usa repositorio en Business pero lo inyecta (p/evitar hacer new de 1 Clase)
        private readonly StockRepositoryCompra _stockRepositoryCompra;

        // Constructor para la capa de Business donde inyecto el respositorio de datos
        public StockBusinessCompra(StockRepositoryCompra stockRepository)
        {
            _stockRepositoryCompra = stockRepository;
        }

        // llamo al método Obtener todas las compras y que retorne de la Capa de Repositorio
        public List<Compra> ObtenerTotalCompras()
        {
            return _stockRepositoryCompra.GetObtenerTotalCompras();
        }

        public List<Producto> ObtenerProductos()
        {
            return _stockRepositoryCompra.ObtenerProductos();
        }

        // Llamo al método que filtra los Productos que se encuentran Habilitados. Habilitado(true) (::::agregado)
        public List<Producto> ObtenerProductosHabilitados()
        {
            return _stockRepositoryCompra.ObtenerProductosHabilitados();
        }

        // Desde Business llamo al Metodo para agregar Compra (llamada al método).
        // Controla que no se pueda cargar una compra en el futuro ni anterior a 7 dias atras
        public void AgregarCompra(Compra compra)
        {
            // Validar la fecha de la compra
            DateTime hoy = DateTime.Now;
            if (compra.Fecha > hoy)
            {
                throw new Exception("La fecha de la compra no puede ser en el futuro.");
            }

            if (compra.Fecha < hoy.AddDays(-7))
            {
                throw new Exception("La fecha de la compra no puede ser más de 7 días en el pasado.");
            }

            // Registrar la compra
            _stockRepositoryCompra.AgregarCompra(compra);
        }


    }
}
