using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib_dominio.Entidades;

namespace lib_repositorios.Interfaces
{
    public interface IRolesAplicacion
    {
        void Configurar(string StringConexion);

        List<Roles> PorNombre(Roles? entidad);

        List<Roles> Listar();

        Roles? Guardar(Roles? entidad);

        Roles? Modificar(Roles? entidad);
    }
}
