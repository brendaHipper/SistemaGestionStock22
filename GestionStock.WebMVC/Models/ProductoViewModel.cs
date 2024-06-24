using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using Stock.Core.Entidades;

namespace GestionStock.WebMVC.Models
{
    // No esta en la base de datos. No lo necesito en la capa de Entidades. Solo la utilizo para crear una Vista
    public class ProductoViewModel
    {
        [Required(ErrorMessage = "Ingrese el nombre del Producto por favor.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Seleccione una categoría por favor.")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione una categoría válida.")]
        public int CategoriaId { get; set; }
        public bool Habilitado { get; set; }

        public List<SelectListItem> Categorias { get; set; }
    }
}
