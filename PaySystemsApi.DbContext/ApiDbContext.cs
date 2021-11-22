using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PaySystemsAPI.Entities;
using System;

namespace PaySystemsApi.DataAccess
{
    //Aquí alojamos la configuración del proveedor de datos. No aplica ningún crud
    //Realizamos la migración aplicando -context IdentityDbContext para alojar las tablas de identity en la BD
    public class ApiDbContext : IdentityDbContext
    {
        public DbSet<UsuarioEntity> Usuario {get;set;}

        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Con esto EF no crea una tabla en la base de datos de tipo Entity. Estaclase Entity 
            //se usa como abstracción de todos los modelos que ameritan de persistencia
            modelBuilder.Ignore<Entity>();          
            base.OnModelCreating(modelBuilder);
        }
    }
}
