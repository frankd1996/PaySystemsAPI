using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaySystemsApi.Abstractions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace PaySystemsAPI.Services
{
    public interface ITokenHandlerService
    {
        string GenerateJwtToken(ITokenParameters pars);
    }
    public class TokenHandlerService:ITokenHandlerService
    {
        private readonly JwtConfig _jwtConfig;
        //Inyectamos la configuración de la estructura del JWT que usaremos. Esto se definió en StartUp y en 
        //appsettings.json
        public TokenHandlerService(IOptionsMonitor<JwtConfig> optionsMonitor)
        {
            _jwtConfig = optionsMonitor.CurrentValue;
        }

        /// <summary>
        /// Este método genera y retorna el token que tendrá una duración de tiempo limitada (se configura aquí ese tiempo)
        /// </summary>
        /// <param name="pars"></param>
        /// <returns></returns>
        public string GenerateJwtToken(ITokenParameters pars)
        {
            //Instancia que maneja el token. Se crea a través de él
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            //Obtenemos en bytes el key de jwt definido en StartUp y en 
            //appsettings.json
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            //definimos los claims del token (los claims es la información que se compartirá y su configuración)
            //en la creación del jwt se usa la key definida en StartUp y en appsettings.json y los claims
            //para generar el token
            var claims = new List<Claim>
            {
                new Claim("Id", pars.Id),
                new Claim(JwtRegisteredClaimNames.Sub, pars.UserName),
                new Claim(JwtRegisteredClaimNames.Email, pars.UserName),
            };

            //agregamos los roles del usuario como claims, esto para que se cree el token considerando estos roles
            // y funcione la autodrizacion por roles [Authorize = "Admin"]
            AddRolesToClaims(claims, pars.Role);
            var claimsIdentity = new ClaimsIdentity(claims);

            //Definimos las configuraciones del token
            //Subject --> claims
            //Expires --> tiempo de expiración del token
            //SigningCredentials --> Algoritmos de encriptación del token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddYears(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };
            
            //Creamos el token
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);

            //serealizamos el token en un formato de serialización compacto (string)
            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;
        }

        /// <summary>
        /// Recorremos los roles del usuario para agregarlos a la lista de claims con los que se 
        /// configurará el token
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="roles"></param>
        private void AddRolesToClaims(List<Claim> claims, IEnumerable<string> roles)
        {
            foreach (var role in roles)
            {
                var roleClaim = new Claim(ClaimTypes.Role, role);
                claims.Add(roleClaim);
            }
        }
    }
}
