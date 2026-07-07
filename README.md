# CitasApp

Aplicacion de citas medicas construida con ASP.NET Core y .NET 10. El proyecto incluye una interfaz web MVC, una API REST y una separacion por capas para dominio, casos de uso e infraestructura.

## Documentacion

- [ADR-01: Componentes principales de CitasApp](docs/adr/ADR-01-componentes-citasapp.md)

## Estado actual

La solucion esta compuesta por cinco proyectos:

- `CitasApp.Domain` - modelos del dominio e interfaces de repositorios y observers.
- `CitasApp.Application` - servicios de aplicacion para pacientes, medicos y citas.
- `CitasApp.Infrastructure` - repositorios JSON, CSV y SQLite; factory, decorator y observers concretos.
- `CitasApp.Api` - API REST para consultar y confirmar citas.
- `CitasApp.Web` - aplicacion MVC con controladores y vistas Razor.

El proyecto conserva una orientacion hexagonal mediante interfaces en el dominio y adaptadores en infraestructura. En el estado actual, la API usa la capa `Application`, mientras que la aplicacion Web MVC consume directamente los repositorios a traves de las interfaces del dominio.

## Arquitectura

```text
CitasApp.Api -> CitasApp.Application -> CitasApp.Domain <- CitasApp.Infrastructure
CitasApp.Web -----------------------> CitasApp.Domain <- CitasApp.Infrastructure
```

Componentes principales:

- `CitasApp.Api` recibe peticiones HTTP REST y delega casos de uso a servicios de aplicacion.
- `CitasApp.Web` sirve paginas MVC y consulta datos mediante repositorios inyectados.
- `CitasApp.Application` coordina casos de uso como consultar pacientes, consultar medicos, listar citas y confirmar citas.
- `CitasApp.Domain` define entidades (`Paciente`, `Medico`, `Cita`) y contratos (`IPacienteRepository`, `IMedicoRepository`, `ICitaRepository`, `ICitaObserver`, `IPacienteObserver`).
- `CitasApp.Infrastructure` implementa los contratos del dominio y contiene los detalles de persistencia y notificacion.

## Entidades

- `Paciente` - representa pacientes registrados con nombre, apellido, email y telefono.
- `Medico` - representa medicos disponibles con especialidad y numero de licencia.
- `Cita` - representa una cita medica con paciente, medico, fecha, hora, motivo y estado.

## Persistencia

El proyecto tiene tres mecanismos de persistencia implementados en `CitasApp.Infrastructure`:

- JSON: `JsonPacienteRepository`, `JsonMedicoRepository`, `JsonCitaRepository`.
- CSV: `CsvPacienteRepository`, `CsvMedicoRepository`, `CsvCitaRepository`.
- SQLite: `SqlitePacienteRepository`, `SqliteMedicoRepository`, `SqliteCitaRepository`.

Uso actual por proyecto:

- `CitasApp.Api` usa archivos JSON ubicados en `CitasApp.Api/data/`.
- `CitasApp.Web` registra repositorios CSV ubicados en `CitasApp.Web/data/`.
- `PacienteRepositoryFactory` selecciona `SqlitePacienteRepository` en entorno `Production` y `JsonPacienteRepository` en otros entornos para la API.

Nota: en el estado actual, `CitasApp.Web/data/pacientes.csv` y `CitasApp.Web/data/citas.csv` solo contienen encabezados, por lo que la interfaz MVC puede mostrar pacientes y citas vacios aunque existan datos JSON.

## API REST

Endpoints principales de `CitasApp.Api`:

- `GET /api/pacientes` - lista pacientes.
- `GET /api/pacientes/{id}` - obtiene un paciente por id.
- `GET /api/medicos` - lista medicos.
- `GET /api/medicos/{id}` - obtiene un medico por id.
- `GET /api/citas` - lista citas.
- `GET /api/citas/porpaciente/{pacienteId}` - lista citas por paciente.
- `POST /api/citas/confirmar/{citaId}` - confirma una cita y notifica observers.

## Navegacion Web MVC

Rutas principales de `CitasApp.Web`:

- `/` - pagina de inicio.
- `/Paciente` - lista de pacientes.
- `/Paciente/Detalle/{id}` - detalle de un paciente.
- `/Medico` - lista de medicos.
- `/Medico/Detalle/{id}` - detalle de un medico.
- `/Cita` - agenda completa.
- `/Cita/PorPaciente?pacienteId=1` - citas de un paciente.

## Patrones GOF aplicados

### Factory Method

Implementado en `CitasApp.Infrastructure/Factories/PacienteRepositoryFactory.cs`.

Permite decidir que implementacion de `IPacienteRepository` usar segun el entorno o tipo solicitado:

- `Production` usa `SqlitePacienteRepository`.
- Otros entornos usan `JsonPacienteRepository`.
- Tambien puede crear repositorios `csv`, `sqlite` o `json` mediante `Crear`.

### Decorator

Implementado en `CitasApp.Infrastructure/Decorators/LoggingPacienteRepository.cs`.

Agrega logs a operaciones de pacientes sin modificar el repositorio real. En `CitasApp.Api/Program.cs`, el repositorio creado por la factory se envuelve con este decorator.

### Observer

Implementado para eventos de citas y pacientes.

- `ICitaObserver` define notificaciones cuando una cita se confirma.
- `SmsCitaObserver`, `EmailCitaObserver` y `DashboardCitaObserver` reaccionan a confirmaciones de citas.
- `IPacienteObserver` define notificaciones cuando se consulta un paciente.
- `SmsPacienteObserver` y `EmailPacienteObserver` reaccionan a consultas de pacientes.

## Requisitos

- .NET SDK 10.0
- Visual Studio 2022 o un editor compatible con proyectos .NET

## Ejecucion

Restaurar y compilar la solucion:

```bash
dotnet restore CitasApp.sln
dotnet build CitasApp.sln
```

Ejecutar la API:

```bash
dotnet run --project CitasApp.Api
```

Ejecutar la aplicacion Web MVC:

```bash
dotnet run --project CitasApp.Web
```

## Consideraciones tecnicas

- No hay proyecto de pruebas automatizadas en el estado actual.
- La API y la Web usan fuentes de datos distintas, por lo que pueden mostrar informacion diferente.
- La Web MVC aun no pasa por `CitasApp.Application`; consume repositorios directamente.
- Los repositorios CSV usan parsing simple por comas, por lo que no son adecuados para datos con comas dentro de campos.
