using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lib_dominio.Entidades
{
    public class Usuarios
    {
        [Key]
        public int IdUsuario { get; set; }
        [Required]
        public string Name { get; set; } 
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        public int IdRol { get; set; }

        [ForeignKey("IdRol")] public Roles? _Rol { get; set; }

    }
}
