using Stock.Core.Entidades;
using Stock.Core.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Stock.Core.DataEF
{
    public class StockRepositoryCompra
    {
        // Aplica Inyeccion de Dependencia a traves del Constructor
        private readonly Config _config;
        public StockRepositoryCompra(Config config)
        {
            _config = config;
        }

        //  Trae Fecha | NombreProducto | Cantidad (producto) | NombreUsuario 
        public List<Compra> GetObtenerTotalCompras()
        {
            // CONSULTA CON LINQ
            using (var db = new StockContext(_config))
            {
                //return db.Compras
                //    .Include(c => c.Producto)
                //    .Include(c => c.Usuario)
                //    .ToList();
                // <<:::::::::::: Optimización de la Consulta >>
                var producto = (from c in db.Compras
                           join p in db.Productos on c.ProductoId equals p.ProductoId
                           join u in db.Usuarios on c.UsuarioId equals u.UsuarioId
                           where c.ProductoId == p.ProductoId
                           
                           select new Compra
                           {
                               CompraId = c.CompraId,
                               Fecha = c.Fecha,
                               Cantidad = c.Cantidad,
                               Producto = p,
                               Usuario = u,
                           }).ToList();
                return producto;
            }
        }

        public List<Producto> ObtenerProductos()
        {
            using (var db = new StockContext(_config))
            {
                return db.Productos.ToList();
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

        // Método para Agregar Compra una compra
        public void AgregarCompra(Compra compra)
        {
            using (var db = new StockContext(_config))
            {
                db.Compras.Add(compra);
                db.SaveChanges();
            }
        }

    }
}
