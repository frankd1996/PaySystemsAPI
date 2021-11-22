using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using PaySystemsAPI.Configuration;
using PaySystemsAPI.DTOs;
using PaySystemsAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaySystemsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase //aqui controlamos el login y registro de usuarios
    {
        private readonly UserManager<IdentityUser> _userManager;//Manejador de usuarios
        private readonly ITokenHandlerService _service;
        private readonly RoleManager<IdentityRole> _roleManager;//Manejador de roles 
        public AuthController(ITokenHandlerService tokenHandlerService, IServiceProvider serviceProvider)
        {
            _userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();//usamos esta nomenclatura cuando se inyecta más de una clase juntas en startup
            _service = tokenHandlerService; 
            _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        }

        /// <summary>
        /// Aquí se encuentra contenida toda la lógica de registro de un nuevo usuario
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequestDTO user)
        {
            if(ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(user.Email);
                if(existingUser != null)
                {
                    return BadRequest("El correo electrónico indicado ya está registrado");
                }

                //verificamos si el rol existe. Si no existe lo creamos y asignamos 
                if (!await _roleManager.RoleExistsAsync("Admin"))
                {
                    var role = new IdentityRole();
                    role.Name = "Admin";
                    var roleIsCreated = await _roleManager.CreateAsync(role);
                }
                var userIdentity = new IdentityUser()
                {
                    Email = user.Email,
                    UserName = user.NombreUsuario,
                };
                //creamos el nuevo usuario
                var userIsCreated = _userManager.CreateAsync(userIdentity,user.Contraseña);               
                
                if (userIsCreated.Result.Succeeded)
                {
                    //obtenemos el usuario que acabamos de guardar
                    var currentUser = await _userManager.FindByEmailAsync(user.Email);

                    //creamos el registro de la tabla transitiva que relaciona a usuarios y roles (UserRole)
                    var result1 = await _userManager.AddToRoleAsync(userIdentity, "Admin");

                    var roles = await _userManager.GetRolesAsync(userIdentity);

                    //creamos el modelo de token en función de los datos del usuario
                    var pars = new TokenParameters()
                    {
                        Id = currentUser.Id,
                        PasswordHash = currentUser.PasswordHash,
                        UserName = currentUser.UserName,
                        Role = roles,
                    };

                    //Creamos el Token específico para este usuario
                    var jwtToken = _service.GenerateJwtToken(pars);

                    return Ok(new UserLoginResponseDTO()
                    {
                        Login = true,
                        Token = jwtToken
                    });
                }
                else
                {
                    return BadRequest(userIsCreated.Result.Errors.Select(x => x.Description).ToList());
                }
            }
            else
            {
                return BadRequest("Se produjo un error en el registro");
            }
        }

        //En la capa de servicios vamos a manejar toda la lógica de inicio de sesión (manejo de token)
        //ver TokenHandlerService
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(UserLoginRequestDTO user)
        {
            if (ModelState.IsValid)
            {                
                var existingUser = await _userManager.FindByEmailAsync(user.Email);
                var roles = await _userManager.GetRolesAsync(existingUser);
                if (existingUser == null)
                {
                    return BadRequest(new UserLoginResponseDTO()
                    {
                        Login = false,
                        Errors = new List<String>()
                        {
                            "Usuario o contraseña incorrecto!"
                        }
                    });
                }

                var isCorrect = await _userManager.CheckPasswordAsync(existingUser, user.Password);

                if (isCorrect)
                {
                    var pars = new TokenParameters()
                    {
                        Id = existingUser.Id,
                        PasswordHash = existingUser.PasswordHash,
                        UserName = existingUser.UserName,
                        Role = roles,
                    };

                    var jwtToken = _service.GenerateJwtToken(pars);

                    return Ok(new UserLoginResponseDTO()
                    {
                        Login = true,
                        Token = jwtToken
                    });

                }
                else
                {
                    return BadRequest(new UserLoginResponseDTO()
                    {
                        Login = false,
                        Errors = new List<String>()
                        {
                            "Usuario o contraseña incorrecto!"
                        }
                    });
                }

            }
            else
            {
                return BadRequest(new UserLoginResponseDTO()
                {
                    Login = false,
                    Errors = new List<String>()
                    {
                        "Usuario o contraseña incorrecto!"
                    }
                });
            }
        }
    }
}
