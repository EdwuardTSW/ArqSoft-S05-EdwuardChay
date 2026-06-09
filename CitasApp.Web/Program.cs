using CitasApp.Domain.Interfaces;
using CitasApp.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var dataPath = Path.Combine(builder.Environment.ContentRootPath, "data");
builder.Services.AddScoped<IPacienteRepository>(_ => new JsonPacienteRepository(dataPath));
builder.Services.AddScoped<IMedicoRepository>(_ => new JsonMedicoRepository(dataPath));
builder.Services.AddScoped<ICitaRepository>(_ => new JsonCitaRepository(dataPath));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
