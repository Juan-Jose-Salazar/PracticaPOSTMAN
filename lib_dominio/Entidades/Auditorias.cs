using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace lib_dominio.Entidades
{
    public class Auditorias
    {
        [Key]
        public int IdAuditoria { get; set; }

        public string Usuario { get; set; }

        public string Accion { get; set; }
        public DateTime Fecha { get; set; }

    }
}
