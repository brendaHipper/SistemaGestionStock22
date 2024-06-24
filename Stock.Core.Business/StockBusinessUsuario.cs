using Stock.Core.DataEF;
using Stock.Core.Entidades;

namespace Stock.Core.Business
{
    public class StockBusinessUsuario
    {
        private readonly StockRepositoryUsuario _stockRepositoryUsuario;

        public StockBusinessUsuario(StockRepositoryUsuario userRepository)
        {
            this._stockRepositoryUsuario = userRepository;
        }

        public Usuario Autenticar(string nombre, string password)
        {
            var usuario = _stockRepositoryUsuario.ObtenerUsuarioPorNombre(nombre);

            if (usuario == null)
            {
                return null;
            }

            var hash = _stockRepositoryUsuario.GenerarHash(password, usuario.Salt);
            if (hash != usuario.Hash.Trim())
            {
                return null;
            }

            return usuario;
        }

        public bool RegistrarUsuario(string nombre, string password)
        {
            if (_stockRepositoryUsuario.ObtenerUsuarioPorNombre(nombre) != null)
            {
                return false; // Usuario ya existe
            }

            var salt = _stockRepositoryUsuario.GenerarSalt();
            var hash = _stockRepositoryUsuario.GenerarHash(password, salt);

            var usuario = new Usuario
            {
                Nombre = nombre,
                Hash = hash,
                Salt = salt
            };

            _stockRepositoryUsuario.CrearUsuario(usuario);
            return true;
        }
    }
}
