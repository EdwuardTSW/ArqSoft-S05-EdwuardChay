using CitasApp.Domain.Interfaces;
using CitasApp.Domain.Models;

namespace CitasApp.Infrastructure.Observers
{
    public class EmailPacienteObserver : IPacienteObserver
    {
        public void OnPacienteConsultado(Paciente paciente)
        {
            Console.WriteLine($"[EMAIL] Notificación - Paciente consultado: {paciente.Nombre} {paciente.Apellido} | Email: {paciente.Email}");
        }
    }
}