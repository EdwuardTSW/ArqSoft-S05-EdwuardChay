using CitasApp.Domain.Interfaces;
using CitasApp.Domain.Models;

namespace CitasApp.Infrastructure.Observers
{
    public class EmailCitaObserver : ICitaObserver
    {
        public void OnCitaConfirmada(Cita cita)
        {
            Console.WriteLine($"[{DateTime.Now}] [EMAIL] Confirmacion enviada al paciente {cita.PacienteId} - cita #{cita.Id} - estado: {cita.Estado}");
        }
    }
}
