using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace GestionStock.WebMVC.Models
{
    public class VentaViewModel
    {
        [Required(ErrorMessage = "El campo Producto es obligatorio.")]
        public int ProductoId { get; set; }

        [Required(ErrorMessage = "El campo Fecha es obligatorio.")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "El campo Cantidad es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser un número positivo.")]
        public int Cantidad { get; set; }

        public int UsuarioId { get; set; }

        public List<SelectListItem> Productos { get; set; }
    }
}
