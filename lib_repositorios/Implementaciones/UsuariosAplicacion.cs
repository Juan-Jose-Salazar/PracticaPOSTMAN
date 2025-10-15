using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;



namespace lib_repositorios.Implementaciones
{
    public class UsuariosAplicacion : IUsuariosAplicacion
    {
        private IConexion? IConexion = null;
        public UsuariosAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }
        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }
        public Usuarios? Borrar(Usuarios? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            var usuarioExistente = this.IConexion!.Usuarios!.FirstOrDefault(u => u.IdUsuario == entidad.IdUsuario);
              
            if (usuarioExistente == null)
                throw new Exception("El usuario no existe o ya fue eliminado");
            this.IConexion!.Usuarios!.Remove(usuarioExistente);
            this.IConexion.SaveChanges();
            return usuarioExistente;
        }
        public Usuarios? Guardar(Usuarios? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");



            var usuarioExistente = this.IConexion!.Usuarios!.FirstOrDefault(u => u.Email == entidad.Email);

            if (usuarioExistente != null)
                throw new Exception("El usuario ya existe. Use el método Actualizar.");
            this.IConexion!.Usuarios!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }
        public List<Usuarios> Listar()
        {
            return this.IConexion!.Usuarios!.Take(20).ToList();
        }
        public List<Usuarios> PorNombre(Usuarios? entidad)
        {
            return this.IConexion!.Usuarios!
            .Where(x => x.Name!.Contains(entidad!.Name!))
            .ToList();
        }
        public Usuarios? Modificar(Usuarios? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad!.IdUsuario == 0)
                throw new Exception("lbNoSeGuardo");
            var entry = this.IConexion!.Entry<Usuarios>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }
    }
}

