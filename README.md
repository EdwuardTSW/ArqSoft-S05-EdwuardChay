# CitasApp

App de citas medicas construida con ASP.NET Core MVC (.NET 10).

## Arquitectura

Hexagonal (Ports & Adapters) dividida en tres proyectos:

- `CitasApp.Domain` - modelos e interfaces, sin dependencias externas.
- `CitasApp.Infrastructure` - repositorios JSON que implementan las interfaces del dominio.
- `CitasApp.Web` - controladores, views, configuracion e inyeccion de dependencias MVC.

## Flujo de dependencias

```text
Web -> Domain -> Infrastructure
```

## Entidades

- `Paciente` - lista y detalle de pacientes registrados.
- `Medico` - lista y detalle de medicos disponibles.
- `Cita` - agenda completa y filtro por paciente.

## Persistencia

Archivos JSON en `CitasApp.Web/data/`:

- `pacientes.json`
- `medicos.json`
- `citas.json`

Los repositorios `JsonPacienteRepository`, `JsonMedicoRepository` y `JsonCitaRepository` leen los archivos JSON y entregan los datos a los controladores mediante interfaces del dominio.

## Navegacion

- `/Paciente` - lista de pacientes.
- `/Paciente/Detalle/{id}` - detalle de un paciente especifico.
- `/Medico` - lista de medicos.
- `/Medico/Detalle/{id}` - detalle de un medico especifico.
- `/Cita` - agenda completa.
- `/Cita/PorPaciente?pacienteId=1` - citas de un paciente especifico.

## Requisitos

- .NET 10.0
- Visual Studio 2022

## Patrones GOF y resiliencia aplicados

### Factory Method

Se aplico en `CitasApp.Infrastructure/Factories/PacienteRepositoryFactory.cs` para decidir que implementacion de `IPacienteRepository` usar segun el entorno.

- `Production` usa `SqlitePacienteRepository`.
- Cualquier otro entorno usa `JsonPacienteRepository`.

Esto evita cambiar controladores o servicios cuando cambia la persistencia entre desarrollo y produccion.

### Decorator

Se aplico en `CitasApp.Infrastructure/Decorators/LoggingPacienteRepository.cs` para agregar logs a las operaciones de pacientes sin modificar los repositorios reales.

El registro se hace en `CitasApp.Api/Program.cs`: primero la Factory crea el repositorio real y despues el Decorator lo envuelve para registrar las operaciones.

### Observer

Se aplico en la confirmacion de citas.

- `CitasApp.Domain/Interfaces/ICitaObserver.cs` define el contrato.
- `SmsCitaObserver`, `EmailCitaObserver` y `DashboardCitaObserver` reaccionan al evento.
- `CitaService` notifica a todos los observers cuando una cita cambia a `Confirmada`.

Esto evita que el servicio principal dependa directamente de correo, SMS o dashboard.

### Resiliencia Cloud-Native

La practica simula conceptos cloud-native:

- Factory Method representa configuracion por entorno, como variables de entorno en despliegues cloud.
- Decorator representa logs centralizables, equivalentes a CloudWatch Logs.
- Observer representa notificaciones desacopladas, similares a eventos con SNS/SQS.

## Ramas

- `main` - estado inicial con persistencia JSON en un solo proyecto.
- `hexagonal` - arquitectura hexagonal multi-proyecto.
