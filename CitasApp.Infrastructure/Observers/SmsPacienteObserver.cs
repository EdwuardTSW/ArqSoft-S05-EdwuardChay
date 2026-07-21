using CitasApp.Domain.Interfaces;
using CitasApp.Domain.Models;

namespace CitasApp.Infrastructure.Observers
{
    public class SmsPacienteObserver : IPacienteObserver
    {
        public void OnPacienteConsultado(Paciente paciente)
        {
            Console.WriteLine($"[SMS] Notificación - Paciente consultado: {paciente.Nombre} {paciente.Apellido} | Tel: {paciente.Telefono}");
        }
    }
}