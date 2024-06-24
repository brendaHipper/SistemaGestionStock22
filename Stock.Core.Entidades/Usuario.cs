using System.ComponentModel.DataAnnotations.Schema;

namespace Stock.Core.Entidades
{
    [Table("Usuario")]
    public class Usuario
    {
        public int UsuarioId { get; set; }
        public string Nombre { get; set; }

        public string Hash { get; set; }
        public string Salt { get; set; }

        // Propiedad de navegación a Compra (Lista de Compras)
        public List<Compra> Compras { get; set; }

        // Propiedad de navegación a Venta (Lista de Ventas)
        public List<Venta> Ventas { get; set; }
    }
}
