
using PharmaStock.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using PharmaStock.Repositories;
using PharmaStock.Services;
using PharmaStock.Hubs;

namespace PharmaStock
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSignalR();//agrega el servicio de signalR
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("MiBaseDeDatos")));//conecta a la base de datos
            services.AddScoped<ProductRepository>();
            services.AddScoped<AuthService>();
            services.AddScoped<UserRepository>();
            services.AddHostedService<AlertService>();
            services.AddHttpClient<PredictionService>();
            services.AddScoped<ProveedorService>();
            services.AddScoped<EmailService>();


            // Configuración de JWT
            var secretKey = Configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new ArgumentNullException("Jwt:Key", "Secret key for JWT cannot be null or empty.");
            }
            var key = System.Text.Encoding.UTF8.GetBytes(secretKey);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                });
                services.AddAuthorization(options =>
                {
                    options.AddPolicy("ADMIN", policy => policy.RequireRole("ADMIN"));
                    // Agregar otras políticas si es necesario
                });
                
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            //habilitar la autenticación y autorización
            //habilitar la autenticación y autorización
            app.UseAuthentication();
            app.UseAuthorization();
            
            
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();  // Aplica las migraciones si no se han aplicado aún
            }

            app.UseEndpoints(endpoints =>
            {
                  
                endpoints.MapControllers();//mapea las rutas del api
                endpoints.MapHub<PredictionHub>("/predictionHub");//mapea las rutas del hub  
            });
        }
    }
}
