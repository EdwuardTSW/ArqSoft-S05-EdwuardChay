using CitasApp.Domain.Models;

namespace CitasApp.Domain.Interfaces
{
    public interface ICitaObserver
    {
        void OnCitaConfirmada(Cita cita);
    }
}
