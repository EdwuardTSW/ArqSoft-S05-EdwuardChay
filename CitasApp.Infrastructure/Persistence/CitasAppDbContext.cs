using CitasApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CitasApp.Infrastructure.Persistence
{
    public class CitasAppDbContext : DbContext
    {
        public CitasAppDbContext(DbContextOptions<CitasAppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Medico> Medicos { get; set; }
        public DbSet<Cita> Citas { get; set; }
    }
}
