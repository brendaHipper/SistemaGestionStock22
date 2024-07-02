using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Core.Entidades.Interfaces
{
    public interface IRepositoryVenta
    {
        List<Venta> GetVentas();
        List<Producto> ObtenerProductos();
        List<Categoria> ObtenerCategorias();
        List<Producto> ObtenerProductosHabilitados();
        bool ProductoExiste(int productoId);
        int ObtenerStockDisponible(int productoId);
        void AgregarVenta(Venta venta);
    }
}
