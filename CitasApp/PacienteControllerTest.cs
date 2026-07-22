using CitasApp.Domain.Interfaces;
using CitasApp.Domain.Models;
using CitasApp.Web.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace CitasApp.Tests.Controllers;

public class PacienteControllerTest
{
    [Fact]
    public void Index_RegresaTodosLosPacientes()
    {
        // Arrange
        var pacientesEsperados = new List<Paciente>
        {
            new() { Id = 1, Email = "ana@correo.com" },
            new() { Id = 2, Email = "luis@correo.com" }
        };
        var controller = new PacienteController(
            new PacienteRepositoryFake(pacientesEsperados));

        // Act
        var resultado = Assert.IsType<ViewResult>(controller.Index());
        var modelo = Assert.IsType<List<Paciente>>(resultado.Model);

        // Assert
        Assert.Equal(pacientesEsperados, modelo);
    }

    private sealed class PacienteRepositoryFake(List<Paciente> pacientes) : IPacienteRepository
    {
        public List<Paciente> ObtenerTodos() => pacientes;

        public Paciente? ObtenerPorId(int id) =>
            pacientes.FirstOrDefault(paciente => paciente.Id == id);

        public void Agregar(Paciente paciente) => pacientes.Add(paciente);
    }
}