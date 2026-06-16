using CitasApp.Application.Services;
using CitasApp.Domain.Interfaces;
using CitasApp.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var dataPath = Path.Combine(builder.Environment.ContentRootPath, "data");

// Repositorios
builder.Services.AddScoped<IPacienteRepository>(_ => new JsonPacienteRepository(dataPath));
builder.Services.AddScoped<IMedicoRepository>(_ => new JsonMedicoRepository(dataPath));
builder.Services.AddScoped<ICitaRepository>(_ => new JsonCitaRepository(dataPath));

// Servicios de aplicación
builder.Services.AddScoped<PacienteService>();
builder.Services.AddScoped<MedicoService>();
builder.Services.AddScoped<CitaService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
