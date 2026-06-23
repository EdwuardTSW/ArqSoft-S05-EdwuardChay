using CitasApp.Domain.Interfaces;
using CitasApp.Domain.Models;

namespace CitasApp.Application.Services
{
    public class CitaService
    {
        private readonly ICitaRepository _repo;

        public CitaService(ICitaRepository repo)
        {
            _repo = repo;
        }

        public List<Cita> ObtenerTodos() => _repo.ObtenerTodos();

        public List<Cita> ObtenerPorPaciente(int pacienteId) => _repo.ObtenerPorPaciente(pacienteId);

        public Cita? Confirmar(int citaId)
        {
            var cita = _repo.Confirmar(citaId);
            if (cita != null)
            {
                Console.WriteLine($"[SMS] Recordatorio enviado al paciente {cita.PacienteId} - cita el {cita.Fecha} a las {cita.Hora}");
                Console.WriteLine($"[EMAIL] Confirmación enviada al paciente {cita.PacienteId} - motivo: {cita.Motivo} - estado: {cita.Estado}");
            }
            return cita;
        }
    }
}