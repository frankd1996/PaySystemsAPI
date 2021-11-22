using System;
using System.Collections.Generic;

#nullable disable

namespace PaySystemsAPI.EFModels
{
    public partial class Usuario
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string NombreUsuario { get; set; }
        public string Contraseña { get; set; }
    }
}
