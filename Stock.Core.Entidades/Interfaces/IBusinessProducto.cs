using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Core.Entidades.Interfaces
{
    public interface IBusinessProducto
    {
        Producto GetProductoPorNombre(string nombre);
        List<Categoria> ObtenerCategorias();
        List<Producto> ObtenerProductosComprasYVentas();
        Producto AgregarProducto(Producto producto);
        Producto EliminarProducto(int ProductoId);
        Producto EditarProducto(Producto producto);
    }
}
