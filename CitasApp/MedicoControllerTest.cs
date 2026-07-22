using CitasApp.Domain.Interfaces;
using CitasApp.Domain.Models;
using CitasApp.Web.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace CitasApp.Tests.Controllers;

public class MedicoControllerTest
{
    [Fact]
    public void Index_RegresaTodosLosMedicos()
    {
        // Arrange
        var medicosEsperados = new List<Medico>
        {
            new() { Id = 1, Nombre = "Dr. Pérez" },
            new() { Id = 2, Nombre = "Dra. López" }
        };
        var controller = new MedicoController(
            new MedicoRepositoryFake(medicosEsperados));

        // Act
        var resultado = Assert.IsType<ViewResult>(controller.Index());
        var modelo = Assert.IsType<List<Medico>>(resultado.Model);

        // Assert
        Assert.Equal(medicosEsperados, modelo);
    }

    private sealed class MedicoRepositoryFake(List<Medico> medicos) : IMedicoRepository
    {
        public List<Medico> ObtenerTodos() => medicos;

        public Medico? ObtenerPorId(int id) =>
            medicos.FirstOrDefault(medico => medico.Id == id);

        public void Agregar(Medico medico) => medicos.Add(medico);
    }
}