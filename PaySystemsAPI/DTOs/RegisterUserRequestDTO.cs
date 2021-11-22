using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PaySystemsAPI.DTOs
{
    //En esta clase se registran los campos necesarios que el usuario enviará para poder registrarse
    public class RegisterUserRequestDTO
    {
        [Required]
        public string NombreCompleto { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Telefono { get; set; }
        [Required]
        public string NombreUsuario { get; set; }
        [Required]
        public string Contraseña { get; set; }
    }
}
