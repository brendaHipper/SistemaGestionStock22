using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Core.Entidades.Interfaces
{
    public interface IRepositoryUsuario
    {
        Usuario ObtenerUsuarioPorNombre(string nombre);
        void CrearUsuario(Usuario usuario);
        string GenerarHash(string input, string salt);
        string GenerarSalt();
    }
}
