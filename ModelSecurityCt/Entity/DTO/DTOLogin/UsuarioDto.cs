using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTO.DTOLogin
{
    public class UsuarioDto
    {
        public int UsuarioId { get; set; }
        public string Email { get; set; } = "";
        public string Rol { get; set; } = "";
    }
}
