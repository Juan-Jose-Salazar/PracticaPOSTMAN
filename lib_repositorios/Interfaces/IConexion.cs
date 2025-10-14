using Microsoft.EntityFrameworkCore;
using lib_dominio.Entidades;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace lib_repositorios.Interfaces
{
    public interface IConexion
    {
        string? StringConexion { get; }

        DbSet<Usuarios>? Usuarios { get; set; }
        DbSet<Roles>? Roles { get; set; }
        DbSet<Auditorias>? Auditorias { get; set; }

        EntityEntry<T> Entry<T>(T entity) where T : class;
        int SaveChanges();

    }
}
