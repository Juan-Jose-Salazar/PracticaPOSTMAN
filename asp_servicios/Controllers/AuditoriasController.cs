using asp_servicios.Nucleo;
using lib_dominio.Entidades; // asegúrate de importar el namespace donde está Auditorias
using lib_dominio.Nucleo;
using lib_repositorios.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace asp_servicios.Controllers
{
    [ApiController]
    [Route("[Controller]/[action]")]
    public class AuditoriasController : ControllerBase
    {
        private  IAuditoriasAplicacion? iAplicacion = null;
        private TokenController? tokenController = null;

        public AuditoriasController(IAuditoriasAplicacion iAplicacion, TokenController? tokenController)
        {
            this.iAplicacion = iAplicacion;
            this.tokenController = tokenController;
        }


        private Dictionary<string, object> ObtenerDatos()
        {
            var datos = new StreamReader(Request.Body).ReadToEnd().ToString();
            if (string.IsNullOrEmpty(datos))
                datos = "{}";
            return JsonConversor.ConvertirAObjeto(datos);
        }

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
    }
}

