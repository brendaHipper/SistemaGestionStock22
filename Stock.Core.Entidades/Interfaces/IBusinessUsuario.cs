using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Core.Entidades.Interfaces
{
    public interface IBusinessUsuario
    {
        Usuario Autenticar(string nombre, string password);
        bool RegistrarUsuario(string nombre, string password);
    }
}
