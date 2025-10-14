using Microsoft.EntityFrameworkCore;
using lib_repositorios.Interfaces;
using lib_dominio.Entidades;

namespace lib_repositorios.Implementaciones
{
    public class Conexion : DbContext, IConexion
    {
        public string StringConexion { get; set; }

        // 🔹 Constructor sin parámetros (para Add-Migration)
        public Conexion()
        {
            StringConexion = "Server=localhost;Database=PracticaPOSTMAN;Trusted_Connection=True;TrustServerCertificate=True;";
        }

        // 🔹 Constructor con opciones (usado en tiempo de ejecución)
        public Conexion(DbContextOptions<Conexion> options) : base(options)
        {
        }

        // 🔹 Configuración de conexión (solo si no está ya en el Startup)
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(StringConexion);
            }
        }


        public DbSet<Usuarios>? Usuarios { get; set; }
        public DbSet<Roles>? Roles { get; set; }
        public DbSet<Auditorias>? Auditorias { get; set; }

    }
}
