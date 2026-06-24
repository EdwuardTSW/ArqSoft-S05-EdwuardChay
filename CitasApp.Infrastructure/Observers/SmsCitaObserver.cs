using CitasApp.Domain.Interfaces;
using CitasApp.Domain.Models;

namespace CitasApp.Infrastructure.Observers
{
    public class SmsCitaObserver : ICitaObserver
    {
        public void OnCitaConfirmada(Cita cita)
        {
            Console.WriteLine($"[{DateTime.Now}] [SMS] Recordatorio enviado al paciente {cita.PacienteId} - cita #{cita.Id} el {cita.Fecha:yyyy-MM-dd} a las {cita.Hora:HH:mm}");
        }
    }
}
