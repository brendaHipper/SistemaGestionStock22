using System.ComponentModel.DataAnnotations.Schema;

namespace Stock.Core.Entidades
{
    [Table("Categoria")]
    public class Categoria
    {
        public int CategoriaId { get; set; }
        public string Nombre { get; set; }

        // Propiedad de navegación a Producto, una Lista de Productos
        public List<Producto> ListaProductos { get; set; }

        // Sobreescribo ToString para mostrarlo en consola
        public override string ToString()
        {
            return $"Categoría: {Nombre}";
        }
    }
}
