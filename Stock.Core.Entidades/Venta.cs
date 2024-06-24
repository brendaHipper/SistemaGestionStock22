using System.ComponentModel.DataAnnotations.Schema;

namespace Stock.Core.Entidades
{
    [Table("Venta")]
    public class Venta
    {
        public int VentaId { get; set; }
        public DateTime Fecha { get; set; }

        // ProductoId (CLAVE FORANEA a Producto)
        [ForeignKey("ProductoId")]
        public int ProductoId { get; set; }

        public int Cantidad { get; set; }

        // UsuarioId (CLAVE FORANEA a Usuario)
        [ForeignKey("UsuarioId")]
        public int UsuarioId { get; set; }

        // Propiedad de navegación hacia Producto y Usuario
        public Producto Producto { get; set; }

        public Usuario Usuario { get; set; }
    }
}
