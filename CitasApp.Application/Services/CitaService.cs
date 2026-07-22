using CitasApp.Domain.Interfaces;
using CitasApp.Domain.Models;

namespace CitasApp.Application.Services
{
    // Service class for managing Cita entities, implementing the Observer pattern to notify observers when a Cita is confirmed 1.
    public class CitaService
    {
        private readonly ICitaRepository _repo;
        private readonly List<ICitaObserver> _observers;

        public CitaService(ICitaRepository repo, IEnumerable<ICitaObserver> observers)
        {
            _repo = repo;
            _observers = observers.ToList();
        }

        public List<Cita> ObtenerTodos() => _repo.ObtenerTodos();

        public List<Cita> ObtenerPorPaciente(int pacienteId) => _repo.ObtenerPorPaciente(pacienteId);

        public Cita? Confirmar(int citaId)
        {
            var cita = _repo.Confirmar(citaId);
            if (cita != null)
            {
                foreach (var observer in _observers)
                {
                    observer.OnCitaConfirmada(cita);
                }
            }
            return cita;
        }
    }
}
