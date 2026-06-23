using CitasApp.Domain.Interfaces;
using CitasApp.Domain.Models;
using System.Text.Json;

namespace CitasApp.Infrastructure.Repositories
{
    public class JsonCitaRepository : ICitaRepository
    {
        private readonly string _path;
        private readonly JsonSerializerOptions _options = new() { WriteIndented = true };

        public JsonCitaRepository(string dataPath)
        {
            _path = Path.Combine(dataPath, "citas.json");
        }

        public List<Cita> ObtenerTodos()
        {
            if (!File.Exists(_path)) return new();
            var json = File.ReadAllText(_path);
            var citasJson = JsonSerializer.Deserialize<List<CitaJson>>(json, _options) ?? new();
            return citasJson.Select(c => new Cita
            {
                Id = c.Id,
                PacienteId = c.PacienteId,
                MedicoId = c.MedicoId,
                Fecha = DateOnly.Parse(c.Fecha),
                Hora = TimeOnly.Parse(c.Hora),
                Motivo = c.Motivo,
                Estado = c.Estado
            }).ToList();
        }

        public List<Cita> ObtenerPorPaciente(int pacienteId) =>
            ObtenerTodos().Where(c => c.PacienteId == pacienteId).ToList();

        public Cita? Confirmar(int citaId)
        {
            var citas = ObtenerTodos();
            var cita = citas.FirstOrDefault(c => c.Id == citaId);
            if (cita == null) return null;

            cita.Estado = "Confirmada";

            var citasJson = citas.Select(c => new CitaJson
            {
                Id = c.Id,
                PacienteId = c.PacienteId,
                MedicoId = c.MedicoId,
                Fecha = c.Fecha.ToString("yyyy-MM-dd"),
                Hora = c.Hora.ToString("HH:mm"),
                Motivo = c.Motivo,
                Estado = c.Estado
            }).ToList();

            File.WriteAllText(_path, JsonSerializer.Serialize(citasJson, _options));
            return cita;
        }
    }
}