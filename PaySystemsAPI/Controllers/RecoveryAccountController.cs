using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaySystemsApi.DataAccess;
using PaySystemsAPI.Application;
using PaySystemsAPI.DTOs;
using PaySystemsAPI.EFModels;
using PaySystemsAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaySystemsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecoveryAccountController : ControllerBase
    {
        private readonly IApplication<UsuarioEntity> _usuario;
        public RecoveryAccountController(IApplication<UsuarioEntity> usuario)
        {
            _usuario = usuario;
        }
        public ActionResult Index()
        {
            return Ok();
        }

        [HttpGet]
        public ActionResult StartRecovery()
        {
            return Ok();
        }

        [HttpPost]
        public ActionResult StartRecovery([FromBody] UserRecoveryAccountDTO email)
        {
            if(!ModelState.IsValid)
            {
                BadRequest("Este correo es inválido");
            }
            else
            {
                using(PaySystemsDBContext dbContext = new PaySystemsDBContext())
                {
                    var usuario = dbContext.Usuarios.Where(x => x.Email==email.Email).FirstOrDefault();
                    if(usuario != null)
                    {

                    }
                    else
                    {
                        BadRequest("El correo es válido pero no existe");
                    }
                }
                    
            }
                
            return Ok();
        }

        [HttpPost]
        public ActionResult Recovery()
        {
            return Ok();
        }

    }
}
