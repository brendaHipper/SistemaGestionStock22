using System.ComponentModel.DataAnnotations;

namespace GestionStock.WebMVC.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo Contraseña es obligatorio.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

}
