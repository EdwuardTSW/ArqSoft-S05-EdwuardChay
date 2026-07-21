using CitasApp.Domain.Interfaces;
using CitasApp.Domain.Models;

namespace CitasApp.Infrastructure.Observers
{
    public class DashboardCitaObserver : ICitaObserver
    {
        public void OnCitaConfirmada(Cita cita)
        {
            Console.WriteLine($"[{DateTime.Now}] [DASHBOARD] Cita #{cita.Id} marcada como {cita.Estado}");
        }
    }
}
