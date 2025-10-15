using asp_servicios.Nucleo;
using lib_dominio.Entidades;
using lib_dominio.Nucleo;
using lib_repositorios.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace asp_servicios.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class RolesController : ControllerBase
    {
        private readonly IRolesAplicacion? _rolesAplicacion;
        private readonly IAuditoriasAplicacion? _auditoriasAplicacion;
        private readonly TokenController? _tokenController;

        public RolesController(
            IRolesAplicacion? rolesAplicacion,
            IAuditoriasAplicacion? auditoriasAplicacion,
            TokenController? tokenController)
        {
            _rolesAplicacion = rolesAplicacion;
            _auditoriasAplicacion = auditoriasAplicacion;
            _tokenController = tokenController;
        }

        private Dictionary<string, object> ObtenerDatos()
        {
            var datos = new StreamReader(Request.Body).ReadToEnd().ToString();
            if (string.IsNullOrEmpty(datos))
                datos = "{}";
            return JsonConversor.ConvertirAObjeto(datos);
        }

        [HttpPost]
        public string Guardar()
        {
            var respuesta = new Dictionary<string, object>();
            try
            {
                var datos = ObtenerDatos();
                if (!_tokenController!.Validate(datos))
                {
                    respuesta["Error"] = "lbNoAutenticacion";
                    return JsonConversor.ConvertirAString(respuesta);
                }

                var entidad = JsonConversor.ConvertirAObjeto<Roles>(
                    JsonConversor.ConvertirAString(datos["Entidad"]!)
                );

                _rolesAplicacion!.Configurar(Configuracion.ObtenerValor("StringConexion"));
                var resultado = _rolesAplicacion.Guardar(entidad);

                // Auditoría (opcional)
                _auditoriasAplicacion!.Configurar(Configuracion.ObtenerValor("StringConexion"));
                _auditoriasAplicacion.PorAccion("Creación de rol", 1);

                respuesta["Entidad"] = resultado!;
                respuesta["Respuesta"] = "OK";
                respuesta["Fecha"] = DateTime.Now.ToString();
                return JsonConversor.ConvertirAString(respuesta);
            }
            catch (Exception ex)
            {
                respuesta["Error"] = ex.InnerException?.Message ?? ex.Message;
                return JsonConversor.ConvertirAString(respuesta);
            }
        }

        [HttpPost]
        public string Listar()
        {
            var respuesta = new Dictionary<string, object>();
            try
            {
                var datos = ObtenerDatos();
                if (!_tokenController!.Validate(datos))
                {
                    respuesta["Error"] = "lbNoAutenticacion";
                    return JsonConversor.ConvertirAString(respuesta);
                }

                _rolesAplicacion!.Configurar(Configuracion.ObtenerValor("StringConexion"));
                respuesta["Entidades"] = _rolesAplicacion.Listar();
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
    }
}

