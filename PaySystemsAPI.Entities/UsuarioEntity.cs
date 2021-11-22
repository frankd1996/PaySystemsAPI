using Microsoft.AspNet.Identity.EntityFramework;
using System;

//En esta capa definieremos las entidades o modelos del negocio
namespace PaySystemsAPI.Entities
{
    public class UsuarioEntity: Entity
    {        
        public string NombreCompleto { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string NombreUsuario { get; set; }
        public string Contraseña { get; set; }
    }
}
