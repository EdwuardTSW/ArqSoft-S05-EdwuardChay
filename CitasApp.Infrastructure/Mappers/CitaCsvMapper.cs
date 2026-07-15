using CitasApp.Domain.Models;

namespace CitasApp.Infrastructure.Mappers
{
    public static class CitaCsvMapper
    {
        public const string Encabezado = "Id,PacienteId,MedicoId,Fecha,Hora,Motivo,Estado";

        public static Cita? DesdeLinea(string linea)
        {
            if (string.IsNullOrWhiteSpace(linea)) return null;

            var partes = linea.Split(',');
            if (partes.Length < 7) return null;

            return new Cita
            {
                Id = int.Parse(partes[0]),
                PacienteId = int.Parse(partes[1]),
                MedicoId = int.Parse(partes[2]),
                Fecha = DateOnly.ParseExact(partes[3], "yyyy-MM-dd"),
                Hora = TimeOnly.ParseExact(partes[4], "HH:mm"),
                Motivo = partes[5],
                Estado = partes[6]
            };
        }

        public static string ALinea(Cita cita) =>
            $"{cita.Id}," +
            $"{cita.PacienteId}," +
            $"{cita.MedicoId}," +
            $"{cita.Fecha:yyyy-MM-dd}," +
            $"{cita.Hora:HH:mm}," +
            $"{Limpiar(cita.Motivo)}," +
            $"{Limpiar(cita.Estado)}";

        private static string Limpiar(string texto) =>
            (texto ?? string.Empty).Replace(",", ";");
    }
}
