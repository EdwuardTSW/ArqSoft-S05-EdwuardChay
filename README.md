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

## Ramas

- `main` - estado inicial con persistencia JSON en un solo proyecto.
- `hexagonal` - arquitectura hexagonal multi-proyecto.
