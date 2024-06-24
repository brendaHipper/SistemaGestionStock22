using Stock.Core.Entidades;
using Stock.Core.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Stock.Core.DataEF
{
    public class StockRepositoryVenta
    {
        // Aplica Inyeccion de Dependencia a traves del Constructor
        private readonly Config _config;
        public StockRepositoryVenta(Config config)
        {
            _config = config;
        }

        // Método para Obtener las Ventas asociando las entidades relacionadas de Producto y Usuario
        public List<Venta> GetVentas()
        {
            // CONSULTA CON LINQ
            using (var db = new StockContext(_config))
            {
                return db.Ventas
                    .Include(c => c.Producto)
                    .Include(c => c.Usuario)
                    .ToList();
            }
        }

        public List<Producto> ObtenerProductos()
        {
            using (var db = new StockContext(_config))
            {
                return db.Productos.ToList();
            }
        }

        //::: Obtiene las Categorias
        public List<Categoria> ObtenerCategorias()
        {
            using (var db = new StockContext(_config))
            {
                return db.Categorias.ToList();
            }

        }

        // Método para filtrar los Productos que están Habilitados (::::agregado)
        public List<Producto> ObtenerProductosHabilitados()
        {
            using (var db = new StockContext(_config))
            {
                return db.Productos.Where(p => p.Habilitado).ToList();
            }
        }

        // Método que valida que exista un producto id para traerlo en la (API)
        public bool ProductoExiste(int productoId)
        {
            using (var db = new StockContext(_config))
            {
                return db.Productos.Any(p => p.ProductoId == productoId);
            }
            
        }

        // Obtiene el Stock en el Repositorio de la Venta
        public int ObtenerStockDisponible(int productoId)
        {
            using (var db = new StockContext(_config))
            {
                int compras = db.Set<Compra>()
                                   .Where(c => c.ProductoId == productoId)
                                   .Sum(c => c.Cantidad);

                int ventas = db.Set<Venta>()
                                      .Where(v => v.ProductoId == productoId)
                                      .Sum(v => v.Cantidad);

                return compras - ventas;
            }
        }

        // Método para Agregar una Venta
        public void AgregarVenta(Venta venta)
        {
            using (var db = new StockContext(_config))
            {
                db.Ventas.Add(venta);
                db.SaveChanges();
            }
        }

        //------------------------------------------------------------------

        // Método para obtener la fecha de la última venta (API)
        public string ObtenerFechaUltimaVenta(int productoId)
        {
            using (var db = new StockContext(_config))
            {
                var ultimaVenta = db.Set<Venta>()
                                     .Where(v => v.ProductoId == productoId)
                                     .OrderByDescending(v => v.Fecha)
                                     .FirstOrDefault();

                if (ultimaVenta != null)
                {
                    return ultimaVenta.Fecha.ToString("dd/MM/yyyy");
                }
                else
                {
                    return null;
                }
            }
        }


    }
}
