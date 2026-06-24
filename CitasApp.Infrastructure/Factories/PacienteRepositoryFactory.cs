using CitasApp.Domain.Interfaces;
using CitasApp.Infrastructure.Repositories;

namespace CitasApp.Infrastructure.Factories
{
    public static class PacienteRepositoryFactory
    {
        public static IPacienteRepository CrearPorEntorno(string entorno, string dataPath)
        {
            Console.WriteLine($"[{DateTime.Now}] [FACTORY] Entorno detectado: {entorno}");

            return entorno switch
            {
                "Production" => new SqlitePacienteRepository(Path.Combine(dataPath, "citasapp.db")),
                _ => new JsonPacienteRepository(dataPath)
            };
        }

        public static IPacienteRepository Crear(string tipo, string dataPath)
        {
            Console.WriteLine($"[{DateTime.Now}] [FACTORY] Creando repositorio de tipo: {tipo}");

            return tipo switch
            {
                "csv" => new CsvPacienteRepository(Path.Combine(dataPath, "pacientes.csv")),
                "sqlite" => new SqlitePacienteRepository(Path.Combine(dataPath, "citasapp.db")),
                _ => new JsonPacienteRepository(dataPath)
            };
        }
    }
}
