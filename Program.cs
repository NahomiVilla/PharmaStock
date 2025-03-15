using PharmaStock;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Carga la configuración desde Startup.cs
var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);  // Configura los servicios

var app = builder.Build();

// Configura el pipeline HTTP desde Startup.cs
startup.Configure(app, builder.Environment);

app.Run();
