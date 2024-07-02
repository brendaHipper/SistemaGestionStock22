using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Core.Entidades.Interfaces
{
    public interface IBusinessVenta
    {
        List<Venta> ObtenerVentas();
        List<Producto> ObtenerProductos();
        List<Producto> ObtenerProductosHabilitados();
        List<Categoria> ObtenerCategorias();
        void AgregarVenta(Venta venta);

    }
}
