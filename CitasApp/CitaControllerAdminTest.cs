using System.Security.Claims;
using CitasApp.Domain.Interfaces;
using CitasApp.Domain.Models;
using CitasApp.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CitasApp.Tests.Controllers
{
    public class CitaControllerAdminTest
    {
        [Fact]
        public void Index_ConCuentaAdmin_RegresaTodasLasCitasSinFiltrar()
        {
            var controller = CrearControllerConDatosDePrueba(out var citasEsperadas);

            var resultado = Assert.IsType<ViewResult>(controller.Index());
            var modelo = Assert.IsType<List<Cita>>(resultado.Model);

            Assert.Equal(citasEsperadas, modelo);
        }

        [Fact]
        public void Index_ConCuentaAdmin_IncluyeCitasDeMasDeUnPaciente()
        {
            var controller = CrearControllerConDatosDePrueba(out _);

            var resultado = Assert.IsType<ViewResult>(controller.Index());
            var modelo = Assert.IsType<List<Cita>>(resultado.Model);

            Assert.True(modelo.Select(cita => cita.PacienteId).Distinct().Count() > 1);
        }

        [Fact]
        public void Index_ConCuentaAdmin_CargaCatalogosDePacientesYMedicosEnViewBag()
        {
            var controller = CrearControllerConDatosDePrueba(out _);

            var resultado = Assert.IsType<ViewResult>(controller.Index());

            Assert.IsType<List<Paciente>>(resultado.ViewData["Pacientes"]);
            Assert.IsType<List<Medico>>(resultado.ViewData["Medicos"]);
        }

        private static CitaController CrearControllerConDatosDePrueba(
            out List<Cita> citasEsperadas)
        {
            citasEsperadas =
            [
                new Cita { Id = 1, PacienteId = 10, Estado = "Pendiente" },
                new Cita { Id = 2, PacienteId = 20, Estado = "Confirmada" },
                new Cita { Id = 3, PacienteId = 10, Estado = "Pendiente" }
            ];
            List<Paciente> pacientes =
            [
                new Paciente { Id = 10, Email = "paciente1@correo.com" },
                new Paciente { Id = 20, Email = "paciente2@correo.com" }
            ];
            List<Medico> medicos =
            [
                new Medico { Id = 1, Nombre = "Dr. Perez" }
            ];

            var controller = new CitaController(
                new CitaRepositoryFake(citasEsperadas),
                new PacienteRepositoryFake(pacientes),
                new MedicoRepositoryFake(medicos));
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, "jorge@admin.com"),
                new(ClaimTypes.Role, "Admin")
            };
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuth"))
                }
            };

            return controller;
        }

        private sealed class CitaRepositoryFake(List<Cita> citas) : ICitaRepository
        {
            public List<Cita> ObtenerTodos() => citas;

            public List<Cita> ObtenerPorPaciente(int pacienteId) =>
                citas.Where(cita => cita.PacienteId == pacienteId).ToList();

            public void Agregar(Cita cita) => citas.Add(cita);

            public Cita? Confirmar(int citaId)
            {
                var cita = citas.FirstOrDefault(item => item.Id == citaId);
                if (cita is not null)
                {
                    cita.Estado = "Confirmada";
                }

                return cita;
            }
        }

        private sealed class PacienteRepositoryFake(List<Paciente> pacientes) : IPacienteRepository
        {
            public List<Paciente> ObtenerTodos() => pacientes;

            public Paciente? ObtenerPorId(int id) =>
                pacientes.FirstOrDefault(paciente => paciente.Id == id);

            public void Agregar(Paciente paciente) => pacientes.Add(paciente);
        }

        private sealed class MedicoRepositoryFake(List<Medico> medicos) : IMedicoRepository
        {
            public List<Medico> ObtenerTodos() => medicos;

            public Medico? ObtenerPorId(int id) =>
                medicos.FirstOrDefault(medico => medico.Id == id);

            public void Agregar(Medico medico) => medicos.Add(medico);
        }
    }
}
