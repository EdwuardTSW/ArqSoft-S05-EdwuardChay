// CitasApp.Infrastructure/Repositories/CsvCitaRepository.cs
// Adapter de salida — implementa ICitaRepository leyendo un archivo CSV
//
// Fecha se guarda como  yyyy-MM-dd  (ej: 2026-06-15)
// Hora  se guarda como  HH:mm       (ej: 09:30)

using CitasApp.Domain.Interfaces;
using CitasApp.Domain.Models;
using CitasApp.Infrastructure.Mappers;

namespace CitasApp.Infrastructure.Repositories
{
    public class CsvCitaRepository : ICitaRepository
    {
        private readonly string _filePath;

        public CsvCitaRepository(string filePath)
        {
            _filePath = filePath;

            if (!File.Exists(_filePath))
                File.WriteAllText(_filePath, CitaCsvMapper.Encabezado + "\n");
        }

        // ── Helpers ─────────────────────────────────────────────────────────────

        private List<Cita> LeerTodos()
        {
            var lista = new List<Cita>();

            foreach (var linea in LeerLineasDeDatos())
            {
                var cita = CitaCsvMapper.DesdeLinea(linea);
                if (cita is not null) lista.Add(cita);
            }

            return lista;
        }

        private IEnumerable<string> LeerLineasDeDatos() =>
            File.ReadAllLines(_filePath).Skip(1);

        private void EscribirTodos(List<Cita> citas)
        {
            var lineas = new List<string> { CitaCsvMapper.Encabezado };

            foreach (var c in citas)
            {
                lineas.Add(CitaCsvMapper.ALinea(c));
            }

            File.WriteAllLines(_filePath, lineas);
        }

        // ── Port ────────────────────────────────────────────────────────────────

        public List<Cita> ObtenerTodos() => LeerTodos();

        public Cita? ObtenerPorId(int id) =>
            LeerTodos().FirstOrDefault(c => c.Id == id);

        public List<Cita> ObtenerPorPaciente(int pacienteId) =>
            LeerTodos().Where(c => c.PacienteId == pacienteId).ToList();

        public void Agregar(Cita cita)
        {
            var citas = LeerTodos();
            cita.Id = citas.Count > 0 ? citas.Max(c => c.Id) + 1 : 1;
            citas.Add(cita);
            EscribirTodos(citas);
        }

        public void ConfirmarCita(int id)
        {
            var citas = LeerTodos();
            var cita  = citas.FirstOrDefault(c => c.Id == id);

            if (cita is not null)
            {
                cita.Estado = "Confirmada";
                EscribirTodos(citas);
            }
        }

        public Cita? Confirmar(int citaId)
        {
            var citas = LeerTodos();
            var cita = citas.FirstOrDefault(c => c.Id == citaId);

            if (cita is null) return null;

            cita.Estado = "Confirmada";
            EscribirTodos(citas);
            return cita;
        }
    }
}
