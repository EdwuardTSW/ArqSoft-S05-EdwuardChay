using CitasApp.Application.Services;
using CitasApp.Domain.Interfaces;
using CitasApp.Infrastructure.Decorators;
using CitasApp.Infrastructure.Factories;
using CitasApp.Infrastructure.Observers;
using CitasApp.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

var dataPath = Path.Combine(builder.Environment.ContentRootPath, "data");
var entorno = builder.Environment.EnvironmentName;

// --- PACIENTES con Factory + Decorator ---
builder.Services.AddScoped<IPacienteRepository>(_ =>
{
    var repo = PacienteRepositoryFactory.CrearPorEntorno(entorno, dataPath);
    return new LoggingPacienteRepository(repo);
});

builder.Services.AddScoped<IPacienteObserver, SmsPacienteObserver>();
builder.Services.AddScoped<IPacienteObserver, EmailPacienteObserver>();
builder.Services.AddScoped<PacienteService>();

// --- MEDICOS y CITAS ---
builder.Services.AddScoped<IMedicoRepository>(_ => new JsonMedicoRepository(dataPath));
builder.Services.AddScoped<ICitaRepository>(_ => new JsonCitaRepository(dataPath));

// Observer: notificaciones desacopladas cuando una cita se confirma.
builder.Services.AddScoped<ICitaObserver, SmsCitaObserver>();
builder.Services.AddScoped<ICitaObserver, EmailCitaObserver>();
builder.Services.AddScoped<ICitaObserver, DashboardCitaObserver>();
builder.Services.AddScoped<MedicoService>();
builder.Services.AddScoped<CitaService>();

var app = builder.Build();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
