using asp_servicios.Nucleo;
using lib_dominio.Entidades;
using lib_dominio.Nucleo;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace asp_servicios.Controllers
{
    [ApiController]
    [Route("[Controller]/[action]")]
    public class UsuariosController : ControllerBase
    {
        private IUsuariosAplicacion? iAplicacion = null;
        private IAuditoriasAplicacion? iAuditoria = null;
        private TokenController? tokenController = null;

        public UsuariosController(IUsuariosAplicacion? iAplicacion, IAuditoriasAplicacion? iAuditoria, TokenController? tokenController)
        {
            this.iAplicacion = iAplicacion;
            this.iAuditoria = iAuditoria;
            this.tokenController = tokenController;
        }

        // Igual que antes: lee el cuerpo del request y lo convierte en diccionario
        private Dictionary<string, object> ObtenerDatos()
        {
            var datos = new StreamReader(Request.Body).ReadToEnd().ToString();
            if (string.IsNullOrEmpty(datos))
                datos = "{}";
            return JsonConversor.ConvertirAObjeto(datos);
        }

        // ================== MÉTODOS ==================

        [HttpPost]
        public string Listar()
        {
            var respuesta = new Dictionary<string, object>();
            try
            {
                var datos = ObtenerDatos();
                if (!tokenController!.Validate(datos))
                {
                    respuesta["Error"] = "lbNoAutenticacion";
                    return JsonConversor.ConvertirAString(respuesta);
                }

                this.iAplicacion!.Configurar(Configuracion.ObtenerValor("StringConexion"));
                respuesta["Entidades"] = this.iAplicacion!.Listar();
                respuesta["Respuesta"] = "OK";
                respuesta["Fecha"] = DateTime.Now.ToString();
                return JsonConversor.ConvertirAString(respuesta);
            }
            catch (Exception ex)
            {
                respuesta["Error"] = ex.Message;
                return JsonConversor.ConvertirAString(respuesta);
            }
        }

        [HttpPost]
        public string Guardar()
        {
            var respuesta = new Dictionary<string, object>();
            try
            {
                var datos = ObtenerDatos();
                if (!tokenController!.Validate(datos))
                {
                    respuesta["Error"] = "lbNoAutenticacion";
                    return JsonConversor.ConvertirAString(respuesta);
                }

                var entidad = JsonConversor.ConvertirAObjeto<Usuarios>(JsonConversor.ConvertirAString(datos["Entidad"]!));
                this.iAplicacion!.Configurar(Configuracion.ObtenerValor("StringConexion"));
                var resultado = this.iAplicacion!.Guardar(entidad);

                // Registrar en auditoría
                iAuditoria!.Configurar(Configuracion.ObtenerValor("StringConexion"));
                iAuditoria.Guardar(new Auditorias
                {
                    IdAuditoria = entidad.IdUsuario,
                    Accion = "Creación de usuario",
                    Fecha = DateTime.Now,
                    
                });

                respuesta["Entidad"] = resultado!;
                respuesta["Respuesta"] = "OK";
                respuesta["Fecha"] = DateTime.Now.ToString();
                return JsonConversor.ConvertirAString(respuesta);
            }
            catch (Exception ex)
            {
                respuesta["Error"] = ex.Message;
                return JsonConversor.ConvertirAString(respuesta);
            }
        }

        [HttpPost]
        public string Modificar()
        {
            var respuesta = new Dictionary<string, object>();
            try
            {
                var datos = ObtenerDatos();
                if (!tokenController!.Validate(datos))
                {
                    respuesta["Error"] = "lbNoAutenticacion";
                    return JsonConversor.ConvertirAString(respuesta);
                }

                var entidad = JsonConversor.ConvertirAObjeto<Usuarios>(JsonConversor.ConvertirAString(datos["Entidad"]!));
                this.iAplicacion!.Configurar(Configuracion.ObtenerValor("StringConexion"));
                this.iAplicacion!.Modificar(entidad);

                // Auditoría
                this.iAuditoria!.Configurar(Configuracion.ObtenerValor("StringConexion"));
                this.iAuditoria!.Guardar(new Auditorias
                {
                    IdAuditoria = entidad.IdUsuario,
                    Accion = "Modificación de usuario",
                    Fecha = DateTime.Now,
                });

                respuesta["Entidad"] = entidad!;
                respuesta["Respuesta"] = "OK";
                respuesta["Fecha"] = DateTime.Now.ToString();
                return JsonConversor.ConvertirAString(respuesta);
            }
            catch (Exception ex)
            {
                respuesta["Error"] = ex.Message;
                return JsonConversor.ConvertirAString(respuesta);
            }
        }

        [HttpPost]
        public string Borrar()
        {
            var respuesta = new Dictionary<string, object>();
            try
            {
                var datos = ObtenerDatos();
                if (!tokenController!.Validate(datos))
                {
                    respuesta["Error"] = "lbNoAutenticacion";
                    return JsonConversor.ConvertirAString(respuesta);
                }

                // Usuario autenticado que realiza la acción
                var usuarioEjecutor = JsonConversor.ConvertirAObjeto<Usuarios>(
                    JsonConversor.ConvertirAString(datos["Usuario"]!)
                );

                // Usuario a eliminar
                var entidad = JsonConversor.ConvertirAObjeto<Usuarios>(
                    JsonConversor.ConvertirAString(datos["Entidad"]!)
                );

                this.iAplicacion!.Configurar(Configuracion.ObtenerValor("StringConexion"));
                this.iAplicacion!.Borrar(entidad);

                // Registrar auditoría
                this.iAuditoria!.Configurar(Configuracion.ObtenerValor("StringConexion"));
                this.iAuditoria!.Guardar(new Auditorias
                {
                    IdAuditoria = 0, // o autogenerado
                    Accion = $"El usuario {usuarioEjecutor.Name} eliminó a {entidad.Name}",
                    Fecha = DateTime.Now,
                    
                });

                respuesta["Entidad"] = entidad!;
                respuesta["Respuesta"] = "OK";
                respuesta["Fecha"] = DateTime.Now.ToString();
                return JsonConversor.ConvertirAString(respuesta);
            }
            catch (Exception ex)
            {
                var respuestaError = new Dictionary<string, object>
                {
                    ["Error"] = ex.Message,
                    ["Detalle"] = ex.InnerException?.Message ?? "Sin detalles internos"
                };
                return JsonConversor.ConvertirAString(respuestaError);
            }
        }

    }
}
