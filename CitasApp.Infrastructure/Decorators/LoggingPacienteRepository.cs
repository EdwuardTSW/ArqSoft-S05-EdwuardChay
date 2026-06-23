using CitasApp.Domain.Interfaces;
using CitasApp.Domain.Models;

namespace CitasApp.Infrastructure.Decorators
{
    public class LoggingPacienteRepository : IPacienteRepository
    {
        private readonly IPacienteRepository _inner;

        public LoggingPacienteRepository(IPacienteRepository inner)
        {
            _inner = inner;
        }

        public List<Paciente> ObtenerTodos()
        {
            Console.WriteLine("[DECORATOR] ObtenerTodos - inicio");
            var result = _inner.ObtenerTodos();
            Console.WriteLine($"[DECORATOR] ObtenerTodos - {result.Count} registros encontrados");
            return result;
        }

        public Paciente? ObtenerPorId(int id)
        {
            Console.WriteLine($"[DECORATOR] ObtenerPorId({id}) - inicio");
            var result = _inner.ObtenerPorId(id);
            Console.WriteLine($"[DECORATOR] ObtenerPorId({id}) - {(result != null ? "encontrado" : "no encontrado")}");
            return result;
        }
    }
}