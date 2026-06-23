using CitasApp.Domain.Interfaces;
using CitasApp.Domain.Models;

namespace CitasApp.Application.Services
{
    public class PacienteService
    {
        private readonly IPacienteRepository _repo;
        private readonly List<IPacienteObserver> _observers;

        public PacienteService(IPacienteRepository repo, IEnumerable<IPacienteObserver> observers)
        {
            _repo = repo;
            _observers = observers.ToList();
        }

        public List<Paciente> ObtenerTodos() => _repo.ObtenerTodos();

        public Paciente? ObtenerPorId(int id)
        {
            var paciente = _repo.ObtenerPorId(id);
            if (paciente != null)
                _observers.ForEach(o => o.OnPacienteConsultado(paciente));
            return paciente;
        }
    }
}