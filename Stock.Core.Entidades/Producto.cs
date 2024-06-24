using System.ComponentModel.DataAnnotations.Schema;

namespace Stock.Core.Entidades
{
    [Table("Producto")]
    public class Producto
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; }
        public bool Habilitado { get; set; }

        [ForeignKey("CategoriaId")]
        public int CategoriaId { get; set; }

        // Propiedades de navegación a Categoria, Compras y Ventas
        public Categoria Categoria { get; set; }
        public List<Compra> Compras { get; set; }
        public List<Venta> Ventas { get; set; }

        // La Anotación NotMapped indica que el atributo no será mapeado a la Base de Datos.
        // La Propiedad Stock no se almacenará en la Base de datos porque es un atributo calculado necesario para mostrarlo en la Vista.
        [NotMapped]
        public int? Stock
        {
            get
            {
                if (Compras == null || !Compras.Any() )
                {
                    return 0;
                }
                int totalCompras = Compras.Sum(c => c.Cantidad);

                int totalVentas;
                if (Ventas == null || !Ventas.Any())
                {
                    totalVentas = 0;
                }
                else {
                    totalVentas = Ventas.Sum(v => v.Cantidad);
                }
                int stock = totalCompras - totalVentas;

                return stock >= 0 ? (int?)stock : 0;
            }
            set
            {
                // Necesario para asignar el valor y poder visualizarlo/mostrarlo
            }
        }

        // Sobreescribo ToString para mostrarlo en consola
        public override string ToString()
        {
            return $"Producto: ({ProductoId}), {Nombre}";
        }
    }
}
