using CitasApp.Application.Services;
using CitasApp.Domain.Interfaces;
using CitasApp.Infrastructure.Decorators;
using CitasApp.Infrastructure.Factories;
using CitasApp.Infrastructure.Observers;
using CitasApp.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

var dataPath = Path.Combine(builder.Environment.ContentRootPath, "data");

// --- PACIENTES con Factory + Decorator + Observer ---
builder.Services.AddScoped<IPacienteRepository>(_ =>
{
    var repo = PacienteRepositoryFactory.Crear("json", dataPath); // cambia a "csv" o "sqlite" si quieres
    return new LoggingPacienteRepository(repo);
});

builder.Services.AddScoped<IPacienteObserver, SmsPacienteObserver>();
builder.Services.AddScoped<IPacienteObserver, EmailPacienteObserver>();
builder.Services.AddScoped<PacienteService>();

// --- Médicos y Citas sin cambios ---
builder.Services.AddScoped<IMedicoRepository>(_ => new JsonMedicoRepository(dataPath));
builder.Services.AddScoped<ICitaRepository>(_ => new JsonCitaRepository(dataPath));
builder.Services.AddScoped<MedicoService>();
builder.Services.AddScoped<CitaService>();

var app = builder.Build();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();