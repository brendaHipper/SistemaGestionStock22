using Stock.Core.Configuration;
using Stock.Core.Entidades;
using System.Security.Cryptography;
using System.Text;

namespace Stock.Core.DataEF
{
    public class StockRepositoryUsuario
    {
        private readonly Config _config;
        public StockRepositoryUsuario(Config config)
        {
            _config = config;
        }

        public Usuario ObtenerUsuarioPorNombre(string nombre)
        {
            var resultado = new Usuario();

            // CONSULTA CON LINQ
            using (var db = new StockContext(_config))
            {
                resultado = db.Set<Usuario>().FirstOrDefault(u => u.Nombre == nombre);
            }

            return resultado;
        }

        public void CrearUsuario(Usuario usuario)
        {
            using (var db = new StockContext(_config))
            {
                db.Set<Usuario>().Add(usuario);
                db.SaveChanges();
            }            
        }

        public string GenerarHash(string input, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                // Uso de Trim() para quitar los espacios en Blanco
                var combinedInput = input.Trim() + salt.Trim();
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combinedInput));
                return Convert.ToBase64String(bytes).Trim();
            }
        }

        public string GenerarSalt()
        {
            var buffer = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(buffer);
            }
            return Convert.ToBase64String(buffer).Trim();
        }
    }
}
