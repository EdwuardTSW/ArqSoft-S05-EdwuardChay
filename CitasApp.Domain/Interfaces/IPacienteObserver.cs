using CitasApp.Domain.Models;

namespace CitasApp.Domain.Interfaces
{
    public interface IPacienteObserver
    {
        void OnPacienteConsultado(Paciente paciente);
    }
}