using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using PaySystemsApi.DataAccess;
using PaySystemsAPI.Application;
using PaySystemsAPI.DTOs;
using PaySystemsAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PaySystemsAPI.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]    
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase //Controlador de uso común. Solo amerita del decorador Authorize
    {
        IApplication<UsuarioEntity> _usuario;
        private readonly UserManager<IdentityUser> _userManager;//Manejador de usuarios
        public UsuarioController(IApplication<UsuarioEntity> usuario, IServiceProvider serviceProvider)
        {
            _usuario = usuario;
            _userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();//usamos esta nomenclatura cuando se inyecta más de una clase juntas en startup
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_usuario.GetAll());            
        }

        //[Authorize(Roles = "Admin")]
        //[HttpGet]
        //public async Task<IActionResult> GetById([FromBody] RegisterUserRequestDTO user)
        //{
        //    var existingUser = await _userManager.FindByEmailAsync(user.Email);
        //    //return Ok(_usuario.GetById(id));
        //}

        [HttpPost]
        public IActionResult Post(UsuarioEntity usuario)
        {
            var f = new UsuarioEntity()
            {
                NombreCompleto = usuario.NombreCompleto,
                Contraseña = usuario.Contraseña,
                Email = usuario.Email,
                Id = usuario.Id,
                NombreUsuario = usuario.NombreUsuario,
                Telefono = usuario.Telefono
            };
            return Ok(_usuario.Save(f));            
        }
    }
}
