# Actividad #32 - Practica .NET: Detectar code smells y refactoriza

## Clase analizada

La clase revisada es `CsvCitaRepository`, ubicada en:

`CitasApp.Infrastructure/Repositories/CsvCitaRepository.cs`

Esta clase implementa `ICitaRepository` y se encarga de manejar la persistencia de citas usando un archivo CSV.

## Code smell 1: God Class

### Clase afectada

`CsvCitaRepository`

### Codigo original afectado

```csharp
// CitasApp.Infrastructure/Repositories/CsvCitaRepository.cs
// Adapter de salida — implementa ICitaRepository leyendo un archivo CSV
//
// Fecha se guarda como  yyyy-MM-dd  (ej: 2026-06-15)
// Hora  se guarda como  HH:mm       (ej: 09:30)

using CitasApp.Domain.Interfaces;
using CitasApp.Domain.Models;

namespace CitasApp.Infrastructure.Repositories
{
    public class CsvCitaRepository : ICitaRepository
    {
        private readonly string _filePath;

        public CsvCitaRepository(string filePath)
        {
            _filePath = filePath;

            if (!File.Exists(_filePath))
                File.WriteAllText(_filePath,
                    "Id,PacienteId,MedicoId,Fecha,Hora,Motivo,Estado\n");
        }

        // ── Helpers ─────────────────────────────────────────────────────────────

        private List<Cita> LeerTodos()
        {
            var lista = new List<Cita>();

            foreach (var linea in File.ReadAllLines(_filePath).Skip(1))
            {
                if (string.IsNullOrWhiteSpace(linea)) continue;
                var p = linea.Split(',');
                if (p.Length < 7) continue;

                lista.Add(new Cita
                {
                    Id         = int.Parse(p[0]),
                    PacienteId = int.Parse(p[1]),
                    MedicoId   = int.Parse(p[2]),
                    Fecha      = DateOnly.ParseExact(p[3], "yyyy-MM-dd"),
                    Hora       = TimeOnly.ParseExact(p[4], "HH:mm"),
                    Motivo     = p[5],
                    Estado     = p[6]
                });
            }

            return lista;
        }

        private void EscribirTodos(List<Cita> citas)
        {
            var lineas = new List<string>
                { "Id,PacienteId,MedicoId,Fecha,Hora,Motivo,Estado" };

            foreach (var c in citas)
            {
                lineas.Add(
                    $"{c.Id}," +
                    $"{c.PacienteId}," +
                    $"{c.MedicoId}," +
                    $"{c.Fecha:yyyy-MM-dd}," +
                    $"{c.Hora:HH:mm}," +
                    $"{Limpiar(c.Motivo)}," +
                    $"{Limpiar(c.Estado)}"
                );
            }

            File.WriteAllLines(_filePath, lineas);
        }

        private static string Limpiar(string texto) =>
            (texto ?? string.Empty).Replace(",", ";");

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
```

### Por que es un code smell

`CsvCitaRepository` puede considerarse una God Class porque concentra demasiadas responsabilidades dentro de una sola clase. Aunque su objetivo principal deberia ser actuar como repositorio de citas, actualmente tambien realiza tareas adicionales relacionadas con archivos, validacion, parsing, serializacion y reglas de actualizacion.

### Responsabilidades concentradas

La clase realiza varias operaciones distintas:

- Crea el archivo CSV si no existe.
- Lee todas las lineas del archivo.
- Valida lineas vacias o incompletas.
- Divide cada linea CSV por columnas.
- Convierte valores de texto a `int`, `DateOnly` y `TimeOnly`.
- Construye objetos `Cita` a partir del contenido del archivo.
- Convierte objetos `Cita` nuevamente a texto CSV.
- Limpia valores para evitar problemas con comas dentro del CSV.
- Genera el siguiente ID para una cita nueva.
- Cambia el estado de una cita a `Confirmada`.
- Guarda nuevamente todos los datos en el archivo.

### Problema principal

La clase mezcla responsabilidades de persistencia, transformacion de datos, validacion y logica de estado. Esto hace que sea mas dificil de mantener, probar y modificar.

Por ejemplo, si se quisiera cambiar la forma de parsear el CSV, validar mejor los datos o modificar la regla para confirmar una cita, probablemente habria que tocar la misma clase. Esto aumenta el riesgo de introducir errores en funcionalidades que no estaban relacionadas directamente.

### Posible refactorizacion

Una mejora seria separar responsabilidades en componentes mas pequenos, por ejemplo:

- Un parser para convertir una linea CSV en una `Cita`.
- Un serializador para convertir una `Cita` en texto CSV.
- Un componente encargado solo de leer y escribir archivos.
- Dejar `CsvCitaRepository` como coordinador de la persistencia.

## Code smell 2: Long Method

### Metodo afectado

`CsvCitaRepository.LeerTodos()`

### Codigo original afectado

```csharp
private List<Cita> LeerTodos()
{
    var lista = new List<Cita>();

    foreach (var linea in File.ReadAllLines(_filePath).Skip(1))
    {
        if (string.IsNullOrWhiteSpace(linea)) continue;
        var p = linea.Split(',');
        if (p.Length < 7) continue;

        lista.Add(new Cita
        {
            Id         = int.Parse(p[0]),
            PacienteId = int.Parse(p[1]),
            MedicoId   = int.Parse(p[2]),
            Fecha      = DateOnly.ParseExact(p[3], "yyyy-MM-dd"),
            Hora       = TimeOnly.ParseExact(p[4], "HH:mm"),
            Motivo     = p[5],
            Estado     = p[6]
        });
    }

    return lista;
}
```

### Por que es un code smell

El metodo `LeerTodos()` puede considerarse un Long Method porque realiza muchas operaciones diferentes dentro del mismo bloque de codigo. Aunque no es extremadamente largo en cantidad de lineas, concentra demasiados pasos y responsabilidades para un solo metodo.

### Operaciones que realiza

El metodo `LeerTodos()` hace lo siguiente:

- Crea una lista vacia de citas.
- Lee todas las lineas del archivo CSV.
- Omite la primera linea porque corresponde al encabezado.
- Recorre cada linea del archivo.
- Verifica si la linea esta vacia.
- Divide la linea usando comas.
- Valida que la linea tenga suficientes columnas.
- Convierte el ID, paciente y medico a numeros enteros.
- Convierte la fecha a `DateOnly`.
- Convierte la hora a `TimeOnly`.
- Crea una instancia de `Cita`.
- Agrega cada cita valida a la lista.
- Retorna la lista final.

### Problema principal

El metodo combina lectura de archivo, validacion, parsing y mapeo de datos. Esto reduce la legibilidad y hace que el metodo sea mas dificil de probar de forma aislada.

Si ocurre un error al convertir una fecha, dividir columnas o construir el objeto `Cita`, todo el problema queda dentro del mismo metodo, lo que dificulta identificar rapidamente la causa.

### Posible refactorizacion

Una mejora seria extraer parte de la logica a metodos mas pequenos, por ejemplo:

- `EsLineaValida(string linea)`
- `ParsearCita(string linea)`
- `CrearCitaDesdeColumnas(string[] columnas)`

De esta manera, `LeerTodos()` quedaria enfocado solo en coordinar la lectura y delegaria los detalles de validacion y conversion a otros metodos.

## Conclusion

Los dos code smells identificados en `CsvCitaRepository` son:

- God Class: porque la clase concentra demasiadas responsabilidades relacionadas con persistencia, validacion, parsing, serializacion y actualizacion de estado.
- Long Method: porque el metodo `LeerTodos()` realiza muchas operaciones distintas dentro de un mismo bloque de codigo.

Ambos smells afectan la mantenibilidad del proyecto y pueden dificultar futuras modificaciones o pruebas unitarias.
