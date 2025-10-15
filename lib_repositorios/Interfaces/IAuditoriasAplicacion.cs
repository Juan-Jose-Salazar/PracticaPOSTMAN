using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib_dominio.Entidades;

namespace lib_repositorios.Interfaces
{
    public interface IAuditoriasAplicacion
    {
        void Configurar(string StringConexion);

        List<Auditorias> PorAccion(string accion, int IdUsuario);

        List<Auditorias> Listar();
        Auditorias? Guardar(Auditorias? entidad);
        Auditorias? Modificar(Auditorias? entidad);
        Auditorias? Borrar(Auditorias? entidad);
    }
}
