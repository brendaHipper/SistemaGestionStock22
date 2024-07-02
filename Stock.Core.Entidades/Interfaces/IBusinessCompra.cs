using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Core.Entidades.Interfaces
{
    public interface IBusinessCompra
    {
        List<Compra> ObtenerTotalCompras();

        List<Producto> ObtenerProductos();

        List<Producto> ObtenerProductosHabilitados();

        void AgregarCompra(Compra compra);

    }
}
