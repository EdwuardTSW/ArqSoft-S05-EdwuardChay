using CitasApp.Domain.Interfaces;
using CitasApp.Infrastructure.Repositories;

namespace CitasApp.Infrastructure.Factories
{
    public static class PacienteRepositoryFactory
    {
        public static IPacienteRepository Crear(string tipo, string dataPath)
        {
            Console.WriteLine($"[FACTORY] Creando repositorio de tipo: {tipo}");

            return tipo switch
            {
                "csv" => new CsvPacienteRepository(dataPath),
                "sqlite" => new SqlitePacienteRepository(dataPath),
                _ => new JsonPacienteRepository(dataPath)
            };
        }
    }
}