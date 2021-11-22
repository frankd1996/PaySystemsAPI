using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using PaySystemsApi.Abstractions;
using PaySystemsApi.DataAccess;
using PaySystemsAPI.Application;
using PaySystemsAPI.Repository;
using PaySystemsAPI.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PaySystemsAPI.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using PaySystemsAPI.Services;

namespace PaySystemsAPI
{
    public class Startup
    {
        //private readonly string _MyCors = "MyCors";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var ConnectionString = Configuration["ConnectionString"];

            //Agregamos estos servicios si queremos habilitar MVC (para construcción de vistas)
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddMvc().AddNewtonsoftJson();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PaySystemsAPI", Version = "v1" });
            });

            //Inyectamos el DbContext de SQL Server como servicio 
            services.AddDbContext<ApiDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //inyectamos IOptionsMonitor<JwtConfig> para que se resuelva como JwtConfig en la clase TokenHandlerService
            //JwtConfig se encuentra en appsettings.json
            services.Configure<JwtConfig>(Configuration.GetSection("JwtConfig"));

            //agregamos el servicio de autenticación de ASP.NET Core
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwt => {
                var key = Encoding.ASCII.GetBytes(Configuration["JwtConfig:Secret"]);

                jwt.SaveToken = true;
                jwt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = false,
                    ValidateLifetime = true
                };
            });
            
            //Inyectamos usuarios con su rol para usar en el controlodaror AuthController
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                //Configuraciones de identidad y contraseña
                options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;                

            }).AddEntityFrameworkStores<ApiDbContext>()
            .AddDefaultTokenProviders();

            //agregamos los servicios para uso de arquitectura o de lógica de negocios
            services.AddScoped(typeof(IApplication<>), typeof(Application<>));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IDbContext<>), typeof(DbContext<>));
            services.AddScoped(typeof(ITokenHandlerService), typeof(TokenHandlerService));

            //Agregamos las políticas para permitir conexiones a esta API
            //services.AddCors(options =>
            //{
            //    options.AddPolicy(name: _MyCors, builder =>
            //     {
            //        //builder.SetIsOriginAllowed(origin => new Uri(origin).Host == "localHost")
            //        //.AllowAnyHeader().AllowAnyMethod();
            //        builder.AllowAnyOrigin()
            //         .AllowAnyMethod()
            //         .AllowAnyHeader();
            //     });
            //});
        ;}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //El orden de declaración afecta el middleware. checar
        //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-6.0#middleware-order
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PaySystemsAPI v1"));
            }

            //El cors da los permisos de acceso a la api desde otros clientes. Funciona con un web fronted,
            //pero no en mobile. usar conveyor by keyoti en visual studio o cualquier redireccionador de url de
            //la api a url remoto
            //app.UseCors(_MyCors);

            app.UseHttpsRedirection();

            app.UseRouting();            

            //Con eso se configura la app para protocolos de autorización de Endpoints (decoradores authorize)
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //Agregamos esta configuración si queremos habilitar MVC (para construcción de vistas)
            app.UseMvc();

            //Con esto la aplicación está configurada para aceptar lógica de autenticación con Identity
            app.UseAuthentication();

            
        }
    }
}
