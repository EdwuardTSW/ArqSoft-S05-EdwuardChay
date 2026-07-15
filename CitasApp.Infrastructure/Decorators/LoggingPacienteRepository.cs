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
            Console.WriteLine($"[{DateTime.Now}] [DECORATOR] ObtenerTodos pacientes - inicio");
            var result = _inner.ObtenerTodos();
            Console.WriteLine($"[{DateTime.Now}] [DECORATOR] ObtenerTodos pacientes - {result.Count} registros encontrados");
            return result;
        }

        public Paciente? ObtenerPorId(int id)
        {
            Console.WriteLine($"[{DateTime.Now}] [DECORATOR] ObtenerPorId paciente: {id}");
            var result = _inner.ObtenerPorId(id);
            Console.WriteLine($"[{DateTime.Now}] [DECORATOR] ObtenerPorId paciente: {id} - {(result != null ? "encontrado" : "no encontrado")}");
            return result;
        }

        public void Agregar(Paciente paciente)
        {
            Console.WriteLine($"[{DateTime.Now}] [DECORATOR] Agregar paciente - inicio");
            _inner.Agregar(paciente);
            Console.WriteLine($"[{DateTime.Now}] [DECORATOR] Agregar paciente - fin");
        }
    }
}
