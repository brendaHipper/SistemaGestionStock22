using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Core.Entidades.Interfaces
{
    public interface IRepositoryProducto
    {
        List<Producto> ObtenerTodosLosProductos();
        List<Producto> ObtenerProductosComprasYVentas();
        Producto ObtenerProductoPorNombre(string nombre);
        Producto AgregarProducto(Producto producto);
        List<Categoria> ObtenerCategorias();
        Producto EditarProducto(Producto producto);
        Producto EliminarProducto(int ProductoId);

    }
}
